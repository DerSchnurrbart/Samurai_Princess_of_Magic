using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MemoryAlt : MonoBehaviour
{

    //save the final score here, 
    //   which will be accessed and displayed on game over screen
    public static int score;

    //game over variables
    private AudioSource memoryDefeatVoice;
    AudioClip memoryDefeat;
    bool memoryDefeatVoiceIsPlaying = false;
    public bool gameIsOver = false;
    public bool gameOverScreenLoaded = false;

    public Text results_text;

    public enum Direction { Up, Down, Left, Right };
    private AudioSource direction_noise;
    public GameObject up_arrow;
    public GameObject down_arrow;
    public GameObject left_arrow;
    public GameObject right_arrow;
    public int current_difficulty;
    private AudioClip[] simple_direction;
    private AudioClip[,] ac_direction; //ac == audio clip
    private AudioClip then;
    private AudioClip welcome;
    private AudioClip uhOh;
    private AudioClip nextPath;

    public List<Direction> sequence;
    public List<Direction> user_guess;
    bool isUsersTurn;
    bool inputDelayActive;
    const float inputDelay = 0.1f;

    /******************************************Helper Functions*****************************************/

    //generate a random direction
    private Direction getRandomDirection()
    {
        int result = Random.Range(0, 4);
        return (Direction)result;
    }

    private void populateSequence(int level_num)
    {
        sequence.Clear();
        for (int i = 0; i < level_num; i++)
        {
            sequence.Add(getRandomDirection());
        }
    }

    //function to disable other arrows
    public void disable_arrows()
    {
        //disable all arrows
        up_arrow.GetComponent<SpriteRenderer>().enabled = false;
        down_arrow.GetComponent<SpriteRenderer>().enabled = false;
        left_arrow.GetComponent<SpriteRenderer>().enabled = false;
        right_arrow.GetComponent<SpriteRenderer>().enabled = false;
    }

    //function to light up pressed arrow, returns length audio clip being played
    public float enable_arrow(Direction dir, bool display = false)
    {
        //disable other arrows
        disable_arrows();
        print(dir);
        //enable only one arrow
        switch (dir)
        {
            case Direction.Up:
                up_arrow.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Direction.Down:
                down_arrow.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Direction.Left:
                left_arrow.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Direction.Right:
                right_arrow.GetComponent<SpriteRenderer>().enabled = true;
                break;
            default:
                print("ERROR: unrecognized Direction");
                return 0.0f;
        }

        if (display)
        {
            int index = Random.Range(0, 4);
            switch (dir)
            {
                case Direction.Up:
                    direction_noise.PlayOneShot(ac_direction[0, index], 1.0f);
                    return ac_direction[0, index].length;
                case Direction.Down:
                    direction_noise.PlayOneShot(ac_direction[1, index], 1.0f);
                    return ac_direction[1, index].length;
                case Direction.Left:
                    direction_noise.PlayOneShot(ac_direction[2, index], 1.0f);
                    return ac_direction[2, index].length;
                case Direction.Right:
                    direction_noise.PlayOneShot(ac_direction[3, index], 1.0f);
                    return ac_direction[3, index].length;
                default:
                    print("ERROR: unrecognized Direction");
                    return 0.0f;
            }
        }
        else
        {
            return 0.0f;
        }
    }




    //user input buffer
    public IEnumerator inputController()
    {
        inputDelayActive = true;
        if (user_guess.Count >= sequence.Count) isUsersTurn = false;
        yield return new WaitForSeconds(inputDelay);
        disable_arrows();
        inputDelayActive = false;
    }

    public IEnumerator OnStart()
    {
        direction_noise.PlayOneShot(welcome);
        yield return new WaitForSeconds(welcome.length);
        StartCoroutine(show_sequence());
    }

    public IEnumerator show_sequence()
    {
        int i = 0;
        while (i < sequence.Count)
        {
            //if it's the final instruction in the set, 
            //   subtract one second from the waitforseconds delay so that 
            //   the user can start inputting immediately after the last word finishes
            if (i == sequence.Count - 1)
            {
                yield return new WaitForSeconds(enable_arrow(sequence[i], true) - 1);
            }
            else {
                yield return new WaitForSeconds(enable_arrow(sequence[i], true));
            }
            disable_arrows();
            if (i < sequence.Count - 1) direction_noise.PlayOneShot(then);
            yield return new WaitForSeconds(then.length);
            i++;
        }
        isUsersTurn = true;
    }

    public IEnumerator correctGuess()
    {
        yield return new WaitForSeconds(1.0f);
        results_text.text = "Correct sequence!";
        direction_noise.PlayOneShot(nextPath);
        yield return new WaitForSeconds(nextPath.length);
        results_text.text = "";
        score++;
        StartCoroutine(show_sequence());
    }

    IEnumerator incorrectGuess()
    {
        yield return new WaitForSeconds(1.0f);
        results_text.text = "Incorrect sequence!";
        direction_noise.PlayOneShot(uhOh);
        yield return new WaitForSeconds(uhOh.length);
        user_guess.Clear();
        current_difficulty = 1;
        populateSequence(current_difficulty);
        StartCoroutine(endGame());
    }


    IEnumerator endGame()
    {
        /* This commented code is the old voiceline "You have lost your way"
        //play first voiceline
        if (memoryDefeatVoiceIsPlaying == false)
        {
            memoryDefeatVoiceIsPlaying = true;
            memoryDefeatVoice.PlayOneShot(memoryDefeat);

        }
        //go to game over screen after 3 seconds
        yield return new WaitForSeconds(3);


        */



        //show gameover screen
        if (gameOverScreenLoaded == false)
        {
            gameOverScreenLoaded = true;

            //update before leaving scene
            Load.updateLastPlayedGame(1);
            SceneManager.LoadScene("GameOver");
        }

        yield return new WaitForSeconds(0);

    }

    void HandlePlayerInput()
    {

#if UNITY_ANDROID

        if (MobileInput.getInput() == MobileInput.InputType.up)
        {
            enable_arrow(Direction.Up);
            user_guess.Add(Direction.Up);
            direction_noise.PlayOneShot(simple_direction[0]);
            StartCoroutine(inputController());
        }
        else if (MobileInput.getInput() == MobileInput.InputType.down)
        {
            enable_arrow(Direction.Down);
            user_guess.Add(Direction.Down);
            direction_noise.PlayOneShot(simple_direction[1]);
            StartCoroutine(inputController());
        }
        else if (MobileInput.getInput() == MobileInput.InputType.right)
        {
            enable_arrow(Direction.Right);
            user_guess.Add(Direction.Right);
            direction_noise.PlayOneShot(simple_direction[2]);
            StartCoroutine(inputController());
        }
        else if (MobileInput.getInput() == MobileInput.InputType.left)
        {
            enable_arrow(Direction.Left);
            user_guess.Add(Direction.Left);
            direction_noise.PlayOneShot(simple_direction[3]);
            StartCoroutine(inputController());
        }
#endif

        if (Input.GetKeyDown("up"))
        {
            enable_arrow(Direction.Up);
            user_guess.Add(Direction.Up);
            direction_noise.PlayOneShot(simple_direction[0]);
            StartCoroutine(inputController());
        }
        else if (Input.GetKeyDown("down"))
        {
            enable_arrow(Direction.Down);
            user_guess.Add(Direction.Down);
            direction_noise.PlayOneShot(simple_direction[1]);
            StartCoroutine(inputController());
        }
        else if (Input.GetKeyDown("left"))
        {
            enable_arrow(Direction.Left);
            user_guess.Add(Direction.Left);
            direction_noise.PlayOneShot(simple_direction[2]);
            StartCoroutine(inputController());
        }
        else if (Input.GetKeyDown("right"))
        {
            enable_arrow(Direction.Right);
            user_guess.Add(Direction.Right);
            direction_noise.PlayOneShot(simple_direction[3]);
            StartCoroutine(inputController());
        }

        if (user_guess.Count == sequence.Count) isUsersTurn = false;
        if (user_guess.Count == sequence.Count) HandlePlayerGuess();

    }

    void HandlePlayerGuess()
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            if (user_guess[i] != sequence[i])
            {
                StartCoroutine(incorrectGuess());
                return;
            }
        }

        StartCoroutine(correctGuess());
        populateSequence(++current_difficulty);
        user_guess.Clear();

    }


    /***************************************Automatically called functions*************************/

    void Start()
    {
        score = 0;

        isUsersTurn = false;
        inputDelayActive = false;
        direction_noise = GetComponent<AudioSource>();
        results_text = GameObject.Find("Canvas/Results").GetComponent<Text>();

        then = Resources.Load("Sounds/BetaVoicelines/MemoryGame/THEN") as AudioClip;
        ac_direction = new AudioClip[4, 4];
        ac_direction[0, 0] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeUp1") as AudioClip;
        ac_direction[0, 1] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeUp2") as AudioClip;
        ac_direction[0, 2] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeUp3") as AudioClip;
        ac_direction[0, 3] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeUp4") as AudioClip;
        ac_direction[1, 0] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeDown1") as AudioClip;
        ac_direction[1, 1] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeDown2") as AudioClip;
        ac_direction[1, 2] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeDown3") as AudioClip;
        ac_direction[1, 3] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeDown4") as AudioClip;
        ac_direction[2, 0] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeLeft1") as AudioClip;
        ac_direction[2, 1] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeLeft2") as AudioClip;
        ac_direction[2, 2] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeLeft3") as AudioClip;
        ac_direction[2, 3] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeLeft4") as AudioClip;
        ac_direction[3, 0] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeRight1") as AudioClip;
        ac_direction[3, 1] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeRight2") as AudioClip;
        ac_direction[3, 2] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeRight3") as AudioClip;
        ac_direction[3, 3] = Resources.Load("Sounds/BetaVoicelines/MemoryGame/SwipeRight4") as AudioClip;
        simple_direction = new AudioClip[4];
        simple_direction[0] = Resources.Load("Sounds/Directions/up") as AudioClip;
        simple_direction[1] = Resources.Load("Sounds/Directions/down") as AudioClip;
        simple_direction[2] = Resources.Load("Sounds/Directions/left") as AudioClip;
        simple_direction[3] = Resources.Load("Sounds/Directions/right") as AudioClip;
        welcome = Resources.Load("Sounds/BetaVoicelines/MemoryGame/Welcome") as AudioClip;
        nextPath = Resources.Load("Sounds/BetaVoicelines/MemoryGame/YourNextPathIs") as AudioClip;
        uhOh = Resources.Load("Sounds/BetaVoicelines/MemoryGame/UhOh") as AudioClip;
        //Arcade Mode:
        current_difficulty = 1;
        populateSequence(current_difficulty);
        disable_arrows();

        memoryDefeatVoice = GetComponent<AudioSource>();

        memoryDefeat = Resources.Load("Sounds/Voicelines/GameOvers/MemoryDefeat") as AudioClip;

        StartCoroutine(OnStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsersTurn)
        {
            if (!inputDelayActive)
            {
                HandlePlayerInput();
            }
        }

        //if user hits escape, go back to main menu
        if (Input.GetKeyDown("escape"))
        {
            print("ESCAPE");
            SceneManager.LoadScene("TitleScreen");
        }
        //user should be able to reload scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            print("attempting scene reload");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

    }
}