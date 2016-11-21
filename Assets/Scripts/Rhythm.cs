using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Rhythm : MonoBehaviour
{

    //save the final score here, 
    //   which will be accessed and displayed on game over screen
    public static int score;

    //Voiceline
    public float sound_speed = 1.0f;
    public AudioSource music;
    public AudioSource a_tap;
    public AudioSource a_up;
    public AudioSource a_down;
    public AudioSource a_left;
    public AudioSource a_right;

    private AudioSource rhythmDefeatVoice;
    AudioClip rhythmDefeat;
    bool rhythmDefeatVoiceIsPlaying = false;


    public enum Action { tap, swipe_up, swipe_down, swipe_left, swipe_right };
    public List<Action> sequence;
    public List<Action> target;
    public List<int> unmutatedPositions;

    public const int CYCLES_NEEDED_TO_PROCEED = 3;
    public const int NUM_MOVES_INCORRECT_TO_FAIL = 5;
    public const float MULTIPLIER_FOR_BEAT_PERIOD = 0.90f;
    public int difficulty;
    public int prompt_index;
    public int correct = 0;
    public int numBeatsThisLevel = 0;
    public int numIncorrectThisCycle = 0;
    public bool valid_input;
    public bool activated;
    public bool triggered;
    public bool looping;
    public bool gameIsOver = false;
    public bool gameOverScreenLoaded = false;
    public float inputDelay = 2.0f;
    public float timeBetweenBeats = 0.2f;

    public GameObject swipe_up;
    public GameObject swipe_down;
    public GameObject swipe_left;
    public GameObject swipe_right;
    public GameObject tap;

    MobileInput mobInput;

    // Use this for initialization
    void Start()
    {
        mobInput = new MobileInput();

        //score starts at the maximum lives, 
        //   because for example if a player survives 3 beats 
        //   but then loses all 5 lives, they actually survived 5+3 beats
        print("starting late start");
        prompt_index = 0;
        //difficulty = 3;
        looping = false;
        activated = false;
        triggered = false;
        reset_sequences();
        score = 0;



        rhythmDefeatVoice = GetComponent<AudioSource>();

        rhythmDefeat = Resources.Load("Sounds/Voicelines/GameOvers/RhythmDefeat") as AudioClip;
        music.GetComponent<AudioSource>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        MobileInput.InputType input = MobileInput.getInput();
        //mobile version starts automatically without needing to tap to start
        if (!triggered)
        {
            triggered = true;
            activated = true;
            music.GetComponent<AudioSource>().enabled = true;
            music.GetComponent<AudioSource>().Play();
            StartCoroutine(show_sequence());
        }
        else if (valid_input)
        {
            if (input == MobileInput.InputType.tap)
            {
                compare_user_input(Action.tap);
            }
            else if (input == MobileInput.InputType.up)
            {
                compare_user_input(Action.swipe_up);
            }
            else if (input == MobileInput.InputType.down)
            {
                compare_user_input(Action.swipe_down);
            }
            else if (input == MobileInput.InputType.left)
            {
                compare_user_input(Action.swipe_left);
            }
            else if (input == MobileInput.InputType.right)
            {
                compare_user_input(Action.swipe_right);
            }
        }
#endif


        //start prompt
        if (!triggered)
        {
            music.GetComponent<AudioSource>().enabled = true;
            music.GetComponent<AudioSource>().Play();
            triggered = true;
            activated = true;
            StartCoroutine(show_sequence());
        }

        //take user input
        else if (valid_input)
        {
            if (Input.GetKeyDown("space"))
            {
                compare_user_input(Action.tap);
            }
            else if (Input.GetKeyDown("up"))
            {
                compare_user_input(Action.swipe_up);
            }
            else if (Input.GetKeyDown("down"))
            {
                compare_user_input(Action.swipe_down);
            }
            else if (Input.GetKeyDown("left"))
            {
                compare_user_input(Action.swipe_left);
            }
            else if (Input.GetKeyDown("right"))
            {
                compare_user_input(Action.swipe_right);
            }
        }

        //keep running prompt
        if (activated && !looping)
        {
            StartCoroutine(show_sequence());
        }

    }

    IEnumerator endGame()
    {

        if (gameOverScreenLoaded == false)
        {
            gameOverScreenLoaded = true;

            //update before leaving scene
            Load.updateLastPlayedGame(3);
            SceneManager.LoadScene("GameOver");
        }

        yield return new WaitForSeconds(0);
    }

    public void compare_user_input(Action act)
    {
        valid_input = false;
        if (sequence[prompt_index] != act)
        {
            numIncorrectThisCycle++;
        }
        else
        {
            correct++;
        }
        numBeatsThisLevel++;
        print("number of beats this level: " + numBeatsThisLevel);
        print("number correct: " + correct);
        print("number incorrect: " + numIncorrectThisCycle);
        if (numBeatsThisLevel % sequence.Count == 0)
        { //end of cycle
            advancePlay();
        }
        return;
    }

    public void advancePlay()
    {
        print("ADVANCEPLAYCALLED");
        if (numIncorrectThisCycle > sequence.Count / 3)
        {
            print("too many incorrect");
            score += correct;
            gameIsOver = true;
            StartCoroutine(endGame());
        }
        else
        {
            print("new cycle, continuing");
            score += correct;
            correct = 0;
            //starting new cycle
            numIncorrectThisCycle = 0;
        }
        if (numBeatsThisLevel == CYCLES_NEEDED_TO_PROCEED * sequence.Count)
        {
            if (unmutatedPositions.Count != 0)
            {
                print("mutating cycle");
                mutate_sequence();
            }
            else
            {
                print("reached next difficulty");
                difficulty++;
                inputDelay *= MULTIPLIER_FOR_BEAT_PERIOD;
                timeBetweenBeats *= MULTIPLIER_FOR_BEAT_PERIOD;
                reset_sequences();
            }
            score += correct;
            correct = 0;
            numBeatsThisLevel = 0;
        }
    }

    void reset_sequences()
    {
        sequence.Clear();
        target.Clear();
        for (int i = 0; i < difficulty; i++)
        {
            int val = Random.Range(1, 4);
            target.Add((Action)val);
            sequence.Add(Action.tap);
            unmutatedPositions.Add(i);
        }
    }

    public void mutate_sequence()
    {
        print("mutation");
        int position = Random.Range(0, unmutatedPositions.Count);
        int positionToMutate = unmutatedPositions[position];
        unmutatedPositions.Remove(unmutatedPositions[position]);
        sequence[positionToMutate] = target[positionToMutate];
    }

    public IEnumerator show_sequence()
    {
        //if game is over, stop showing sequence, and show game over screen instead
        if (gameIsOver == false)
        {
            looping = true;

            //print("entered coroutine");
            if (sequence == null)
            {
                //print("null sequence");
            }
            print(sequence.Count);
            int i = 0;
            while (i < sequence.Count)
            {
                if (gameIsOver)
                {
                    SceneManager.LoadScene("GameOver");
                }
                prompt_index = i;
                prompt_user(sequence[i]);
                valid_input = true;
                yield return new WaitForSeconds(inputDelay);
                disable_prompts();
                if (valid_input)
                {
                    correct = 0;
                    print("no input given");
                    numIncorrectThisCycle++;
                    numBeatsThisLevel++;
                    print("number of beats this level: " + numBeatsThisLevel);
                    print("number correct: " + correct);
                    print("number incorrect: " + numIncorrectThisCycle);
                    if (numBeatsThisLevel % sequence.Count == 0)
                    {
                        advancePlay();
                    }
                    valid_input = false;
                }
                yield return new WaitForSeconds(timeBetweenBeats);
                i++;
            }
            looping = false;
        }

    }


    public void disable_prompts()
    {
        swipe_up.GetComponent<SpriteRenderer>().enabled = false;
        swipe_down.GetComponent<SpriteRenderer>().enabled = false;
        swipe_left.GetComponent<SpriteRenderer>().enabled = false;
        swipe_right.GetComponent<SpriteRenderer>().enabled = false;
        tap.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void prompt_user(Action act)
    {
        disable_prompts();
        switch (act)
        {
            case Action.tap:
                a_tap.GetComponent<AudioSource>().Play();
                tap.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Action.swipe_up:
                a_up.GetComponent<AudioSource>().Play();
                swipe_up.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Action.swipe_down:
                a_down.GetComponent<AudioSource>().Play();
                swipe_down.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Action.swipe_left:
                a_left.GetComponent<AudioSource>().Play();
                swipe_left.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Action.swipe_right:
                a_right.GetComponent<AudioSource>().Play();
                swipe_right.GetComponent<SpriteRenderer>().enabled = true;
                break;
            default:
                print("ERROR: unrecognized action");
                break;
        }
    }
}