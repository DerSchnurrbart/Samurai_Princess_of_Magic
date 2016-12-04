using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class SwordCombat : MonoBehaviour {

    //save the final score here, 
    //   which will be accessed and displayed on game over screen
    public static int mostRecentScore;

   
    
    //*********************************GameOver Variables************************************/
    AudioClip swordDefeat;
    bool audioIsPlaying = false;
    public bool gameIsOver = false;
    public bool gameOverScreenLoaded = false;


    /*********************************Enum Definitions************************************/

    const float rotationDuration = 0.25f;
    const float inputDelay = 0.25f;
    const int numberOfEnemies = 3;
    const int numberOfWeapons = 3;
    enum EnemyType { insect, wolf, ghost };
    enum PlayerWeapons { hammer, sword, magic_staff }; 
    //Important note: items in EnemyType map to PlayerWeapons, i.e. hammer kills insect, sword kills wolf
    enum Directions { north, east, south, west };
    public enum GameMode { tutorial, normal, hard, random };
    

    /*********************************Assets**********************************************/

    static AudioClip[] enemyNoises;
    static AudioClip[] enemyDeaths;
    static AudioClip weaponMiss;
    static AudioClip playerHit;
    static AudioClip heartbeat;
    static AudioClip player_death;
    static AudioClip[] weaponEquipSound;
    private AudioClip beginning;
    static GameObject enemyPrefab;
    public GameObject screen_tear;

    /********************************Global State*****************************************/

    int player_health = 3;
    int enemies_per_spawn = 1;
    public int active_enemies = 0;
    int score = 0;
    PlayerWeapons currentWeapon;
    Directions playerFacing;
    bool weaponDelayActive = false;
    bool isRotating = false;
    float approachRate;
    float attack_delay;
    List<enemySpawner> spawners;
    static AudioSource playerAudioSource;
    static GameMode mode;
    MobileInput mobInput;
    
    /*******************************Public Functions********************************/

    public void SetGameMode(GameMode m)
    {
        mode = m;
        if (m == GameMode.normal)
        {
            approachRate = 2.0f;
            attack_delay = 2.0f;
        }
        else if (m == GameMode.hard)
        {
            approachRate = 8.0f;
            attack_delay = 1.0f;
        }

    }


    /*************************************Class Definitions********************************************/

    class enemySpawner
    {
        List<enemy> enemies;
        public Directions initialDirection;
        Vector3 StartingPosition;
        Vector3 AttackVector;
        SwordCombat TopLevel;

        public enemySpawner(Vector3 pos, Directions directionFromPlayer, SwordCombat parent)
        {
            enemies = new List<enemy>();
            TopLevel = parent;
            StartingPosition = pos;
            initialDirection = directionFromPlayer;
            AttackVector = -1*pos.normalized;
        }

        public void spawnEnemy(float ApproachRate)
        {
            if (TopLevel.player_health > 0)
            {
                enemies.Add(new enemy(StartingPosition, AttackVector * ApproachRate, TopLevel));
            }
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
            else playerAudioSource.PlayOneShot(weaponMiss);
            foreach(enemy x in toRemove)
            {
                enemies.Remove(x);
                TopLevel.score++;
                if (TopLevel.score % 25 == 0) TopLevel.enemies_per_spawn++;
                HandleScaling(mode);
            }

        }

        public void KillAll()
        {
            foreach(enemy x in enemies)
            {
                x.kill();
            }
            enemies.Clear();
        }

        void HandleScaling(GameMode difficulty)
        {
            if (difficulty == GameMode.hard) TopLevel.approachRate = (float) Math.Pow(Math.Log(TopLevel.score, 2), 2) + 7;
            else TopLevel.approachRate = (float) Math.Pow(Math.Log(TopLevel.score, 2), 2) + 3;
        }

    };

    class enemy
    {
        GameObject source;
        AudioSource au_source; //Houses positional information, audio clip
        public EnemyType type;
        public PlayerWeapons weakness;
        SwordCombat parent;

        public enemy(Vector3 pos, Vector3 approachSpeed, EnemyType type_in, SwordCombat parent)
        {
            source = Instantiate(enemyPrefab);
            source.transform.position = pos;
            au_source = source.GetComponent<AudioSource>();
            au_source.clip = enemyNoises[(int)type_in];
            au_source.Play();
            source.GetComponent<Rigidbody>().velocity = approachSpeed;
            source.GetComponent<EnemyLogic>().currentInstance = parent;
            source.GetComponent<EnemyLogic>().attack_delay = parent.attack_delay;
            this.parent = parent;
            parent.active_enemies++;
            type = type_in;
            weakness = (PlayerWeapons) type_in;
        }

        public enemy(Vector3 pos, Vector3 approachSpeed, SwordCombat parent) : this(pos, approachSpeed, 
                    (EnemyType)UnityEngine.Random.Range(0, numberOfEnemies), parent)
        {
        }

        public void kill()
        {
            parent.active_enemies--;
            DestroyImmediate(au_source);
            DestroyImmediate(source);
        }

    }


    /******************************************Helper Functions*****************************************/


    void HandlePlayerInputMobile()
    {
        MobileInput.InputType input = MobileInput.getInput();
        if (!isRotating)
        {
            if (input == MobileInput.InputType.right)
            {
                playerFacing = (Directions)(((int)playerFacing + 1) % 4);
                StartCoroutine(rotatePlayer(transform.rotation, transform.rotation * Quaternion.Euler(0, -90, 0), rotationDuration));
            }
            else if (input == MobileInput.InputType.left)
            {
                StartCoroutine(rotatePlayer(transform.rotation, transform.rotation * Quaternion.Euler(0, 90, 0), rotationDuration));
                playerFacing = (Directions)(((int)playerFacing + 3) % 4); //mod on negative doesn't work, this is equivalent to subtracting 1
            }
        }

        if (!weaponDelayActive)
        {
            if (input == MobileInput.InputType.up)
            {
                spawners[(int)playerFacing].playerSwing(currentWeapon);
                StartCoroutine(inputController());
            }
        }

        if (input == MobileInput.InputType.down)
        {
            currentWeapon = (PlayerWeapons)(((int)currentWeapon + 1) % numberOfWeapons);
            playerAudioSource.PlayOneShot(weaponEquipSound[(int)currentWeapon], 0.3f);
            weaponDelayActive = false;
        }
    }

    void HandlePlayerInput()
    {
        if (!isRotating)
        {
            if (Input.GetKeyDown("right"))
            {
                playerFacing = (Directions)(((int)playerFacing + 1) % 4);
                StartCoroutine(rotatePlayer(transform.rotation, transform.rotation * Quaternion.Euler(0, -90, 0), rotationDuration));
            }
            else if (Input.GetKeyDown("left"))
            {
                StartCoroutine(rotatePlayer(transform.rotation, transform.rotation * Quaternion.Euler(0, 90, 0), rotationDuration));
                playerFacing = (Directions)(((int)playerFacing + 3) % 4); //mod on negative doesn't work, this is equivalent to subtracting 1
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
            transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, percentElapsed);
            yield return 0; 
        }
        transform.rotation = endingRotation;
        isRotating = false;
    }


    //Exists to prevent players from spamming attacks, making the game trivial
    public IEnumerator inputController()
    {
        weaponDelayActive = true;
        yield return new WaitForSeconds(inputDelay);
        weaponDelayActive = false;
    }

    public void spawn_enemy()
    {
        int randomNumber = UnityEngine.Random.Range(0, 3);
        while (((int)playerFacing + 2) % 4 == (int)spawners[randomNumber].initialDirection)
        {
            randomNumber = UnityEngine.Random.Range(0, 3);
        }
        spawners[randomNumber].spawnEnemy(approachRate);
    }

    public IEnumerator attack_screen()
    {
        screen_tear.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        screen_tear.SetActive(false);
    }

    /*********************************Begin Unity Automatic Calls***************************************/

	void Start () {

        mobInput = new MobileInput();
        playerAudioSource = GetComponent<AudioSource>();
        //defeat sounds
        swordDefeat = Resources.Load("Sounds/Voicelines/GameOvers/SwordDefeat") as AudioClip;

        enemyNoises = new AudioClip[3];
        enemyNoises[(int)EnemyType.insect] = Resources.Load("Sounds/Enemies/scratching") as AudioClip;
        enemyNoises[(int)EnemyType.wolf] = Resources.Load("Sounds/Enemies/wolfhowl") as AudioClip;
        enemyNoises[(int)EnemyType.ghost] = Resources.Load("Sounds/Enemies/ghost") as AudioClip;
        enemyDeaths = new AudioClip[3];
        enemyDeaths[(int)EnemyType.wolf] = Resources.Load("Sounds/Death/wolfdeath") as AudioClip;
        enemyDeaths[(int)EnemyType.insect] = Resources.Load("Sounds/Death/bugsmash") as AudioClip;
        enemyDeaths[(int)EnemyType.ghost] = Resources.Load("Sounds/Death/Wilhelm") as AudioClip;
        player_death = Resources.Load("Sounds/Death/player_death") as AudioClip;

        weaponEquipSound = new AudioClip[3];
        weaponEquipSound[(int)PlayerWeapons.sword] = Resources.Load("Sounds/PlayerWeapons/unsheath") as AudioClip;
        weaponEquipSound[(int)PlayerWeapons.hammer] = Resources.Load("Sounds/PlayerWeapons/hammer") as AudioClip;
        weaponEquipSound[(int)PlayerWeapons.magic_staff] = Resources.Load("Sounds/PlayerWeapons/Spell_01") as AudioClip;

        heartbeat = Resources.Load("Sounds/Death/heartbeat") as AudioClip;

        weaponMiss = Resources.Load("Sounds/PlayerWeapons/SwingMiss") as AudioClip;
        playerHit = Resources.Load("Sounds/Death/PlayerHit") as AudioClip;

        beginning = Resources.Load("Sounds/BetaVoicelines/BeginningGame") as AudioClip;

        currentWeapon = PlayerWeapons.hammer;
        playerFacing = Directions.north;

        
        spawners = new List<enemySpawner>();
        //Spawning Locations are cardinal directions
        spawners.Add(new enemySpawner(new Vector3(0, 0, -50), Directions.north, this)); //in front of player
        spawners.Add(new enemySpawner(new Vector3(50, 0, 0), Directions.east, this)); //right of player at start 
        spawners.Add(new enemySpawner(new Vector3(0, 0, 50), Directions.south, this)); //behind player
        spawners.Add(new enemySpawner(new Vector3(-50, 0, 0), Directions.west, this)); //left of player 

        enemyPrefab = Resources.Load("Prefabs/enemy") as GameObject;

        playerAudioSource.PlayOneShot(beginning);

        spawn_enemy();
	}
    
    // Update is called once per frame
    void Update() {

        //If running on Unity Android, run this block to use mobile input controls
        #if UNITY_ANDROID
            //TODO: Implement a way to escape the game
            //   Maybe grab the data for how long a tap is held, 
            //   and quit if touch is held for 3 seconds
            if (player_health < 1)
            {
                //goes to game over / score screen, and player can Play Again
                StartCoroutine(endGame());
            }
            else
            {
                HandlePlayerInputMobile();
            }
        
        #endif

        //Run desktop keyboard/mouse controls

        if (Input.GetKeyDown("escape"))
        {
            print("ESCAPE");
            SceneManager.LoadScene("TitleScreen");
        }

        if (player_health < 1)
        {
            for (int i = 0; i < 4; i++) spawners[i].KillAll();
            player_health = 3;
            mostRecentScore = score;
            score = 0;

            //commented out above because the following code
            //   goes to game over / score screen, and player can Play Again
            StartCoroutine(endGame());
        }
        else
        {
            HandlePlayerInput();
            if (active_enemies == 0)
            {
                for (int i = 0; i < enemies_per_spawn; i++) spawn_enemy();
            }
        }
	}

    

	public void enemy_attacked()
    {

        StartCoroutine(attack_screen());
        player_health--;
        if (player_health == 2)
        {
            playerAudioSource.PlayOneShot(playerHit);
        }
        else if (player_health == 1)
        {
            playerAudioSource.PlayOneShot(playerHit);
            if (player_health == 1)
            {
                playerAudioSource.PlayOneShot(heartbeat);
            }
        }
        else
        {
            playerAudioSource.PlayOneShot(swordDefeat);

        }

        print(player_health);
    }

    IEnumerator endGame()
    {
        playerAudioSource.PlayOneShot(player_death);
        yield return new WaitForSeconds(player_death.length);
        playerAudioSource.mute = true;

        //show gameover screen
        if (gameOverScreenLoaded == false)
        {
            gameOverScreenLoaded = true;

            //update before leaving scene
            Load.updateLastPlayedGame(2);
            SceneManager.LoadScene("GameOver");
        }
    }


}
