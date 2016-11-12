using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayAgain : MonoBehaviour {

    public void returnToGame()
    {
        int sceneNumber = Load.lastPlayedGame;
        //currently doesn't work some reason
        //is it because I need to set OnClick's target to GameOver Screen (PlayAgain)
        //  and not to PlayAgain? Don't know how to select GameOver Screen (PlayAgain)
        SceneManager.LoadScene(sceneNumber);
    }
}
