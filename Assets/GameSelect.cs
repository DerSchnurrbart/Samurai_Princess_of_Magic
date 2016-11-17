using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameSelect : MonoBehaviour
{
    //Mobile Touch Input
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    //To keep track of which game is currently selected
    bool rhythm;
    bool sword;
    bool memory;

    // Use this for initialization
    void Start()
    {
        //define what % of the screen is needed to be touched for a swipe to register
        dragDistance = Screen.height * 15 / 100;

        //no game is selected initially
        rhythm = false;
        sword = false;
        memory = false;
    }

    // Update is called once per frame
    void Update()
    {

        //if on android, tap will go to arcade game screen
#if UNITY_ANDROID
        MobileInput();
#endif

        //if on desktop, press up left or right then enter
        if (Input.GetKeyDown("right"))
        {
            Debug.Log("Right Swipe");
            rhythm = false;
            memory = false;
            sword = true;

        }
        if (Input.GetKeyDown("left"))
        {
            Debug.Log("Left Swipe");
            rhythm = false;
            memory = true;
            sword = false;
        }
        if (Input.GetKeyDown("up"))
        {
            Debug.Log("Up Swipe");
            rhythm = true;
            memory = false;
            sword = false;
        }
        if (Input.GetKeyDown("down"))
        {
            Debug.Log("Down Swipe");
            //Optional Design: swiping down clears selection
            //rhythm = false;
            //memory = false;
            //sword = false;
        }
        //Confirm game selection
        if (Input.GetKeyDown("return"))
        {
            Debug.Log("Tap");
            if (rhythm == true) SceneManager.LoadScene("RhythmMagic");
            if (sword == true) SceneManager.LoadScene("SwordCombat");
            if (memory == true) SceneManager.LoadScene("MemoryGame");

        }
    }

    void MobileInput()
    {
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
                        //the drag is horizontal                          
                        //last touch position was right of first touch position
                           if ((lp.x > fp.x))
                           {
                                Debug.Log("Right Swipe");
                            rhythm = false;
                            memory = false;
                            sword = true;
                            
                        }
                            else
                            {
                                Debug.Log("Left Swipe");
                            rhythm = false;
                            memory = true;
                            sword = false;
                            
                        }
                    }
                    //movement was vertical
                    else
                    {
                        //last touch position is more up of first touch position
                        if (lp.y > fp.y)
                        {
                           Debug.Log("Up Swipe");
                            rhythm = true;
                            memory = false;
                            sword = false;
                            

                        }
                        //movement is a down swipe
                        else
                        {
                            Debug.Log("Down Swipe");
                            //Optional Design: swiping down clears selection
                            //rhythm = false;
                            //memory = false;
                            //sword = false;
                        }
                    }

                }
                //Is a tap, since distance was less than 15% of screen height
                //Confirm game selection
                else
                {
                    Debug.Log("Tap");
                    if (rhythm == true) SceneManager.LoadScene("RhythmMagic");
                    if (sword == true) SceneManager.LoadScene("SwordCombat");
                    if (memory == true) SceneManager.LoadScene("MemoryGame");
                    
                }
            }
        }
    }
}
