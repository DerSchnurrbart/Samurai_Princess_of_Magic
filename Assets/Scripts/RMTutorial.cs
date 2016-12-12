using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RMTutorial : MonoBehaviour {


    //save the final score here, 
    //   which will be accessed and displayed on game over screen
    public static int score;

    //tutorial progression variables
    bool tap_enabled;
    bool swipe_enabled;

    //Voiceline
    public float sound_speed = 1.0f;
    public AudioSource music;
    public AudioSource a_tap;
    public AudioSource a_up;
    public AudioSource a_down;
    public AudioSource a_left;
    public AudioSource a_right;
    public AudioSource philipe;
    static AudioClip[] philipe_instructions;
    static AudioClip[] philipe_praise;
    static AudioClip[] philipe_scold;

    private AudioSource rhythmDefeatVoice;
    AudioClip rhythmDefeat;
    bool rhythmDefeatVoiceIsPlaying = false;


    public enum Action { tap, swipe_up, swipe_down, swipe_left, swipe_right };
    public List<Action> sequence;
    
    public int difficulty;
    public int prompt_index;
    public int numIncorrectThisCycle = 0;

    public bool valid_input;
    public bool activated;
    public bool triggered;
    public bool looping;
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

        tap_enabled = false;
        swipe_enabled = false;

        //score starts at the maximum lives, 
        //   because for example if a player survives 3 beats 
        //   but then loses all 5 lives, they actually survived 5+3 beats
        print("starting late start");
        prompt_index = 0;
        difficulty = 3;
        looping = false;
        activated = false;
        triggered = false;
        reset_sequences();
        score = 0;

        rhythmDefeatVoice = GetComponent<AudioSource>();

        rhythmDefeat = Resources.Load("Sounds/Voicelines/GameOvers/RhythmDefeat") as AudioClip;
        music.GetComponent<AudioSource>().enabled = false;

        philipe_instructions = new AudioClip[5];
        philipe_instructions[0] = Resources.Load("Sounds/BetaVoicelines/Rhythm/Tutorial/WelcomeTapTheScreen") as AudioClip;
        philipe_instructions[1] = Resources.Load("Sounds/BetaVoicelines/Rhythm/Tutorial/FantasticSwipe") as AudioClip;
        philipe_instructions[2] = Resources.Load("Sounds/BetaVoicelines/Rhythm/Tutorial/TapsToMakeItEasy") as AudioClip;
        philipe_instructions[3] = Resources.Load("Sounds/BetaVoicelines/Rhythm/Tutorial/TrainingComplete") as AudioClip;

        //missing praise line?
        philipe_praise = new AudioClip[2];
        philipe_praise[0] = Resources.Load("Sounds/BetaVoicelines/Rhythm/Tutorial/AboutToChange") as AudioClip;
        philipe_praise[1] = Resources.Load("Sounds/BetaVoicelines/Rhythm/Tutorial/ChangeOneMoreTime") as AudioClip;

        philipe_scold = new AudioClip[3];
        philipe_scold[0] = Resources.Load("Sounds/BetaVoicelines/Rhythm/Tutorial/ThisIsJustSad") as AudioClip;
        philipe_scold[1] = Resources.Load("Sounds/BetaVoicelines/Rhythm/Tutorial/OhGod") as AudioClip;
        philipe_scold[2] = Resources.Load("Sounds/BetaVoicelines/Rhythm/Tutorial/OhMyFlamingo") as AudioClip;

        //automatically start tutorial coroutine
        StartCoroutine(philipe_tutorial());
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        MobileInput.InputType input = MobileInput.getInput();
        //mobile version starts automatically without needing to tap to start

        if (input == MobileInput.InputType.hold)
        {
            SceneManager.LoadScene("TitleScreen");
        }
        if (!triggered)
        {
            triggered = true;
            activated = true;
            music.GetComponent<AudioSource>().enabled = true;
            //music.GetComponent<AudioSource>().Play();
            //StartCoroutine(show_sequence());
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
            //music.GetComponent<AudioSource>().Play();
            triggered = true;
            activated = true;
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
    }


    public void compare_user_input(Action act)
    {
        valid_input = false;
        if (sequence[prompt_index] != act)
        {
            numIncorrectThisCycle++;
        }
        print("number incorrect: " + numIncorrectThisCycle);
        return;
    }

 

    void reset_sequences()
    {
        sequence.Clear();
        for (int i = 0; i < difficulty; i++)
        {
            sequence.Add(Action.tap);
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


    //function to wait for user input
    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
            yield return null;
    }

    public IEnumerator philipe_tutorial() {
        //introduction
        philipe.PlayOneShot(philipe_instructions[0]);
        yield return new WaitForSeconds(philipe_instructions[0].length);
        prompt_user(Action.tap);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Space));
        disable_prompts();

        //the five dance moves
        philipe.PlayOneShot(philipe_instructions[1]);
        yield return new WaitForSeconds(philipe_instructions[1].length);
        a_up.Play();
        prompt_user(Action.swipe_up);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.UpArrow));
        disable_prompts();
        a_down.Play();
        prompt_user(Action.swipe_down);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.DownArrow));
        disable_prompts();
        a_left.Play();
        prompt_user(Action.swipe_left);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.LeftArrow));
        disable_prompts();
        a_right.Play();
        prompt_user(Action.swipe_right);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.RightArrow));
        disable_prompts();

        //start dancing
        philipe.PlayOneShot(philipe_instructions[2]);
        yield return new WaitForSeconds(philipe_instructions[2].length);
        music.Play();
        List<Action> seq_1 = new List<Action>();
        seq_1.Add(Action.tap);
        seq_1.Add(Action.tap);
        seq_1.Add(Action.tap);
        yield return StartCoroutine(tutorial_sequence());

        //tutorial complete
        philipe.PlayOneShot(philipe_instructions[3]);
        yield return new WaitForSeconds(philipe_instructions[3].length + 2f);


        //TODO: return to Rhythm Magic mode menu
        SceneManager.LoadScene("RhythmMagic");
    }


    public IEnumerator tutorial_sequence()
    {
        int insult = 0;
        looping = true;
        print(sequence.Count);
        //loop until user gets final sequence correct
        int num_mutations = 0;
        while (num_mutations < 3)
        {
            if (!music.isPlaying)
                music.Play();
            //loop each sequence 3 times and give update
            for (int i = 0; i < 3; i++)
            {
                numIncorrectThisCycle = 0;
                for (int j = 0; j < sequence.Count; j++)
                {
                    //prompt the user
                    valid_input = true;
                    prompt_index = j;
                    prompt_user(sequence[j]);
                    yield return new WaitForSeconds(inputDelay);
                    disable_prompts();
                    if (valid_input)
                    {
                        //no input counts as incorrect input
                        valid_input = false;
                        numIncorrectThisCycle++;
                    }
                    yield return new WaitForSeconds(timeBetweenBeats);
                }
            }
            music.Stop();
            //has the user succeeded or failed the final cycle of the sequence?
            if (numIncorrectThisCycle > 0)
            {
                //scold player and try same sequence again
                insult = (insult + 1) % 3;
                philipe.PlayOneShot(philipe_scold[insult]);
                yield return new WaitForSeconds(philipe_scold[insult].length);
            }
            else
            {
                //don't want tutorial to take ages
                if (num_mutations == 2)
                    break;

                int complement = 0;
                if (num_mutations == 0)
                {
                    sequence[2] = Action.swipe_up;
                }
                else
                {
                    complement = 1;
                    sequence[1] = Action.swipe_down;
                }
                num_mutations++;
                philipe.PlayOneShot(philipe_praise[complement]);
                yield return new WaitForSeconds(philipe_praise[complement].length);
            }
        }
        //if you get this far, you passed the tutorial!!!
        looping = false;
    }


}
