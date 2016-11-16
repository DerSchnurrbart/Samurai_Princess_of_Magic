using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour {

    static GameObject playerPrefab;
    static GameObject player;
    static SwordCombat script;

	// Use this for initialization
	void Start () {
        
        playerPrefab = Resources.Load("Prefabs/Player") as GameObject;
        player = Instantiate(playerPrefab);
        script = player.GetComponent<SwordCombat>();
        
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("escape"))
        {
            print("ESCAPE");
            SceneManager.LoadScene("TitleScreen");
        }

        if (Input.GetKeyDown("1")) //"Tutorial level", just for explaining gameplay
        {
            script.SetGameMode(SwordCombat.GameMode.tutorial);
            script.enabled = true;
            Destroy(this);
        } 
        else if (Input.GetKeyDown("2")) //Beginning mode
        {
            script.SetGameMode(SwordCombat.GameMode.normal);
            script.enabled = true;
            Destroy(this);
        }
        else if (Input.GetKeyDown("3")) //Intermediate
        {
            script.SetGameMode(SwordCombat.GameMode.hard);
            script.enabled = true;
            Destroy(this);
        }
	}
}
