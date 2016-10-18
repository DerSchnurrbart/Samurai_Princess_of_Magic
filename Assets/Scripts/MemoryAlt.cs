using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryAlt : MonoBehaviour
{

    public enum Direction { Up, Down, Left, Right };
    public GameObject up_arrow;
    public GameObject down_arrow;
    public GameObject left_arrow;
    public GameObject right_arrow;

    public List<Direction> sequence;
    bool isUsersTurn;
    bool promptingUser;
    const float inputDelay = 0.3f;

    // Use this for initialization
    void Start()
    {
        promptingUser = false;
        isUsersTurn = false;
        sequence.Add(getRandomDirection());
        sequence.Add(getRandomDirection());
        sequence.Add(getRandomDirection());
        sequence.Add(getRandomDirection());
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsersTurn)
        {
            if (Input.GetKey("up"))
            {
                print("up arrow key is held down");
                enable_arrow(Direction.Up);
                StartCoroutine(inputBuffer());
            }

            if (Input.GetKey("down"))
            {
                print("down arrow key is held down");
                enable_arrow(Direction.Down);
                StartCoroutine(inputBuffer());
            }

            if (Input.GetKey("left"))
            {
                print("left arrow key is held down");
                enable_arrow(Direction.Left);
                StartCoroutine(inputBuffer());
            }

            if (Input.GetKey("right"))
            {
                print("right arrow key is held down");
                enable_arrow(Direction.Right);
                StartCoroutine(inputBuffer());
            }
        }
        else if (Input.GetKey("return"))
        {
                print("return key entered");
                isUsersTurn = false;
                promptingUser = true;
                //IMPORTANT: this function, "show_sequence()", is broken
                //show_sequence();
        }
    }


    //generate a random direction
    private Direction getRandomDirection()
    {
        int result = Random.Range(1, 4);
        return (Direction)result;
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
    public IEnumerator inputBuffer()
    {
        isUsersTurn = false;
        yield return new WaitForSeconds(inputDelay);
        //disable_arrows();
        isUsersTurn = true;
    }

    //prompt buffer
    public IEnumerator outputBuffer()
    {
        promptingUser = false;
        yield return new WaitForSeconds(inputDelay);
        //disable_arrows();
        promptingUser = true;
        print("promptingUser: " + promptingUser);
    }

    public void show_sequence() {
        print("sequence count: " + sequence.Count);
        int i = 0;
        while (i < sequence.Count)
        {
            if (promptingUser)
            {
                enable_arrow(sequence[i]);
                print("before: " + promptingUser);
                StartCoroutine(outputBuffer());
                //promptingUser should not be false here
                promptingUser = true;
                print("after: " + promptingUser);
                i++;
            }
        }
        disable_arrows();
        promptingUser = false;
        isUsersTurn = true;
    }



    
}