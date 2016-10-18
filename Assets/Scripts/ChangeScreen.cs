using UnityEngine;
using System.Collections;

public class ChangeScreen : MonoBehaviour {

    public void SwitchScreen()
    {
        Camera cam = Camera.main;
        Vector3 camPos = cam.transform.position;
        camPos.x = 703.5f;
        cam.transform.position = camPos;
    } 

}
