using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    private AudioSource gameOverVoice;
    AudioClip gameOverClip;
    bool gameOverVoiceIsPlaying = false;
    bool gameOverLoaded = false;

    // Use this for initialization
    void Start()
    {
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

        StartCoroutine(gameOverScreen());


    }

    IEnumerator gameOverScreen()
    {
        //once voiceline is finished, return to main menu
        yield return new WaitForSeconds(3);

        print("changing scenes");
        SceneManager.LoadScene("TitleScreen");
    }
}
