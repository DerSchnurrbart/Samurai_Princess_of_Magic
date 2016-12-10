using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {


    //Audio
    private AudioSource highScoreVoice;
    private AudioClip highScoreClip;

    private AudioSource source;
    private AudioClip youLasted;
    private AudioClip youFollowed;
    private AudioClip youKilled;
    private AudioClip[] score;
    private AudioClip beats;
    private AudioClip directions;
    private AudioClip monsters;


    // Use this for initialization
    void Start () {
        highScoreVoice = GetComponent<AudioSource>();
        highScoreClip = Resources.Load("Sounds/BetaVoicelines/HighScores/HighScores") as AudioClip;

        source = GetComponent<AudioSource>();
        youLasted = Resources.Load("Sounds/BetaVoicelines/HighScores/YouLastedOver") as AudioClip;
        youFollowed = Resources.Load("Sounds/BetaVoicelines/HighScores/YouFollowedOver") as AudioClip;
        youKilled = Resources.Load("Sounds/BetaVoicelines/HighScores/YouKilledOver") as AudioClip;
        beats = Resources.Load("Sounds/BetaVoicelines/GameOver/Beats") as AudioClip;
        directions = Resources.Load("Sounds/BetaVoicelines/GameOver/Directions") as AudioClip;
        monsters = Resources.Load("Sounds/BetaVoicelines/GameOver/Monsters") as AudioClip;

        score = new AudioClip[31];

        //initiate audioclips 0 to 30 with score number voicelines
        initAudioClips();

        //pass in the high scores of each game, then play the voicelines in order
        StartCoroutine(playSound(
            PlayerPrefs.GetInt(UpdateHighScore.highScoreMemoryKey, 0),
            PlayerPrefs.GetInt(UpdateHighScore.highScoreSwordKey, 0),
            PlayerPrefs.GetInt(UpdateHighScore.highScoreRhythmKey, 0)));
    }
	
	// Update is called once per frame
	void Update ()
    {
        //If running on Unity Android, run this block to use mobile input controls
#if UNITY_ANDROID
        if (MobileInput.getInput() == MobileInput.InputType.down)
        {
            SceneManager.LoadScene("MinigameScreen", LoadSceneMode.Single);
        }
#endif
        //Run desktop keyboard/mouse controls
        if (Input.GetKeyDown("down"))
        {
            SceneManager.LoadScene("MinigameScreen", LoadSceneMode.Single);
        }

    }

    //pass in the high scores then read out voiceclines for each game in order
    public IEnumerator playSound(int memoryScore, int swordScore, int rhythmScore)
    {
        while (true)
        {
            source.PlayOneShot(highScoreClip);
            yield return new WaitForSeconds(highScoreClip.length + 0.2f);

            source.PlayOneShot(youFollowed);
            yield return new WaitForSeconds(youFollowed.length + 0.1f);
            source.PlayOneShot(score[getIndex(memoryScore)]);
            yield return new WaitForSeconds(score[getIndex(memoryScore)].length + 0.1f);
            source.PlayOneShot(directions);
            yield return new WaitForSeconds(directions.length + 0.5f);

            source.PlayOneShot(youKilled);
            yield return new WaitForSeconds(youKilled.length + 0.1f);
            source.PlayOneShot(score[getIndex(swordScore)]);
            yield return new WaitForSeconds(score[getIndex(swordScore)].length + 0.1f);
            source.PlayOneShot(monsters);
            yield return new WaitForSeconds(monsters.length + 0.5f);

            source.PlayOneShot(youLasted);
            yield return new WaitForSeconds(youLasted.length + 0.1f);
            source.PlayOneShot(score[getIndex(rhythmScore)]);
            yield return new WaitForSeconds(score[getIndex(rhythmScore)].length + 0.1f);
            source.PlayOneShot(beats);
            yield return new WaitForSeconds(beats.length + 0.5f);

            //loop the voicelines with a delay
            yield return new WaitForSeconds(4);
        }
    }

    //returns the index of what audio clip to be played for the score number
    public int getIndex(int playerScore)
    {
        //index 30 is a audioclip that says "zero"
        if (playerScore == 0) return 30;
        else if (playerScore == 1) return 0;
        else if (playerScore == 2) return 1;
        else if (playerScore == 3) return 2;
        else if (playerScore == 4) return 3;
        else if (playerScore == 5) return 4;
        else if (playerScore == 6) return 5;
        else if (playerScore == 7) return 6;
        else if (playerScore == 8) return 7;
        else if (playerScore == 9) return 8;

        else if (playerScore > 300) return 29;
        else if (playerScore > 250) return 28;
        else if (playerScore > 200) return 27;
        else if (playerScore > 180) return 26;
        else if (playerScore > 160) return 25;
        else if (playerScore > 140) return 24;
        else if (playerScore > 120) return 23;
        else if (playerScore > 100) return 22;
        else if (playerScore > 90) return 21;
        else if (playerScore > 80) return 20;
        else if (playerScore > 70) return 19;
        else if (playerScore > 60) return 18;
        else if (playerScore > 50) return 17;
        else if (playerScore > 45) return 16;
        else if (playerScore > 40) return 15;
        else if (playerScore > 35) return 14;
        else if (playerScore > 30) return 13;
        else if (playerScore > 25) return 12;
        else if (playerScore > 20) return 11;
        else if (playerScore > 15) return 10;

        //it is smaller than 15 and must be 10 or greater, 
        //    so return the index 9 to play the voiceline "over 10"
        return 9;

    }
    public void initAudioClips()
    {
        score[30] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/0") as AudioClip;
        score[0] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/1") as AudioClip;
        score[1] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/2") as AudioClip;
        score[2] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/3") as AudioClip;
        score[3] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/4") as AudioClip;
        score[4] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/5") as AudioClip;
        score[5] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/6") as AudioClip;
        score[6] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/7") as AudioClip;
        score[7] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/8") as AudioClip;
        score[8] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/9") as AudioClip;
        score[9] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/10") as AudioClip;
        score[10] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/15") as AudioClip;
        score[11] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/20") as AudioClip;
        score[12] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/25") as AudioClip;
        score[13] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/30") as AudioClip;
        score[14] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/35") as AudioClip;
        score[15] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/40") as AudioClip;
        score[16] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/45") as AudioClip;
        score[17] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/50") as AudioClip;
        score[18] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/60") as AudioClip;
        score[19] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/70") as AudioClip;
        score[20] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/80") as AudioClip;
        score[21] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/90") as AudioClip;
        score[22] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/100") as AudioClip;
        score[23] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/120") as AudioClip;
        score[24] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/140") as AudioClip;
        score[25] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/160") as AudioClip;
        score[26] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/180") as AudioClip;
        score[27] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/200") as AudioClip;
        score[28] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/250") as AudioClip;
        score[29] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/300") as AudioClip;

    }
}
