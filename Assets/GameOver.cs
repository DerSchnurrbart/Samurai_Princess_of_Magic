using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    /*
    //Mobile Touch Input
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    */

    //Audio
    private AudioSource gameOverVoice;
    private AudioClip gameOverClip;

    private AudioSource source;
    private AudioClip playAgain;
    private AudioClip youLasted;
    private AudioClip youFollowed;
    private AudioClip youKilled;
    private AudioClip[] score;
    private AudioClip rounds;
    private AudioClip directions;
    private AudioClip monsters;
    private AudioClip[] compliment;
    bool gameOverVoiceIsPlaying = false;
    bool gameOverLoaded = false;

    //saves the score of the last played game into variable to reduce repetitive code
    int newestScore;

    // Use this for initialization
    void Start()
    {
        /*
        //define what % of the screen is needed to be touched for a swipe to register
        dragDistance = Screen.height * 15 / 100;
        */

        gameOverVoice = GetComponent<AudioSource>();
        gameOverClip = Resources.Load("Sounds/Voicelines/GameOvers/GameOver") as AudioClip;

        source = GetComponent<AudioSource>();
        playAgain = Resources.Load("Sounds/BetaVoicelines/GameOver/PlayAgain") as AudioClip;
        youLasted = Resources.Load("Sounds/BetaVoicelines/GameOver/GOYouLasted") as AudioClip;
        youFollowed = Resources.Load("Sounds/BetaVoicelines/GameOver/GOYouFollowed") as AudioClip;
        youKilled = Resources.Load("Sounds/BetaVoicelines/GameOver/GOYouKilled") as AudioClip;
        rounds = Resources.Load("Sounds/BetaVoicelines/GameOver/Rounds") as AudioClip;
        directions = Resources.Load("Sounds/BetaVoicelines/GameOver/Directions") as AudioClip;
        monsters = Resources.Load("Sounds/BetaVoicelines/GameOver/Monsters") as AudioClip;

        score = new AudioClip[30];
        compliment = new AudioClip[5];

        //initiate audioclips 0 to 29 with score number voicelines
        //   and 0 to 4 with compliments
        initAudioClips();

        //grab the score
        if (Load.lastPlayedGame == 1)
        {
            newestScore = MemoryAlt.score;
        }
        else if (Load.lastPlayedGame == 2)
        {
            newestScore = SwordCombat.mostRecentScore;
        }
        else if (Load.lastPlayedGame == 3)
        {
            newestScore = Rhythm.score;
        }

        StartCoroutine(playSound());
    }

    // Update is called once per frame
    void Update()
    {

        if (gameOverVoiceIsPlaying == false)
        {
            gameOverVoiceIsPlaying = true;
            gameOverVoice.PlayOneShot(gameOverClip);
        }

        //If running on Unity Android, run this block to use mobile input controls
#if UNITY_ANDROID
        if (MobileInput.getInput() == MobileInput.InputType.up)
        {
            SceneManager.LoadScene(Load.lastPlayedGame, LoadSceneMode.Single);
        }
        if (MobileInput.getInput() == MobileInput.InputType.down)
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
#endif
        //Run desktop keyboard/mouse controls

        if (Input.GetKeyDown("up"))
        { 
            SceneManager.LoadScene(Load.lastPlayedGame, LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown("down"))
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }

        //This function returns the game to main menu after 3 seconds
        //StartCoroutine(gameOverScreen());

    }

    public IEnumerator playSound()
    {
        //if rhythm magic
        if (Load.lastPlayedGame == 3)
        {
            source.PlayOneShot(youLasted);
            yield return new WaitForSeconds(youLasted.length + 0.1f);
            source.PlayOneShot(score[getIndex(newestScore)]);
            yield return new WaitForSeconds(score[getIndex(newestScore)].length + 0.1f);
            source.PlayOneShot(rounds);
            yield return new WaitForSeconds(rounds.length);
        }
        else if (Load.lastPlayedGame == 1)
        {
            source.PlayOneShot(youFollowed);
            yield return new WaitForSeconds(youFollowed.length + 0.1f);
            source.PlayOneShot(score[getIndex(newestScore)]);
            yield return new WaitForSeconds(score[getIndex(newestScore)].length + 0.1f);
            source.PlayOneShot(directions);
            yield return new WaitForSeconds(directions.length);
        }
        else if (Load.lastPlayedGame == 2)
        {
            source.PlayOneShot(youKilled);
            yield return new WaitForSeconds(youKilled.length + 0.1f);
            source.PlayOneShot(score[getIndex(newestScore)]);
            yield return new WaitForSeconds(score[getIndex(newestScore)].length + 0.1f);
            source.PlayOneShot(monsters);
            yield return new WaitForSeconds(monsters.length);
        }

        //play the compliment voicelines
        if (newestScore >= 0)
        {
            source.PlayOneShot(compliment[0]);
            yield return new WaitForSeconds(compliment[0].length);
        }
        else if (newestScore >= 5)
        {
            source.PlayOneShot(compliment[1]);
            yield return new WaitForSeconds(compliment[1].length);
        }
        if (newestScore >= 15)
        {
            source.PlayOneShot(compliment[2]);
            yield return new WaitForSeconds(compliment[2].length);
        }
        if (newestScore >= 25)
        {
            source.PlayOneShot(compliment[3]);
            yield return new WaitForSeconds(compliment[3].length);
        }
        if (newestScore >= 35)
        {
            source.PlayOneShot(compliment[4]);
            yield return new WaitForSeconds(compliment[4].length);
        }

        yield return new WaitForSeconds(0.5f);
        source.PlayOneShot(playAgain);
    }

    //returns the index of what audio clip to be played for the score number
    public int getIndex(int playerScore)
    {
        if (newestScore == 1) return 0;
        else if (newestScore == 2) return 1;
        else if (newestScore == 3) return 2;
        else if (newestScore == 4) return 3;
        else if (newestScore == 5) return 4;
        else if (newestScore == 6) return 5;
        else if (newestScore == 7) return 6;
        else if (newestScore == 8) return 7;
        else if (newestScore == 9) return 8;
        else if (newestScore == 10) return 9;
        else if (newestScore > 15) return 10;
        else if (newestScore > 20) return 11;
        else if (newestScore > 25) return 12;
        else if (newestScore > 30) return 13;
        else if (newestScore > 35) return 14;
        else if (newestScore > 40) return 15;
        else if (newestScore > 45) return 16;
        else if (newestScore > 50) return 17;
        else if (newestScore > 60) return 18;
        else if (newestScore > 70) return 19;
        else if (newestScore > 80) return 20;
        else if (newestScore > 90) return 21;
        else if (newestScore > 100) return 22;
        else if (newestScore > 120) return 23;
        else if (newestScore > 140) return 24;
        else if (newestScore > 160) return 25;
        else if (newestScore > 180) return 26;
        else if (newestScore > 200) return 27;
        else if (newestScore > 250) return 28;
        else return 29;
    }

    public void initAudioClips()
    {
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
        score[19] = Resources.Load("Sounds/BetaVoicelines/GameOver/Score/0") as AudioClip;
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

        compliment[0] = Resources.Load("Sounds/BetaVoicelines/GameOver/NotBad") as AudioClip;
        compliment[1] = Resources.Load("Sounds/BetaVoicelines/GameOver/WellDone") as AudioClip;
        compliment[2] = Resources.Load("Sounds/BetaVoicelines/GameOver/GreatJob") as AudioClip;
        compliment[3] = Resources.Load("Sounds/BetaVoicelines/GameOver/Amazing") as AudioClip;
        compliment[4] = Resources.Load("Sounds/BetaVoicelines/GameOver/Fantastic") as AudioClip;

    }

    /*
    IEnumerator gameOverScreen()
    {
        //once voiceline is finished, return to main menu
        yield return new WaitForSeconds(3);

        print("changing scenes");
        SceneManager.LoadScene("TitleScreen");
    }
    */

    /*
void MobileInput()
{
    // user is touching the screen with one finger
    if (Input.touchCount == 1)
    {
        Touch touch = Input.GetTouch(0);

        //get coordinates of the first touch
        if (touch.phase == TouchPhase.Began)
        {
            fp = touch.position;
            lp = touch.position;
        }
        //update the last position based on where they moved
        else if (touch.phase == TouchPhase.Moved)
        {
            lp = touch.position;
        }
        //check if the finger is removed from the screen
        else if (touch.phase == TouchPhase.Ended)
        {
            lp = touch.position;

            //Check if drag distance is greater than 15% of the screen height
            if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
            {

                //check if the drag is vertical
                if (Mathf.Abs(lp.x - fp.x) < Mathf.Abs(lp.y - fp.y))
                {
                    //last touch position is more up of first touch position
                    if (lp.y > fp.y)
                    {

                        Debug.Log("Up Swipe");
                        SceneManager.LoadScene(Load.lastPlayedGame, LoadSceneMode.Single);
                    }
                    //movement is a down swipe
                    else
                    {
                        Debug.Log("Down Swipe");
                        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
                    }
                }
            }
        }
    }
}*/


}




