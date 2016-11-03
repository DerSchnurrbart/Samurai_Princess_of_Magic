using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class OnHoverSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    //public AudioClip clicksound2;
    public AudioClip hoversound2;

    private Button button { get { return GetComponent<Button>(); } }
    private AudioSource source { get { return GetComponent<AudioSource>(); } }


    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        //source.clip = clicksound2;
        source.playOnAwake = false;

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

}
