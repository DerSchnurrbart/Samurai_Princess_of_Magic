using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateHighScore : MonoBehaviour {
    //Mobile Touch Input
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    Text txt;


    public int highScoreMemory = 0;
    public int highScoreSword = 0;
    public int highScoreRhythm = 0;
    public static string highScoreMemoryKey = "HighScoreMemory";
    public static string highScoreSwordKey = "HighScoreSword";
    public static string highScoreRhythmKey = "HighScoreRhythm";

    // Use this for initialization
    void Start () {
        txt = gameObject.GetComponent<Text>();

        //Get value if highscore has been assigned to highScoreKey yet, else 0
        highScoreMemory = PlayerPrefs.GetInt(highScoreMemoryKey, 0);
        highScoreSword = PlayerPrefs.GetInt(highScoreSwordKey, 0);
        highScoreRhythm = PlayerPrefs.GetInt(highScoreRhythmKey, 0);
    }
	
	// Update is called once per frame
	void Update () {
        //if most recent score is greater than high score, update and display "High Score!" text
        if (Load.lastPlayedGame == 1 && ScoreDisplay.gameScore > highScoreMemory)
        {
            PlayerPrefs.SetInt(highScoreMemoryKey, ScoreDisplay.gameScore);
            txt.text = "High Score!";
            PlayerPrefs.Save();
        }
        else if (Load.lastPlayedGame == 2 && ScoreDisplay.gameScore > highScoreSword)
        {
            PlayerPrefs.SetInt(highScoreSwordKey, ScoreDisplay.gameScore);
            txt.text = "High Score!";
            PlayerPrefs.Save();
            Debug.Log("High Score for Sword Game is: " + PlayerPrefs.GetInt(highScoreSwordKey));
            Debug.Log("highScoreSword (previous highscore) is: " + highScoreSword);
            
        }
        else if (Load.lastPlayedGame == 3 && ScoreDisplay.gameScore > highScoreRhythm)
        {
            PlayerPrefs.SetInt(highScoreRhythmKey, ScoreDisplay.gameScore);
            txt.text = "High Score!";
            PlayerPrefs.Save();
        }
        //no high score; do not display high score notification
        else
        {
            Debug.Log("Last Played Game: " + Load.lastPlayedGame + " and Score: " + ScoreDisplay.gameScore);
            Debug.Log("No High Score; best score is: " + highScoreSword);
            txt.text = "";
        }
    }
}
