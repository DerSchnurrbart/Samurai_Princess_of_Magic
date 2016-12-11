using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MobileInput {

    /*********************************Mobile Touch Input************************************/
    private static Vector3 fp;   //First touch position
    private static Vector3 lp;   //Last touch position
    private static float dragDistance;  //minimum distance for a swipe to be registered
    public enum InputType {left, right, up, down, tap, hold, none};

    private static float holdTime = 3.0f;
    private static float acumTime = 0;

    public MobileInput()
    {
        //define what % of the screen is needed to be touched for a swipe to register
        dragDistance = Screen.height * 8 / 100;
    }

    public static InputType getInput()
    {
        InputType retVal = InputType.none;
        // user is touching the screen with one finger
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            //record how much time the screen is held
            acumTime += Input.GetTouch(0).deltaTime;

            //if screen is held for the minimum length then register a hold input
            if (acumTime >= holdTime)
            {
                retVal = InputType.hold;
            }

            //get coordinates of the first touch
            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }
            //update the last position based on where they moved
            else if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;
            }
            //check if the finger is removed from the screen
            else if (touch.phase == TouchPhase.Ended)
            {
                //input was not a hold for 3 seconds; reset the timer to 0
                acumTime = 0;

                lp = touch.position;
                //Check if drag distance is greater than 8% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    //check if the drag is horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {
                        //last touch position was right of first touch position
                        if ((lp.x > fp.x))
                        {
                            Debug.Log("Right Swipe");
                            retVal = InputType.right;
                        }
                        else
                        {
                            Debug.Log("Left Swipe");
                            retVal = InputType.left;
                        }
                    }
                    //movement was vertical
                    else
                    {
                        //last touch position is more up of first touch position
                        if (lp.y > fp.y)
                        {
                            Debug.Log("Up Swipe");
                            retVal = InputType.up;
                        }
                        //movement is a down swipe
                        else
                        {
                            Debug.Log("Down Swipe");
                            retVal = InputType.down;
                        }
                    }
                }
                //Is a tap, since distance was less than 15% of screen height
                else
                {
                    Debug.Log("Tap");
                    retVal = InputType.tap;
                }
            }
        }
        return retVal;
    }
}
