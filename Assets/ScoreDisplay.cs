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
            /*if (Load.lastPlayedGame == 1)
            {
                gameScore = MemoryAlt.score;
                gameScoreUnits = "rounds";
            }
            else if (Load.lastPlayedGame == 2)
            {
                gameScore = SwordCombat.score;
                gameScoreUnits = "seconds";
            }
            else
            {
                gameScore = Rhythm.score;
                gameScoreUnits = "rounds";
            }
            */
                
                //This is for testing the display
                //Comment this out when score tracking is implemented for each game
                gameScore = 10;
                gameScoreUnits = "rounds";

        //Example text: You lasted over 5 rounds!
        txt.text = "You lasted over " + gameScore + "+ " + gameScoreUnits + "!";
	}
}
