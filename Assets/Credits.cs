using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Credits : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        //If running on Unity Android, run this block to use mobile input controls
#if UNITY_ANDROID
        if (MobileInput.getInput() == MobileInput.InputType.down)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (MobileInput.getInput() == MobileInput.InputType.up)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (MobileInput.getInput() == MobileInput.InputType.right)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (MobileInput.getInput() == MobileInput.InputType.left)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (MobileInput.getInput() == MobileInput.InputType.tap)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
#endif
        //Run desktop keyboard/mouse controls
        if (Input.GetKeyDown("down"))
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown("up"))
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown("right"))
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown("down"))
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }

    }
}
