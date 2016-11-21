using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Button))]

public class OnHoverSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    //public AudioClip clicksound2;
    public AudioClip hoversound2;

    private Button button { get { return GetComponent<Button>(); } }
    private AudioSource source { get { return GetComponent<AudioSource>(); } }

    int secondsWait;


    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        //source.clip = clicksound2;
        source.playOnAwake = false;
        source.mute = true;
        

    }

    void Update()
    {
        StartCoroutine(delayHoverSound());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        source.clip = hoversound2;
        source.PlayOneShot(hoversound2);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        source.Stop();
    }

    IEnumerator delayHoverSound()
    {
        //secondsWait = (int)ClickSound.audioLength;
        yield return new WaitForSeconds(11);
        source.mute = false;
    }
}
