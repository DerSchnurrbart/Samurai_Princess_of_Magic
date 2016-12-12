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
        Debug.Log("Last played game is: " + Load.lastPlayedGame + " and its Game Over Score: " + ScoreDisplay.gameScore);
        Debug.Log("Grabbing Rhythm High Score: " + PlayerPrefs.GetInt(UpdateHighScore.highScoreRhythmKey, 0));
        txt.text = PlayerPrefs.GetInt(UpdateHighScore.highScoreRhythmKey, 0) + " beats";
    }

    // Update is called once per frame
    void Update()
    {
    }
}
