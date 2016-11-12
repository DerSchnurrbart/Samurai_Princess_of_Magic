using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Load : MonoBehaviour
{
    //this loadScene does the same as unity SceneManager's LoadScene,
    //but also saves the name of the scene passing it,
    //so the game can keep track of what scene it was on previously (so you can return)

        //game starts at title screen, so initialize to 0
    public static int lastPlayedGame = 0;
    
    public static void updateLastPlayedGame(int sceneNumber){
        lastPlayedGame = sceneNumber;
    }

    //can't be accessed without creating an object since it is non-static
    public void loadScene(int sceneNumber)
    {
        
        SceneManager.LoadScene(sceneNumber);
    }

}
