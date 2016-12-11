using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SCTutorial : MonoBehaviour {

    enum EnemyType { insect, wolf, ghost };
    enum PlayerWeapons { hammer, sword, magic_staff }; 

    static bool allowturning = false;
    static bool allowWeaponSwitch = false;
    static bool allowAttack = false;
    static int weapon = 0;
    static AudioSource playerAudioSource;
    static AudioClip playerHit;
    static AudioClip[] enemyNoises;
    static AudioClip[] enemyDeaths;
    static AudioClip[] weaponEquipSound;
    static AudioClip[] TutorialVoiceLines;
    static GameObject enemyPrefab;
    static enemy currentEnemy;


    class enemy
    {
        GameObject source;
        AudioSource au_source; //Houses positional information, audio clip
        public EnemyType type;
        public PlayerWeapons weakness;

        public enemy(Vector3 pos, float approachSpeed, EnemyType type_in)
        {
            source = Instantiate(enemyPrefab);
            source.transform.position = pos;
            au_source = source.GetComponent<AudioSource>();
            au_source.clip = enemyNoises[(int)type_in];
            au_source.Play();
            source.GetComponent<Rigidbody>().velocity = -1 * pos.normalized * approachSpeed;
            type = type_in;
            weakness = (PlayerWeapons) type_in;
        }

        public void kill()
        {
            Destroy(au_source);
            Destroy(source);
        }

    }





	// Use this for initialization
	void Start () {

        playerAudioSource = GetComponent<AudioSource>();
        enemyPrefab = Resources.Load("Prefabs/enemy") as GameObject;

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

        playerHit = Resources.Load("Sounds/Death/PlayerHit") as AudioClip;

        TutorialVoiceLines = new AudioClip[11];
        TutorialVoiceLines[0] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/WelcomeToTheSwordTutorial") as AudioClip;
        TutorialVoiceLines[1] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/DoYouHearThatGhost") as AudioClip;
        TutorialVoiceLines[2] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/HowAboutNow") as AudioClip;
        TutorialVoiceLines[3] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/NowThatYoureLooking") as AudioClip;
        TutorialVoiceLines[4] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/SwipeUpToAttack") as AudioClip;
        TutorialVoiceLines[5] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/GoodJob") as AudioClip;
        TutorialVoiceLines[6] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/ShouldHearAWolf") as AudioClip;
        TutorialVoiceLines[7] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/WhenEnemyGetsClose") as AudioClip;
        TutorialVoiceLines[8] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/SwitchToTheSword") as AudioClip;
        TutorialVoiceLines[9] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/CrinklingPaper") as AudioClip;
        TutorialVoiceLines[10] = Resources.Load("Sounds/BetaVoicelines/SwordCombatTutorial/Congrats") as AudioClip;

        StartCoroutine(TurningTutorial());
	}
	
	// Update is called once per frame
	void Update () {

#if UNITY_ANDROID
        if (MobileInput.getInput() == MobileInput.InputType.hold)
        {
            SceneManager.LoadScene("TitleScreen");
        }

        if (allowturning && MobileInput.getInput() == MobileInput.InputType.left)
        {
            allowturning = false;
            StartCoroutine(rotatePlayer(transform.rotation, transform.rotation * Quaternion.Euler(0, 90, 0), 0.25f));
            StartCoroutine(SwitchWeaponTutorial());
        }
        if (allowWeaponSwitch && MobileInput.getInput() == MobileInput.InputType.down)
        {
            weapon++;
            allowWeaponSwitch = false;
            if (weapon == 1)
            {
                playerAudioSource.PlayOneShot(weaponEquipSound[(int)PlayerWeapons.magic_staff]);
                StartCoroutine(AttackTutorial());
            }
            if (weapon == 2)
            {
                playerAudioSource.PlayOneShot(weaponEquipSound[(int)PlayerWeapons.sword]);
                allowAttack = true;
            }
            if (weapon == 3)
            {
                playerAudioSource.PlayOneShot(weaponEquipSound[(int)PlayerWeapons.hammer]);
                allowAttack = true;

            }
        }
        if (allowAttack && MobileInput.getInput() == MobileInput.InputType.up)
        {
            allowAttack = false;
            if (weapon == 1)
            {
                currentEnemy.kill();
                playerAudioSource.PlayOneShot(enemyDeaths[(int)EnemyType.ghost]);
                StartCoroutine(EnemyMoveTutorial());
            }
            if (weapon == 2)
            {
                currentEnemy.kill();
                playerAudioSource.PlayOneShot(enemyDeaths[(int)EnemyType.wolf]);
                StartCoroutine(BugTutorial());

            }
            if (weapon == 3)
            {
                currentEnemy.kill();
                playerAudioSource.PlayOneShot(enemyDeaths[(int)EnemyType.insect]);
                StartCoroutine(FinishedTutorial());
            }
        }
#endif

        if (allowturning && Input.GetKeyDown("left")) {
            allowturning = false;
            StartCoroutine(rotatePlayer(transform.rotation, transform.rotation * Quaternion.Euler(0, 90, 0), 0.25f));
            StartCoroutine(SwitchWeaponTutorial());
        }
        if(allowWeaponSwitch && Input.GetKeyDown("down"))
        {
            weapon++;
            allowWeaponSwitch = false;
            if (weapon == 1)
            {
                playerAudioSource.PlayOneShot(weaponEquipSound[(int)PlayerWeapons.magic_staff]);
                StartCoroutine(AttackTutorial());
            } 
            if (weapon == 2)
            {
                playerAudioSource.PlayOneShot(weaponEquipSound[(int)PlayerWeapons.sword]);
                allowAttack = true;
            } 
            if (weapon == 3)
            {
                playerAudioSource.PlayOneShot(weaponEquipSound[(int)PlayerWeapons.hammer]);
                allowAttack = true;

            } 
        }
        if(allowAttack && Input.GetKeyDown("up"))
        {
            allowAttack = false;
            if (weapon == 1)
            {
                currentEnemy.kill();
                playerAudioSource.PlayOneShot(enemyDeaths[(int)EnemyType.ghost]);
                StartCoroutine(EnemyMoveTutorial());
            }
            if (weapon == 2)
            {
                currentEnemy.kill();
                playerAudioSource.PlayOneShot(enemyDeaths[(int)EnemyType.wolf]);
                StartCoroutine(BugTutorial());

            }
            if (weapon == 3)
            {
                currentEnemy.kill();
                playerAudioSource.PlayOneShot(enemyDeaths[(int)EnemyType.insect]);
                StartCoroutine(FinishedTutorial());
            }
        }
	
	}

    public IEnumerator TurningTutorial()
    {
        playerAudioSource.PlayOneShot(TutorialVoiceLines[0]);
        yield return new WaitForSeconds(TutorialVoiceLines[0].length + 0.25f);
        currentEnemy = new enemy(new Vector3(0, 0, -25), 0.0f, EnemyType.ghost);
        yield return new WaitForSeconds(4.0f);
        playerAudioSource.PlayOneShot(TutorialVoiceLines[1]);
        yield return new WaitForSeconds(TutorialVoiceLines[1].length + 1.0f);
        currentEnemy.kill();
        currentEnemy = new enemy(new Vector3(-25, 0, 0), 0.0f, EnemyType.ghost);
        yield return new WaitForSeconds(4.0f);
        playerAudioSource.PlayOneShot(TutorialVoiceLines[2]);
        yield return new WaitForSeconds(TutorialVoiceLines[2].length);

        allowturning = true;
    }

    public IEnumerator SwitchWeaponTutorial()
    {
        yield return new WaitForSeconds(3.0f);
        playerAudioSource.PlayOneShot(TutorialVoiceLines[3]);
        yield return new WaitForSeconds(TutorialVoiceLines[3].length);
        allowWeaponSwitch = true;
    }
    public IEnumerator AttackTutorial()
    {
        yield return new WaitForSeconds(1.5f);
        playerAudioSource.PlayOneShot(TutorialVoiceLines[4]);
        yield return new WaitForSeconds(TutorialVoiceLines[4].length);
        allowAttack = true;
    }
    public IEnumerator EnemyMoveTutorial()
    {
        yield return new WaitForSeconds(1.5f);
        playerAudioSource.PlayOneShot(TutorialVoiceLines[5]);
        yield return new WaitForSeconds(TutorialVoiceLines[5].length + 0.25f);
        playerAudioSource.PlayOneShot(TutorialVoiceLines[6]);
        yield return new WaitForSeconds(1.0f);
        currentEnemy = new enemy(new Vector3(-50, 0, 0), 5.0f, EnemyType.wolf);
        yield return new WaitForSeconds(11.0f);
        playerAudioSource.PlayOneShot(TutorialVoiceLines[7]);
        yield return new WaitForSeconds(TutorialVoiceLines[7].length + 0.25f);
        currentEnemy = new enemy(new Vector3(-50, 0, 0), 0.0f, EnemyType.wolf);
        playerAudioSource.PlayOneShot(TutorialVoiceLines[8]);
        yield return new WaitForSeconds(TutorialVoiceLines[8].length);
        allowWeaponSwitch = true;
    }
    
    public IEnumerator BugTutorial()
    {
        yield return new WaitForSeconds(1.5f);
        currentEnemy = new enemy(new Vector3(-50, 0, 0), 0.0f, EnemyType.insect);
        playerAudioSource.PlayOneShot(TutorialVoiceLines[9]);
        yield return new WaitForSeconds(TutorialVoiceLines[9].length);
        allowWeaponSwitch = true;
    }

    public IEnumerator FinishedTutorial()
    {
        yield return new WaitForSeconds(1.5f);
        playerAudioSource.PlayOneShot(TutorialVoiceLines[10]);
        yield return new WaitForSeconds(TutorialVoiceLines[10].length + 1.0f);
        SceneManager.LoadScene("SwordCombat");
    }

    public IEnumerator rotatePlayer(Quaternion startingRotation, Quaternion endingRotation, float duration)
    {
        float endTime = Time.time + duration;
        while(Time.time <= endTime)
        {
            float percentElapsed = 1 - ((endTime - Time.time) / duration);
            transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, percentElapsed);
            yield return 0; 
        }
        transform.rotation = endingRotation;
    }

	void OnTriggerEnter()
    {
        currentEnemy.kill();
        playerAudioSource.PlayOneShot(playerHit);
    }
}
