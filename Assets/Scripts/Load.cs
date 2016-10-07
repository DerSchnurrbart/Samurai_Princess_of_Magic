using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Load : MonoBehaviour {

    public void loadScene(int SceneNumber)
    {
        SceneManager.LoadScene(SceneNumber);
    }

}
