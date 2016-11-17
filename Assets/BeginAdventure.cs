using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BeginAdventure : MonoBehaviour {
    //Mobile Touch Input
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    // Use this for initialization
    void Start () {
        //define what % of the screen is needed to be touched for a swipe to register
        dragDistance = Screen.height * 15 / 100;
    }
	
	// Update is called once per frame
	void Update () {

        //if on android, tap will go to arcade game screen
#if UNITY_ANDROID
        MobileInput();
#endif

        //if on desktop, enter will go to arcade game screen
        if (Input.GetKeyDown("return"))
        {
            ChangeScreen.SwitchScreen();
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

                //Check if drag distance is lower than 15% of the screen height
                if (Mathf.Abs(lp.x - fp.x) < dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {

                    //Is a tap, since distance was less than 15% of screen height
                    Debug.Log("Tap");
                    ChangeScreen.SwitchScreen();

                }
            }
        }
    }
}
