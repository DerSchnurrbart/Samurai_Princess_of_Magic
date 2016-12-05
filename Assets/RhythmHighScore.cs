using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RhythmHighScore : MonoBehaviour
{

    Text txt;

    // Use this for initialization
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        txt.text = PlayerPrefs.GetInt(HighScore.highScoreRhythmKey, 0) + " beats";
    }

    // Update is called once per frame
    void Update()
    {
    }
}
