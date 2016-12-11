using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Credits : MonoBehaviour {

    private AudioSource creditsVoice;
    private AudioClip swipe;

    // Use this for initialization
    void Start () {
        creditsVoice = GetComponent<AudioSource>();

        swipe = Resources.Load("Sounds/BetaVoicelines/CreditsSwipeDown") as AudioClip;

        creditsVoice.PlayOneShot(swipe);
    }

    // Update is called once per frame
    void Update()
    {
        //If running on Unity Android, run this block to use mobile input controls
#if UNITY_ANDROID
        MobileInput.InputType input = MobileInput.getInput();
        
        if (input == MobileInput.InputType.up)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (input == MobileInput.InputType.right)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (input == MobileInput.InputType.left)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (input == MobileInput.InputType.tap)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (input == MobileInput.InputType.down)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
#endif
        //Run desktop keyboard/mouse controls
        if (Input.GetKeyDown("down"))
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown("up"))
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown("right"))
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown("down"))
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }

    }
}
