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
    static AudioClip[] weaponEquipSound;
    AudioClip twoLivesLeft;
    AudioClip oneLifeLeft;
    static GameObject enemyPrefab;

    /********************************Global State*****************************************/

    int PlayerHealth = 3;
    int score = 0;
    PlayerWeapons currentWeapon;
    Directions playerFacing;
    bool weaponDelayActive = false;
    bool isRotating = false;
    bool spawning = false;
    float spawnRate;
    float approachRate;
    bool scaling;
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
            spawnRate = 50 / approachRate;
            scaling = true;
        }
        else if (m == GameMode.hard)
        {
            approachRate = 8.0f;
            spawnRate = 50 / approachRate;
            scaling = true;
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
            if (TopLevel.PlayerHealth > 0)
            {
                enemies.Add(new enemy(StartingPosition, AttackVector * ApproachRate));
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
                if (mode != GameMode.tutorial) HandleScaling(mode);
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
            if (difficulty == GameMode.hard) TopLevel.approachRate = (float) Math.Pow(Math.Log(TopLevel.score, 2), 2) + 8;
            else TopLevel.approachRate = (float) Math.Pow(Math.Log(TopLevel.score, 2), 2) + 2;
            TopLevel.spawnRate = 50/TopLevel.approachRate + 0.5f;
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
                    (EnemyType)UnityEngine.Random.Range(0, numberOfEnemies))
        {
        }

        public void kill()
        {
            DestroyImmediate(au_source);
            DestroyImmediate(source);
        }

    }


    /******************************************Helper Functions*****************************************/

    //Spawn Rate and Approach Rate should be set first or undefined behavior 
    void StartSpawning()
    {
        spawning = true;
        StartCoroutine(spawnMaster());
    }

    void HandlePlayerInputMobile()
    {
        MobileInput.InputType input = mobInput.getInput();
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

    public IEnumerator spawnMaster()
    {
        while (spawning)
        {
            int randomNumber = UnityEngine.Random.Range(0, 3);
            if (((int)playerFacing + 2) % 4 == (int)spawners[randomNumber].initialDirection) continue;
            spawners[randomNumber].spawnEnemy(approachRate);
            yield return new WaitForSeconds(spawnRate);
        }
    }


    /*********************************Begin Unity Automatic Calls***************************************/

	void Start () {

        mobInput = new MobileInput();
        playerAudioSource = GetComponent<AudioSource>();

        //defeat sounds
        swordDefeat = Resources.Load("Sounds/Voicelines/GameOvers/SwordDefeat") as AudioClip;
        AudioClip twoLivesLeft = Resources.Load("Sounds/Voicelines/Lives/2LivesLeft") as AudioClip;
        AudioClip oneLifeLeft = Resources.Load("Sounds/Voicelines/Lives/1LifeLeft") as AudioClip;

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

        heartbeat = Resources.Load("Sounds/Death/heartbeat") as AudioClip;

        weaponMiss = Resources.Load("Sounds/PlayerWeapons/SwingMiss") as AudioClip;
        playerHit = Resources.Load("Sounds/Death/PlayerHit") as AudioClip;

        currentWeapon = PlayerWeapons.hammer;
        playerFacing = Directions.north;

        
        spawners = new List<enemySpawner>();
        //Spawning Locations are cardinal directions
        spawners.Add(new enemySpawner(new Vector3(0, 0, -50), Directions.north, this)); //in front of player
        spawners.Add(new enemySpawner(new Vector3(50, 0, 0), Directions.east, this)); //right of player at start 
        spawners.Add(new enemySpawner(new Vector3(0, 0, 50), Directions.south, this)); //behind player
        spawners.Add(new enemySpawner(new Vector3(-50, 0, 0), Directions.west, this)); //left of player 

        enemyPrefab = Resources.Load("Prefabs/enemy") as GameObject;


        StartSpawning();
	}
    
    // Update is called once per frame
    void Update() {

        //If running on Unity Android, run this block to use mobile input controls
        #if UNITY_ANDROID
            //TODO: Implement a way to escape the game
            //   Maybe grab the data for how long a tap is held, 
            //   and quit if touch is held for 3 seconds
            if (PlayerHealth < 1)
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

        if (PlayerHealth < 1)
        {
            spawning = false;
            for (int i = 0; i < 4; i++) spawners[i].KillAll();
            PlayerHealth = 3;
            mostRecentScore = score;
            score = 0;

            //commented out above because the following code
            //   goes to game over / score screen, and player can Play Again
            StartCoroutine(endGame());
        }
        else
        {
            HandlePlayerInput();
        }
	}

	void OnTriggerEnter()
    {
        PlayerHealth--;
        if (PlayerHealth == 2)
        {
            playerAudioSource.PlayOneShot(playerHit);
        }
        else if (PlayerHealth == 1)
        {
            playerAudioSource.PlayOneShot(playerHit);
            if (PlayerHealth == 1)
            {
                playerAudioSource.PlayOneShot(heartbeat);
            }
        }
        else
        {
            playerAudioSource.PlayOneShot(swordDefeat);

        }

        print(PlayerHealth);
    }

    IEnumerator endGame()
    {
        //go to game over screen after 2 seconds, 
        //to let the "you have been killed" voiceline finished
        yield return new WaitForSeconds(2);
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

    //helper function to play audio for lives remaining
    IEnumerator livesLeft(int lives)
    {
        //wait 2 seconds after oof SFX
        yield return new WaitForSeconds(2);

        //if an audio is playing, do not play more than one thing at a time
        if (audioIsPlaying == false)
        {
            audioIsPlaying = true;
            if (lives == 2)
            {
                playerAudioSource.PlayOneShot(twoLivesLeft);
            }
            if (lives == 1)
            {
                playerAudioSource.PlayOneShot(oneLifeLeft);
            }

            
        }
        //after voiceline finishes, future audio can play
        yield return new WaitForSeconds(2);
        audioIsPlaying = false;
    }
}
