using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordCombat : MonoBehaviour {

    /*********************************Enum Definitions************************************/

    const int numberOfEnemies = 3;
    const int numberOfWeapons = 3;
    const int turnsBeforeHittingPlayer = 3;
    enum EnemyType { insect, wolf, ghost };
    enum PlayerWeapons { hammer, sword, magic_staff }; 
    //Important note: items in EnemyType map to PlayerWeapons, i.e. hammer kills insect, sword kills wolf
    enum StartingPositions { left, right, forward };

    /*********************************Assets**********************************************/

    public static AudioClip[] enemyNoises;
    public static AudioClip[] enemyDeaths;
    public static AudioClip[] weaponSoundEffects;
    public static GameObject enemyPrefab;

    /********************************Global State*****************************************/

    PlayerWeapons currentWeapon;
    static float spawnRate = 25.0f;
    static float approachRate = 2.0f;
    List<enemySpawner> spawners;
    AudioSource playerBasic;

    class enemySpawner
    {
        List<enemy> enemies;
        int killedCount = 0;
        Vector3 StartingPosition;
        Vector3 AttackVector;
        Vector3 IncrementVector; 

        public enemySpawner(Vector3 pos)
        {
            enemies = new List<enemy>();
            StartingPosition = pos;
            AttackVector = -1*pos.normalized;
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
            enemies.Add(new enemy(StartingPosition, AttackVector * approachRate));
        }
        
        public void playerSwing(PlayerWeapons weapon)
        {

            //Kills closest enemy if player used the right type, in one direction
            //Closest enemy will be at front of the List at index 0
            //Killed enemy is removed from list and list auto-updates its indexes and such
            //Keeps track of # of killed enemies
            if (enemies[0].weakness == weapon)
            {
                int enemyDeathSoundType = (int)enemies[0].type;

                //change default audio clip of enemy being killed to the death sound, then play death sound
                Debug.Log("Enemy type: " + enemyDeathSoundType + " killed: Play SFX");
                enemies[0].au_source.clip = enemyDeaths[enemyDeathSoundType];
                enemies[0].au_source.Play();

                killedCount++;
                enemies[0].kill();
                enemies.RemoveAt(0);
            }




            /*
            Old Code: "Sloppy implementation, kills all enemies of a type in a direction"
            List<enemy> toRemove = new List<enemy>();

            foreach(enemy x in enemies)
            {
                if (x.weakness == weapon)
                {
                    x.kill();
                    toRemove.Add(x);
                    //Todo Add death sound
                }
            }
            foreach(enemy x in toRemove)
            {
                enemies.Remove(x);
            }
*/
        }

    };

    class enemy
    {
        GameObject source;
        public AudioSource au_source; //Houses positional information, audio clip
        int turnsActive;
        public EnemyType type;
        public PlayerWeapons weakness;

        public enemy(Vector3 pos, Vector3 approachSpeed, EnemyType type_in)
        {
            source = Instantiate(enemyPrefab);
            source.transform.position = pos;
            au_source = source.GetComponent<AudioSource>();
            au_source.clip = enemyNoises[(int)type_in];
            au_source.Play();

                

            source.GetComponent<Rigidbody>().velocity = approachSpeed;
            type = type_in;
            weakness = (PlayerWeapons) type_in;
            turnsActive = 0;
        }

        public enemy(Vector3 pos, Vector3 approachSpeed) : this(pos, approachSpeed, 
                    (EnemyType)Random.Range(0, numberOfEnemies))
        {
        }

        public bool approachPlayer(Vector3 IncrementVector)
        {
            au_source.transform.position += IncrementVector;
            au_source.Play();
            if (turnsActive++ == turnsBeforeHittingPlayer) return true;
            else return false;
        }

        public void kill()
        {
            DestroyImmediate(au_source);
            DestroyImmediate(source);
        }

    }



	void Start () {
        playerBasic = GameObject.Find("Player").GetComponent<AudioSource>();

        enemyNoises = new AudioClip[3];
        enemyNoises[(int)EnemyType.insect] = Resources.Load("Sounds/Enemies/scratching") as AudioClip;
        enemyNoises[(int)EnemyType.wolf] = Resources.Load("Sounds/Enemies/wolfhowl") as AudioClip;
            enemyNoises[(int)EnemyType.ghost] = Resources.Load("Sounds/Enemies/ghost") as AudioClip;

            enemyDeaths = new AudioClip[3];
            enemyDeaths[(int)EnemyType.insect] = Resources.Load("Sounds/Enemies/insectdeath") as AudioClip;
            enemyDeaths[(int)EnemyType.wolf] = Resources.Load("Sounds/Enemies/wolfdeath") as AudioClip;
            enemyDeaths[(int)EnemyType.ghost] = Resources.Load("Sounds/Enemies/ghostdeath") as AudioClip;

            weaponSoundEffects = new AudioClip[3];
        weaponSoundEffects[(int)PlayerWeapons.sword] = Resources.Load("Sounds/PlayerWeapons/unsheath") as AudioClip;
        weaponSoundEffects[(int)PlayerWeapons.hammer] = Resources.Load("Sounds/PlayerWeapons/hammer") as AudioClip;
        weaponSoundEffects[(int)PlayerWeapons.magic_staff] = Resources.Load("Sounds/PlayerWeapons/Spell_01") as AudioClip;

        currentWeapon = PlayerWeapons.hammer;
        spawners = new List<enemySpawner>();
        spawners.Add(new enemySpawner(new Vector3(50, 0, 0))); //Right of player
        spawners.Add(new enemySpawner(new Vector3(-50, 0, 0))); //Left of player
        spawners.Add(new enemySpawner(new Vector3(0, 0, 50))); //Directly in front of player

        enemyPrefab = Resources.Load("Prefabs/enemy") as GameObject;

        StartCoroutine(spawnMaster());
 //       StartCoroutine(enemyLeader());

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("right")) spawners[0].playerSwing(currentWeapon);
        else if (Input.GetKeyDown("left")) spawners[1].playerSwing(currentWeapon);
        else if (Input.GetKeyDown("up")) spawners[2].playerSwing(currentWeapon);
        else if (Input.GetKeyDown("down"))
        {
            currentWeapon = (PlayerWeapons)(((int)currentWeapon + 1) % numberOfWeapons);
            playerBasic.PlayOneShot(weaponSoundEffects[(int)currentWeapon], 0.5f);
        }

	
	}



/*    public IEnumerator enemyLeader()
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
*/
    public IEnumerator spawnMaster()
    {
        while (true)
        {
            spawners[Random.Range(0, 3)].spawnEnemy();
            yield return new WaitForSeconds(spawnRate);
        }
    }

}
