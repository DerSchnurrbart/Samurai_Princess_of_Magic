﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour {
    /*
    //Mobile Touch Input
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    */
    static GameObject playerPrefab;
    static GameObject player;
    static SwordCombat script;

    private AudioSource source;
    private AudioClip mode;
    

	// Use this for initialization
	void Start () {
        //define what % of the screen is needed to be touched for a swipe to register
        //dragDistance = Screen.height * 15 / 100;
        source = GetComponent<AudioSource>();
        mode = Resources.Load("Sounds/BetaVoicelines/SurvivalGameModeSelect") as AudioClip;

        playerPrefab = Resources.Load("Prefabs/Player") as GameObject;
        player = Instantiate(playerPrefab);
        script = player.GetComponent<SwordCombat>();

        StartCoroutine(playSound());
	}
	
	// Update is called once per frame
	void Update () {

        //if on android, tap will go to arcade game screen
#if UNITY_ANDROID
        MobileInput.InputType input = MobileInput.getInput();
        if (input == MobileInput.InputType.left)
        {
            SceneManager.LoadScene("SCTutorial");
        }

        else if (input == MobileInput.InputType.up)
        {
            source.Stop();
            script.SetGameMode(SwordCombat.GameMode.normal);
            script.enabled = true;
            Destroy(this);
        }
        else if (input == MobileInput.InputType.right)
        {
            source.Stop();
            script.SetGameMode(SwordCombat.GameMode.hard);
            script.enabled = true;
            Destroy(this);
        }
#endif
        //if on desktop, use keyboard controls
        if (Input.GetKeyDown("escape"))
        {
            print("ESCAPE");
            SceneManager.LoadScene("TitleScreen");
            
        }

        if (Input.GetKeyDown("left")) //"Tutorial level", just for explaining gameplay
        {
            SceneManager.LoadScene("SCTutorial");
        } 
        else if (Input.GetKeyDown("up")) //Beginning mode
        {
            source.Stop();
            script.SetGameMode(SwordCombat.GameMode.normal);
            script.enabled = true;
            Destroy(this);
        }
        else if (Input.GetKeyDown("right")) //Intermediate
        {
            source.Stop();
            script.SetGameMode(SwordCombat.GameMode.hard);
            script.enabled = true;
            Destroy(this);
        }
	}

    public IEnumerator playSound()
    {
        yield return new WaitForSeconds(0);
        source.PlayOneShot(mode);
    }

    /*
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
                            script.SetGameMode(SwordCombat.GameMode.hard);
                            script.enabled = true;
                            Destroy(this);
                        }
                        else
                        {
                            Debug.Log("Left Swipe");
                            SceneManager.LoadScene("SCTutorial");
                            Destroy(this);
                        }
                    }
                    //movement was vertical
                    else
                    {
                        //last touch position is more up of first touch position
                        if (lp.y > fp.y)
                        {
                            Debug.Log("Up Swipe");
                            script.SetGameMode(SwordCombat.GameMode.normal);
                            script.enabled = true;
                            Destroy(this);
                        }
                        //movement is a down swipe
                        else
                        {
                            Debug.Log("Down Swipe");
                        }
                    }
                }
                //Is a tap, since distance was less than 15% of screen height
                //Confirm game selection
                else
                {
                    Debug.Log("Tap");
                }
            }
        }
    }
    */

}
