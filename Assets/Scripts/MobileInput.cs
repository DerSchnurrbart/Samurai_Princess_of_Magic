using UnityEngine;
using System.Collections;

public class MobileInput {

    /*********************************Mobile Touch Input************************************/
    private static Vector3 fp;   //First touch position
    private static Vector3 lp;   //Last touch position
    private static float dragDistance;  //minimum distance for a swipe to be registered
    public enum InputType {left, right, up, down, tap, none};

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
                lp = touch.position;
                //Check if drag distance is greater than 15% of the screen height
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
