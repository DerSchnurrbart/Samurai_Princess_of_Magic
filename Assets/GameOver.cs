using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    //Mobile Touch Input
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    //Audio
    private AudioSource gameOverVoice;
    AudioClip gameOverClip;
    bool gameOverVoiceIsPlaying = false;
    bool gameOverLoaded = false;

    // Use this for initialization
    void Start()
    {
        //define what % of the screen is needed to be touched for a swipe to register
        dragDistance = Screen.height * 15 / 100;

        gameOverVoice = GetComponent<AudioSource>();
        gameOverClip = Resources.Load("Sounds/Voicelines/GameOvers/GameOver") as AudioClip;
    }

    // Update is called once per frame
    void Update()
    {


        if (gameOverVoiceIsPlaying == false)
        {
            gameOverVoiceIsPlaying = true;
            gameOverVoice.PlayOneShot(gameOverClip);
        }

        //If running on Unity Android, run this block to use mobile input controls
#if UNITY_ANDROID
        MobileInput();
#endif
        //Run desktop keyboard/mouse controls

        if (Input.GetKeyDown("up"))
        {
            SceneManager.LoadScene(Load.lastPlayedGame, LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown("down"))
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }

        //This function returns the game to main menu after 3 seconds
        //StartCoroutine(gameOverScreen());

    }

    /*
    IEnumerator gameOverScreen()
    {
        //once voiceline is finished, return to main menu
        yield return new WaitForSeconds(3);

        print("changing scenes");
        SceneManager.LoadScene("TitleScreen");
    }
    */

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
                        //last touch position is more up of first touch position
                        if (lp.y > fp.y)
                        {

                            Debug.Log("Up Swipe");
                            SceneManager.LoadScene(Load.lastPlayedGame, LoadSceneMode.Single);
                        }
                        //movement is a down swipe
                        else
                        {
                            Debug.Log("Down Swipe");
                            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
                        }
                    }
                }
            }
        }
    }


}




