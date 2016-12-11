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
    void Update()
    {

        //if on android, tap will go to arcade game screen
#if UNITY_ANDROID
        MobileInput.InputType input = MobileInput.getInput();
        if (input == MobileInput.InputType.hold)
        {
            SceneManager.LoadScene("TitleScreen");
        }
        //else if (input == MobileInput.InputType.tap)
        //{
        //    SceneManager.LoadScene("MinigameScreen");
        //}
        else if (input == MobileInput.InputType.down)
        {
            SceneManager.LoadScene("Credits");
        }
#endif

        //if on desktop, enter will go to arcade game screen
        if (Input.GetKeyDown("return"))
        {
            SceneManager.LoadScene("MinigameScreen");
        }
        else if (Input.GetKeyDown("down"))
        {
            SceneManager.LoadScene("Credits");
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
