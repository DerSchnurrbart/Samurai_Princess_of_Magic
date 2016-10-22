using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MemoryAlt : MonoBehaviour
{

    public enum Direction { Up, Down, Left, Right };
    public GameObject up_arrow;
    public GameObject down_arrow;
    public GameObject left_arrow;
    public GameObject right_arrow;
    public int current_difficulty;

    public List<Direction> sequence;
    public List<Direction> user_guess;
    bool isUsersTurn;
    bool inputDelayActive;
    const float inputDelay = 0.5f;

    // Use this for initialization
    void Start()
    {
        isUsersTurn = false;
        inputDelayActive = false;
        
        //Arcade Mode:
        current_difficulty = 1;
        populateSequence(current_difficulty);
        disable_arrows();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsersTurn)
        {
            if (!inputDelayActive)
            {
                if (Input.GetKey("up"))
                {
                    enable_arrow(Direction.Up);
                    user_guess.Add(Direction.Up);
                    StartCoroutine(inputController());
                }
                else if (Input.GetKey("down"))
                {
                    enable_arrow(Direction.Down);
                    user_guess.Add(Direction.Down);
                    StartCoroutine(inputController());
                }
                else if (Input.GetKey("left"))
                {
                    enable_arrow(Direction.Left);
                    user_guess.Add(Direction.Left);
                    StartCoroutine(inputController());
                }
                else if (Input.GetKey("right"))
                {
                    enable_arrow(Direction.Right);
                    user_guess.Add(Direction.Right);
                    StartCoroutine(inputController());
                }
            }
        }
        else if (Input.GetKey("return"))
        {
            StartCoroutine(show_sequence());
        }

        //if user hits escape, go back to main menu
        if (Input.GetKey("escape"))
        {
            print("ESCAPE");
            SceneManager.LoadScene("TitleScreen");
        }
        //user should be able to reload scene
        if (Input.GetKey(KeyCode.R))
        {
            print("attempting scene reload");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (user_guess.Count == sequence.Count)
        {
            //TODO: Add splash screen for loss
            isUsersTurn = false;
            for (int i = 0; i < sequence.Count; i++)
            {
                if (user_guess[i] != sequence[i])
                {
                    print("Incorrect sequence!");
                    //Game over, try again? 
                    return;
                }
            }
            print("Correct sequence!");
            populateSequence(++current_difficulty);
            user_guess.Clear();
        }
    }


    //generate a random direction
    private Direction getRandomDirection()
    {
        int result = Random.Range(0, 4);
        return (Direction)result;
    }

    private void populateSequence(int level_num)
    {
        sequence.Clear();
        for(int i = 0; i < level_num; i++)
        {
            sequence.Add(getRandomDirection());
        }
    }

    //function to disable other arrows
    public void disable_arrows() {
        //disable all arrows
        up_arrow.GetComponent<SpriteRenderer>().enabled = false;
        down_arrow.GetComponent<SpriteRenderer>().enabled = false;
        left_arrow.GetComponent<SpriteRenderer>().enabled = false;
        right_arrow.GetComponent<SpriteRenderer>().enabled = false;
    }

    //function to light up pressed arrow
    public void enable_arrow(Direction dir) {
        //disable other arrows
        disable_arrows();

        //enable only one arrow
        switch (dir) {
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
                break;
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

    public IEnumerator show_sequence() {
        int i = 0;
        while (i < sequence.Count)
        {
            enable_arrow(sequence[i]);
            yield return new WaitForSeconds(inputDelay);
            disable_arrows();
            yield return new WaitForSeconds(0.2f);
            i++;
        }
        disable_arrows();
        isUsersTurn = true;
    }



    
}