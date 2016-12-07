using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    /*
    //Mobile Touch Input
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    */
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
        //define what % of the screen is needed to be touched for a swipe to register
        //dragDistance = Screen.height * 15 / 100;
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
#endif
        //if on desktop, use keyboard controls
        if (Input.GetKeyDown("escape"))
        {
            print("ESCAPE");
            SceneManager.LoadScene("TitleScreen");

        }

        if (Input.GetKeyDown("left")) //"Tutorial level", just for explaining gameplay
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

        }

    }

    public IEnumerator playSound()
    {
        yield return new WaitForSeconds(0);
        source.PlayOneShot(mode);
    }
}
