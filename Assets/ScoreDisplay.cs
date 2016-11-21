using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour {

    Text txt;

    private int gameScore = 0;
    private string gameScoreUnits;

	// Use this for initialization
	void Start () {
        txt = gameObject.GetComponent<Text>();

        //Uncommment the following block once score tracking is implemented in each game
            //Grabs the final game score, 
            //   stored in a static int variable in each game's script.
            if (Load.lastPlayedGame == 1)
            {
                gameScore = MemoryAlt.score;
                gameScoreUnits = "directions";
            }
            else if (Load.lastPlayedGame == 2)
            {
                gameScore = SwordCombat.mostRecentScore;
                gameScoreUnits = "monsters";
            }
            else
            {
                gameScore = Rhythm.score;
                gameScoreUnits = "rounds";
            }
            
                
                //This is for testing the display only
                //Comment this out when score tracking is implemented for each game
                        //gameScore = 10;
                        //gameScoreUnits = "rounds";

        //Example text: You lasted 5 rounds!
        txt.text = "You lasted at least " + gameScore + " " + gameScoreUnits + "!";

        //If game played was survival game,
        //Example text: You killed 5 monsters!
        if (Load.lastPlayedGame == 2)
        {
            txt.text = "You killed at least " + gameScore + " " + gameScoreUnits + "!";
        }
        //If memory game
        if (Load.lastPlayedGame == 1)
        {
            txt.text = "You followed at least " + gameScore + " " + gameScoreUnits + "!";
        }
    }
}
