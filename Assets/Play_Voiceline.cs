using UnityEngine;
using System.Collections;

public class Play_Voiceline : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip audioClip;

    public void playClip()
    {
        //not working yet
        audioSource.clip = Resources.Load("Sounds/Voicelines/Main Menu Instructions/SPoM - Main Menu.wav") as AudioClip; ;
        audioSource.Play();
    }
}
