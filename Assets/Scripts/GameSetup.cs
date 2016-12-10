using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    static GameObject playerPrefab;
    static GameObject player;
    static SwordCombat script;

    private AudioSource source;
    private AudioClip mode;

    GameObject screen_tear;
    Animator an;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        mode = Resources.Load("Sounds/BetaVoicelines/SurvivalGameModeSelect") as AudioClip;


        playerPrefab = Resources.Load("Prefabs/Player") as GameObject;
        player = Instantiate(playerPrefab);
        script = player.GetComponent<SwordCombat>();

        screen_tear = GameObject.Find("/Canvas/Screen_Tear");
        screen_tear.SetActive(false);
        script.screen_tear = screen_tear;


        StartCoroutine(playSound());
    }

    // Update is called once per frame
    void Update()
    {

        //if on android, tap will go to arcade game screen
        #if UNITY_ANDROID
        MobileInput.InputType input = MobileInput.getInput();
        if (input == MobileInput.InputType.left)
        {
            SceneManager.LoadScene("SCTutorial");
        }
        else if (input == MobileInput.InputType.up)
        {
            source.Stop();
            script.SetGameMode(SwordCombat.GameMode.normal);
            script.enabled = true;
            Destroy(this);
        }
        else if (input == MobileInput.InputType.right)
        {
            source.Stop();
            script.SetGameMode(SwordCombat.GameMode.hard);
            script.enabled = true;
            Destroy(this);
        }
        else if (input == MobileInput.InputType.down)
        {
            SceneManager.LoadScene("TitleScreen");
        }
        #endif

        if (Input.GetKeyDown("left")) //"Tutorial level"
        {
            SceneManager.LoadScene("SCTutorial");
        }
        else if (Input.GetKeyDown("up")) //Beginning mode
        {
            source.Stop();
            script.SetGameMode(SwordCombat.GameMode.normal);
            script.enabled = true;
            Destroy(this);
        }
        else if (Input.GetKeyDown("right")) //Intermediate
        {
            source.Stop();
            script.SetGameMode(SwordCombat.GameMode.hard);
            script.enabled = true;
            Destroy(this);
        }
        else if (Input.GetKeyDown("down"))
        {
            SceneManager.LoadScene("TitleScreen");
        }

    }

    public IEnumerator playSound()
    {
        while (true)
        {
            source.PlayOneShot(mode);
            yield return new WaitForSeconds(mode.length + 3.0f);
        }
    }
}
