using UnityEngine;
using System.Collections;

public class Play_Voiceline : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip audioClip;

    public void playClip()
    {
        //not working yet
        audioClip = Resources.Load("Sounds/Enemies/scratching") as AudioClip;
        //audioSource.PlayOneShot(audioClip);
        audioSource.clip = audioClip;
       
    }

    // Use this for initialization
    /*void Start()
    {
        myclip = Resources.Load("Sounds/Voicelines/Main Menu Instructions/SPoM - Main Menu.") as AudioClip;
        //audioSource = this.GetComponent<AudioSource>();
        //audioSource.clip = myclip;
        audioSource.GetComponent<AudioSource>().clip = myclip;
        audioSource.Play();
    }
    */
}
