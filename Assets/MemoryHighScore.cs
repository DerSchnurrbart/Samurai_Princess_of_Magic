using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MemoryHighScore : MonoBehaviour
{
    //Mobile Touch Input
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    Text txt;

    // Use this for initialization
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        txt.text = PlayerPrefs.GetInt(UpdateHighScore.highScoreMemoryKey, 0) + " directions";
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        MobileInput();
        /*
        if (MobileInput.getInput() == MobileInput.InputType.up)
        {
            ChangeScreen.SwitchScreen();
        }
        else if (MobileInput.getInput() == MobileInput.InputType.tap)
        {
            ChangeScreen.SwitchScreen();
        }
        */
#endif

        //if on desktop, enter will go to arcade game screen
        if (Input.GetKeyDown("down"))
        {
            SceneManager.LoadScene("MinigameScreen");
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

                    //check if the drag is vertical
                    if (Mathf.Abs(lp.x - fp.x) < Mathf.Abs(lp.y - fp.y))
                    {
                        if (lp.y < fp.y)
                        {
                            Debug.Log("Down Swipe");
                            SceneManager.LoadScene("MinigameScreen", LoadSceneMode.Single);
                        }
                    }
                }
            }
        }
    }
}
