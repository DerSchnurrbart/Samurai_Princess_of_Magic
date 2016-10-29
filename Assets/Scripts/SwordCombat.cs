using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordCombat : MonoBehaviour {

    /*********************************Enum Definitions************************************/

    const int numberOfEnemies = 3;
    const int numberOfWeapons = 3;
    const int turnsBeforeHittingPlayer = 3;
    enum EnemyType { insect, wolf, ghost };
    enum StartingPositions { left, right, forward };
    enum PlayerWeapons { hammer, sword, magic_staff };

    /*********************************Assets**********************************************/

    public static AudioClip[] enemyNoises;
    public static AudioClip[] weaponSoundEffects;

    /********************************Global State*****************************************/

    PlayerWeapons currentWeapon;
    float spawnRate = 30.0f;
    float approachRate = 9.0f;
    List<enemySpawner> spawners;
    AudioSource playerBasic;

    class enemySpawner
    {
        List<enemy> enemies;
        Vector3 StartingPosition;
        Vector3 IncrementVector; 

        public enemySpawner(Vector3 pos)
        {
            enemies = new List<enemy>();
            StartingPosition = pos;
            IncrementVector = (Vector3.zero - pos) / (float)turnsBeforeHittingPlayer;
        }

        public bool incrementEnemies()
        {
            foreach(enemy x in enemies) {
                if (x.approachPlayer(IncrementVector))
                    return true;
            }
            return false;
        }

        public void spawnEnemy()
        {
            enemies.Add(new enemy(StartingPosition));
        }

    };

    class enemy
    {
        GameObject source; 
        AudioSource au_source; //Houses positional information, audio clip
        int turnsActive;
        public EnemyType type;

        public enemy(EnemyType type_in, Vector3 pos)
        {
            au_source = new GameObject().AddComponent<AudioSource>(); 
            au_source.transform.position = pos;
            au_source.spatialBlend = 1.0f;
            au_source.clip = enemyNoises[(int)type_in];
            type = type_in;
            turnsActive = 0;
        }

        public enemy(Vector3 pos)
        {
            //If type not specified make random enemy

            au_source = new GameObject().AddComponent<AudioSource>();
            au_source.transform.position = pos;
            au_source.spatialBlend = 1.0f;
            type = (EnemyType)Random.Range(0, numberOfEnemies);
            au_source.clip = enemyNoises[(int)type];
            turnsActive = 0;
        }

        public bool approachPlayer(Vector3 IncrementVector)
        {
            au_source.transform.position += IncrementVector;
            au_source.Play();
            if (turnsActive++ == turnsBeforeHittingPlayer) return true;
            else return false;
        }
    }



	void Start () {
        playerBasic = GameObject.Find("Player").GetComponent<AudioSource>();
        enemyNoises = new AudioClip[3];
        enemyNoises[(int)EnemyType.insect] = Resources.Load("Sounds/Enemies/scratching") as AudioClip;
        enemyNoises[(int)EnemyType.wolf] = Resources.Load("Sounds/Enemies/wolfhowl") as AudioClip;
        enemyNoises[(int)EnemyType.ghost] = Resources.Load("Sounds/Enemies/ghost") as AudioClip;
        weaponSoundEffects = new AudioClip[3];
        weaponSoundEffects[(int)PlayerWeapons.sword] = Resources.Load("Sounds/PlayerWeapons/unsheath") as AudioClip;
        weaponSoundEffects[(int)PlayerWeapons.hammer] = Resources.Load("Sounds/PlayerWeapons/hammer") as AudioClip;
        weaponSoundEffects[(int)PlayerWeapons.magic_staff] = Resources.Load("Sounds/PlayerWeapons/Spell_01") as AudioClip;
        currentWeapon = PlayerWeapons.hammer;
        spawners = new List<enemySpawner>();
        spawners.Add(new enemySpawner(new Vector3(50, 0, 0)));
        spawners.Add(new enemySpawner(new Vector3(-50, 0, 0)));
        spawners.Add(new enemySpawner(new Vector3(0, 0, 50)));

        StartCoroutine(spawnMaster());
        StartCoroutine(enemyLeader());

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("up"))
        {

        }
        else if (Input.GetKeyDown("left"))
        {

        }
        else if (Input.GetKeyDown("right"))
        {

        } 
        else if (Input.GetKeyDown("down"))
        {
            currentWeapon = (PlayerWeapons) (((int)currentWeapon + 1) % numberOfWeapons);
            //print(currentWeapon);
            playerBasic.PlayOneShot(weaponSoundEffects[(int)currentWeapon], 0.5f);
        }

	
	}



    public IEnumerator enemyLeader()
    {
        while (true)
        {
            yield return new WaitForSeconds(approachRate);
            for (int i = 0; i < spawners.Count; i++)
                if (spawners[i].incrementEnemies())
                {
                    //player dead
                }
        }
    }

    public IEnumerator spawnMaster()
    {
        while (true)
        {
            spawners[Random.Range(0, 3)].spawnEnemy();
            yield return new WaitForSeconds(spawnRate);
        }
    }

}
