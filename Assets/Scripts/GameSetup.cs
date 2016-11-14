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
            script.SetDifficulty(200.0f, 0.0f);
            script.SetGameMode(SwordCombat.GameMode.tutorial);
            script.enabled = true;
            Destroy(this);
        } 
        else if (Input.GetKeyDown("2")) //Beginning mode
        {
            script.SetDifficulty(12.0f, 5.0f);
            script.SetGameMode(SwordCombat.GameMode.levels);
            script.enabled = true;
            Destroy(this);
        }
        else if (Input.GetKeyDown("3")) //Intermediate
        {
            script.SetDifficulty(9.0f, 8.0f);
            script.SetGameMode(SwordCombat.GameMode.levels);
            script.enabled = true;
            Destroy(this);
        }
        else if (Input.GetKeyDown("4"))
        {
            script.SetDifficulty(5.0f, 11.0f);
            script.SetGameMode(SwordCombat.GameMode.levels);
            script.enabled = true;
            Destroy(this);
        }
        else if (Input.GetKeyDown("5")) //Unfair Mode, spawns/enemies get quicker every 5 enemies killed
        {
            script.SetDifficulty(5.0f, 11.0f);
            script.SetGameMode(SwordCombat.GameMode.scaling);
            script.enabled = true;
            Destroy(this);
        }
        else if (Input.GetKeyDown("6"))
        {
            //Variable speed and spawn rate ???
        }
	}
}
