using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BeginAdventure : MonoBehaviour
{
    //Mobile Touch Input
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    private float holdTime = 5.0f;
    private float acumTime = 0;

    private AudioSource source;
    private AudioClip welcome;
    //helper for triggering game select directions after tapping on title screen

    // Use this for initialization
    void Start()
    {
        //define what % of the screen is needed to be touched for a swipe to register
        dragDistance = Screen.height * 8 / 100;

        source = GetComponent<AudioSource>();
        welcome = Resources.Load("Sounds/BetaVoicelines/Menu/Welcome") as AudioClip;

        StartCoroutine(playSound());
    }

    // Update is called once per frame
    void Update()
    {


        //if on android, tap will go to arcade game screen
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
        if (Input.GetKeyDown("return"))
        {
            SceneManager.LoadScene("MinigameScreen");
        }

    }

    public IEnumerator playSound()
    {
        yield return new WaitForSeconds(0);
        source.PlayOneShot(welcome);
    }

    void MobileInput()
    {
        // user is touching the screen with one finger
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            //record how much time the screen is held
            acumTime += Input.GetTouch(0).deltaTime;

            //if screen is held for the minimum length then register a hold input
            if (acumTime >= holdTime)
            {
                SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
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

                        }
                        else
                        {
                            Debug.Log("Left Swipe");

                        }
                    }
                    //movement was vertical
                    else
                    {
                        //last touch position is more up of first touch position
                        if (lp.y > fp.y)
                        {
                            Debug.Log("Up Swipe");

                        }
                        //movement is a down swipe
                        else
                        {
                            Debug.Log("Down Swipe");
                            SceneManager.LoadScene("Credits");
                        }
                    }

                }
                //was not a swipe
                else
                {
                    Debug.Log("Tap");
                    SceneManager.LoadScene("MinigameScreen");
                }
            }
        }
    }
}
