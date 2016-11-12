using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordCombat : MonoBehaviour {

    //save the final score here, 
    //   which will be accessed and displayed on game over screen
    public static int score;


    /*********************************Enum Definitions************************************/

    const int numberOfEnemies = 3;
    const int numberOfWeapons = 3;
    const int turnsBeforeHittingPlayer = 3;
    enum EnemyType { insect, wolf, ghost };
    enum PlayerWeapons { hammer, sword, magic_staff }; 
    //Important note: items in EnemyType map to PlayerWeapons, i.e. hammer kills insect, sword kills wolf
    enum Directions { north, east, south, west };
    

    /*********************************Assets**********************************************/

    public static AudioClip[] enemyNoises;
    public static AudioClip[] enemyDeaths;
    public static AudioClip[] weaponMiss;
    public static AudioClip[] weaponEquipSound;
    public static GameObject enemyPrefab;

    /********************************Global State*****************************************/

    static PlayerWeapons currentWeapon;
    static Directions playerFacing;
    static bool weaponDelayActive = false;
    static bool isRotating = false;
    static float rotationDuration = 0.5f;
    static float inputDelay = 0.25f;
    static float spawnRate = 15.0f;
    static float approachRate = 3.0f;
    static List<enemySpawner> spawners;
    static AudioSource playerAudioSource;
    static GameObject player;
    static int PlayerHealth = 5;

    class enemySpawner
    {
        List<enemy> enemies;
        public Directions initialDirection;
        Vector3 StartingPosition;
        Vector3 AttackVector;
        Vector3 IncrementVector; 

        public enemySpawner(Vector3 pos, Directions directionFromPlayer)
        {
            enemies = new List<enemy>();
            StartingPosition = pos;
            initialDirection = directionFromPlayer;
            AttackVector = -1*pos.normalized;
        }

        public void spawnEnemy()
        {
            enemies.Add(new enemy(StartingPosition, AttackVector * approachRate));
        }
        
        public void playerSwing(PlayerWeapons weapon)
        {
            //Sloppy implementation, kills all enemies of a type in a direction
            //May want to change to only kill closest enemy
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
            if (toRemove.Count > 0) playerAudioSource.PlayOneShot(enemyDeaths[(int)weapon]);
            foreach(enemy x in toRemove)
            {
                enemies.Remove(x);
            }

        }

    };

    class enemy
    {
        GameObject source;
        AudioSource au_source; //Houses positional information, audio clip
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
        }

        public enemy(Vector3 pos, Vector3 approachSpeed) : this(pos, approachSpeed, 
                    (EnemyType)Random.Range(0, numberOfEnemies))
        {
        }

        public void kill()
        {
            DestroyImmediate(au_source);
            DestroyImmediate(source);
        }

    }



	void Start () {

        player = GameObject.Find("Player");
        playerAudioSource = player.GetComponent<AudioSource>();

        enemyNoises = new AudioClip[3];
        enemyNoises[(int)EnemyType.insect] = Resources.Load("Sounds/Enemies/scratching") as AudioClip;
        enemyNoises[(int)EnemyType.wolf] = Resources.Load("Sounds/Enemies/wolf") as AudioClip;
        enemyNoises[(int)EnemyType.ghost] = Resources.Load("Sounds/Enemies/ghost") as AudioClip;
        enemyDeaths = new AudioClip[3];
        enemyDeaths[(int)EnemyType.wolf] = Resources.Load("Sounds/Death/wolfdeath") as AudioClip;
        enemyDeaths[(int)EnemyType.insect] = Resources.Load("Sounds/Death/bugsmash") as AudioClip;
        enemyDeaths[(int)EnemyType.ghost] = Resources.Load("Sounds/Death/Wilhelm") as AudioClip;

        weaponEquipSound = new AudioClip[3];
        weaponEquipSound[(int)PlayerWeapons.sword] = Resources.Load("Sounds/PlayerWeapons/unsheath") as AudioClip;
        weaponEquipSound[(int)PlayerWeapons.hammer] = Resources.Load("Sounds/PlayerWeapons/hammer") as AudioClip;
        weaponEquipSound[(int)PlayerWeapons.magic_staff] = Resources.Load("Sounds/PlayerWeapons/Spell_01") as AudioClip;

        currentWeapon = PlayerWeapons.hammer;
        playerFacing = Directions.north;

        
        spawners = new List<enemySpawner>();
        //Spawning Locations are cardinal directions
        spawners.Add(new enemySpawner(new Vector3(0, 0, -50), Directions.north)); //in front of player
        spawners.Add(new enemySpawner(new Vector3(50, 0, 0), Directions.east)); //right of player at start 
        spawners.Add(new enemySpawner(new Vector3(0, 0, 50), Directions.south)); //behind player
        spawners.Add(new enemySpawner(new Vector3(-50, 0, 0), Directions.west)); //left of player 

        enemyPrefab = Resources.Load("Prefabs/enemy") as GameObject;

        StartCoroutine(spawnMaster());

	}

    // Update is called once per frame
    void Update() {

        if (!isRotating) {
            if (Input.GetKeyDown("right"))
            {
                playerFacing = (Directions)(((int)playerFacing + 1) % 4);
                StartCoroutine(rotatePlayer(player.transform.rotation, player.transform.rotation * Quaternion.Euler(0, -90, 0), rotationDuration));
            }
            else if (Input.GetKeyDown("left"))
            {
                StartCoroutine(rotatePlayer(player.transform.rotation, player.transform.rotation * Quaternion.Euler(0, 90, 0), rotationDuration));
                playerFacing = (Directions)(((int)playerFacing - 1) % 4);
            }
        }

        if (!weaponDelayActive)
        {
            if (Input.GetKeyDown("up"))
            {
                spawners[(int)playerFacing].playerSwing(currentWeapon);
                StartCoroutine(inputController());
            }
        }
    
        if (Input.GetKeyDown("down"))
        {
            currentWeapon = (PlayerWeapons)(((int)currentWeapon + 1) % numberOfWeapons);
            playerAudioSource.PlayOneShot(weaponEquipSound[(int)currentWeapon], 0.3f);
            weaponDelayActive = false;
        }

	
	}

    //credit to http://gamedev.stackexchange.com/questions/97074/how-to-stop-rotation-every-90-degrees
    //for the core idea of this implementation
    public IEnumerator rotatePlayer(Quaternion startingRotation, Quaternion endingRotation, float duration)
    {
        float endTime = Time.time + duration;
        isRotating = true;
        while(Time.time <= endTime)
        {
            float percentElapsed = 1 - ((endTime - Time.time) / duration);
            player.transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, percentElapsed);
            yield return 0; 
        }
        player.transform.rotation = endingRotation;
        isRotating = false;
    }


    //Exists to prevent players from spamming attacks, making the game trivial
    public IEnumerator inputController()
    {
        weaponDelayActive = true;
        yield return new WaitForSeconds(inputDelay);
        weaponDelayActive = false;
    }

    public IEnumerator spawnMaster()
    {
        while (true)
        {
            int randomNumber = Random.Range(0, 3);
            if (((int)playerFacing + 2) % 4 == (int)spawners[randomNumber].initialDirection) continue;
            spawners[randomNumber].spawnEnemy();
            yield return new WaitForSeconds(spawnRate);
        }
    }

	void OnTriggerEnter()
    {
        PlayerHealth--;
    }
}
