using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayAgain : MonoBehaviour {

    public void returnToGame()
    {
        int sceneNumber = Load.lastPlayedGame;

        SceneManager.LoadScene(sceneNumber);
    }
}
