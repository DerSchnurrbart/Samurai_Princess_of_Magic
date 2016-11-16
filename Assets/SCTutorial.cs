using UnityEngine;
using System.Collections;

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


	// Use this for initialization
	void Start () {
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

        StartCoroutine(TurningTutorial());
	}
	
	// Update is called once per frame
	void Update () {

        if (allowturning && Input.GetKeyDown("left")) {
            allowturning = false;
            StartCoroutine(rotatePlayer(transform.rotation, transform.rotation * Quaternion.Euler(0, 90, 0), 0.25f));
            StartCoroutine(SwitchWeaponTutorial());
        }
        if(allowWeaponSwitch && Input.GetKeyDown("down"))
        {
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
                //Play Ghost death
                //Despawn Ghost
                StartCoroutine(EnemyMoveTutorial());
            }
            if (weapon == 2)
            {
                StartCoroutine(BugTutorial());

            }
            if (weapon == 3)
            {

                StartCoroutine(FinishedTutorial());

            }
        }
	
	}

    public IEnumerator TurningTutorial()
    {
        playerAudioSource.PlayOneShot(TutorialVoiceLines[0]);
        yield return new WaitForSeconds(TutorialVoiceLines[0].length);
        //Spawn ghost in front of user
        playerAudioSource.PlayOneShot(TutorialVoiceLines[1]);
        yield return new WaitForSeconds(TutorialVoiceLines[1].length);
        //Despawn ghost
        //Spawn ghost to the left of user
        playerAudioSource.PlayOneShot(TutorialVoiceLines[2]);
        yield return new WaitForSeconds(TutorialVoiceLines[2].length/2);

        allowturning = true;
    }

    public IEnumerator SwitchWeaponTutorial()
    {
        playerAudioSource.PlayOneShot(TutorialVoiceLines[3]);
        yield return new WaitForSeconds(TutorialVoiceLines[3].length/2);
        allowWeaponSwitch = true;
    }
    public IEnumerator AttackTutorial()
    {
        playerAudioSource.PlayOneShot(TutorialVoiceLines[4]);
        yield return new WaitForSeconds(TutorialVoiceLines[4].length/2);
        allowAttack = true;
    }
    public IEnumerator EnemyMoveTutorial()
    {
        playerAudioSource.PlayOneShot(TutorialVoiceLines[5]);
        yield return new WaitForSeconds(TutorialVoiceLines[5].length);
        //Spawn Wolf that moves toward player
        playerAudioSource.PlayOneShot(TutorialVoiceLines[6]);
        //MAKE SURE g is FINISHED playerhit noise, wolf despawns
        playerAudioSource.PlayOneShot(TutorialVoiceLines[7]);
        yield return new WaitForSeconds(TutorialVoiceLines[7].length);
        //Spawn wolf that doesn't move
        playerAudioSource.PlayOneShot(TutorialVoiceLines[8]);
        yield return new WaitForSeconds(TutorialVoiceLines[8].length/2);
        allowWeaponSwitch = true;
    }
    
    public IEnumerator BugTutorial()
    {
        playerAudioSource.PlayOneShot(TutorialVoiceLines[9]);
        yield return new WaitForSeconds(TutorialVoiceLines[9].length);
        allowWeaponSwitch = true;
    }

    public IEnumerator FinishedTutorial()
    {
        playerAudioSource.PlayOneShot(TutorialVoiceLines[10]);
        yield return new WaitForSeconds(TutorialVoiceLines[10].length);
        //Load Sword Combat Scene
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

}
