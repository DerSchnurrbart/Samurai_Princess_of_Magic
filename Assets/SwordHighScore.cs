using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SwordHighScore : MonoBehaviour
{

    Text txt;

    // Use this for initialization
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        txt.text = PlayerPrefs.GetInt(UpdateHighScore.highScoreSwordKey, 0) + " monsters";
    }

    // Update is called once per frame
    void Update()
    {
    }
}
