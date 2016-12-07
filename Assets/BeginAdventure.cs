using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BeginAdventure : MonoBehaviour {

    private AudioSource source;
    private AudioClip welcome;
    //helper for triggering game select directions after tapping on title screen

    // Use this for initialization
    void Start () {

        source = GetComponent<AudioSource>();
        welcome = Resources.Load("Sounds/BetaVoicelines/Menu/Welcome") as AudioClip;

        StartCoroutine(playSound());
    }
	
	// Update is called once per frame
	void Update () {
        

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
        while (true)
        {
            source.PlayOneShot(welcome);
            yield return new WaitForSeconds(10);
        }
    }

}
