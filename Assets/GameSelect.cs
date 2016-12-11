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

    private AudioSource source;
    private AudioClip instructions;
    private AudioClip lost;
    private AudioClip magic;
    private AudioClip survival;

    private static float holdTime = 3.0f;
    private static float acumTime = 0;

    //To keep track of which game is currently selected
    bool rhythm;
    bool sword;
    bool memory;
    //secret back button
    bool back;

    // Use this for initialization
    void Start()
    {
          //define what % of the screen is needed to be touched for a swipe to register
        dragDistance = Screen.height * 8 / 100;

        source = GetComponent<AudioSource>();
        instructions = Resources.Load("Sounds/BetaVoicelines/Menu/SelectGame") as AudioClip;
        lost = Resources.Load("Sounds/BetaVoicelines/Menu/LostInTheDark") as AudioClip;
        magic = Resources.Load("Sounds/BetaVoicelines/Menu/RhythmMagic") as AudioClip;
        survival = Resources.Load("Sounds/BetaVoicelines/Menu/SurvivalGame") as AudioClip;

        

        //no game is selected initially
        rhythm = false;
        sword = false;
        memory = false;
        back = false;

        StartCoroutine(playSound());
    }

    // Update is called once per frame
    void Update()
    {


        //if on android, tap will go to arcade game screen
#if UNITY_ANDROID
        MobileInput();
        /*
        MobileInput.InputType input = MobileInput.getInput();
        if (input == MobileInput.InputType.right)
        {
            rhythm = false;
            memory = false;
            sword = true;
            back = false;
        }
        if (input == MobileInput.InputType.left)
        {
            rhythm = true;
            memory = true;
            sword = false;
            back = false;
        }
        if (input == MobileInput.InputType.up)
        {
            rhythm = true;
            memory = false;
            sword = false;
            back = false;
        }
        if (input == MobileInput.InputType.down)
        {
             SceneManager.LoadScene("HighScores");
        }
        if (input == MobileInput.InputType.tap)
        {
                if (rhythm == true) 
                {
                    SceneManager.LoadScene("RhythmMagic");
                }
                else if (sword == true)
                {
                    SceneManager.LoadScene("SwordCombat");
                }
                else if (memory == true)
                {
                    SceneManager.LoadScene("MemoryGame");
                }
                else if (back == true)
                {
                    SceneManager.LoadScene("TitleScreen");
                }
                
        }*/

#endif

        //if on desktop, press up left or right then enter
        if (Input.GetKeyDown("right"))
        {

            Debug.Log("Right Swipe");
            rhythm = false;
            memory = false;
            sword = true;
            source.Stop();
            source.PlayOneShot(survival);

        }
        if (Input.GetKeyDown("left"))
        {
            Debug.Log("Left Swipe");
            rhythm = false;
            memory = true;
            sword = false;
            source.Stop();
            source.PlayOneShot(lost);
        }
        if (Input.GetKeyDown("up"))
        {
            Debug.Log("Up Swipe");
            rhythm = true;
            memory = false;
            sword = false;
            source.Stop();
            source.PlayOneShot(magic);
        }
        if (Input.GetKeyDown("down"))
        {
            Debug.Log("Down Swipe");
            SceneManager.LoadScene("HighScores");
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

    public IEnumerator playSound()
    {
        source.PlayOneShot(instructions);
        yield return new WaitForSeconds(instructions.length + 5);
    }

    public IEnumerator playSword()
    {
        source.Stop();
        yield return new WaitForSeconds(0);
        source.PlayOneShot(survival);
    }

    public IEnumerator playLost()
    {
        source.Stop();
        yield return new WaitForSeconds(0);
        source.PlayOneShot(lost);
    }

    public IEnumerator playRhythm()
    {
        source.Stop();
        yield return new WaitForSeconds(0);
        source.PlayOneShot(magic);
    }

    void MobileInput()
    {
        // user is touching the screen with one finger
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            acumTime = 0;

            //record how much time the screen is held
            acumTime += Input.GetTouch(0).deltaTime;

            //if screen is held for the minimum length then register a hold input
            if (acumTime >= holdTime)
            {
                acumTime = 0;
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
                            rhythm = false;
                            memory = false;
                            sword = true;
                            back = false;
                            source.Stop();
                            source.PlayOneShot(survival);
                        }
                            else
                            {
                                Debug.Log("Left Swipe");
                            rhythm = false;
                            memory = true;
                            sword = false;
                            back = false;
                            source.Stop();
                            source.PlayOneShot(lost);
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
                            back = false;
                            source.Stop();
                            source.PlayOneShot(magic);

                        }
                        //movement is a down swipe
                        else
                        {
                            Debug.Log("Down Swipe");
                            SceneManager.LoadScene("HighScores");
                        }
                    }

                }
                //Is a tap, since distance was less than 15% of screen height
                //Confirm game selection
                else
                {
                   if (rhythm == true) 
                {
                    SceneManager.LoadScene("RhythmMagic");
                }
                else if (sword == true)
                {
                    SceneManager.LoadScene("SwordCombat");
                }
                else if (memory == true)
                {
                    SceneManager.LoadScene("MemoryGame");
                }
                else if (back == true)
                {
                    SceneManager.LoadScene("TitleScreen");
                }
                    
                }
            }
        }
    }
}
