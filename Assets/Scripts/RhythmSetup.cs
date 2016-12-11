﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RhythmSetup : MonoBehaviour
{
	static GameObject script_holder;
	static Rhythm rhythm_game;

	private AudioSource source;
	private AudioClip mode;

	// Use this for initialization
	void Start()
	{
		source = GetComponent<AudioSource>();
		mode = Resources.Load("Sounds/BetaVoicelines/Rhythm/RhythmSetupVoiceline") as AudioClip;

		rhythm_game = gameObject.GetComponent<Rhythm>();

		StartCoroutine(playSound());
	}

	// Update is called once per frame
	void Update()
	{

		//if on android, tap will go to arcade game screen
		#if UNITY_ANDROID
		MobileInput.InputType input = MobileInput.getInput();
        if (input == MobileInput.InputType.hold)
        {
            SceneManager.LoadScene("TitleScreen");
        }
        else if (input == MobileInput.InputType.left)
		{
            SceneManager.LoadScene("RhythmTutorial");
		}
		else if (input == MobileInput.InputType.up)
		{
            source.Stop();
            rhythm_game.SetGameMode(Rhythm.GameMode.normal);
            rhythm_game.enabled = true;
            this.enabled = false;
		}
		else if (input == MobileInput.InputType.right)
		{
            source.Stop();
            rhythm_game.SetGameMode(Rhythm.GameMode.normal);
            rhythm_game.enabled = true;
            this.enabled = false;
		}
        else if (input == MobileInput.InputType.down)
        {
            SceneManager.LoadScene("TitleScreen");
        }
		#endif

		if (Input.GetKeyDown("left")) //"Tutorial level", just for explaining gameplay
		{
			SceneManager.LoadScene("RhythmTutorial");
		}
		else if (Input.GetKeyDown("up")) //Beginning mode
		{
			source.Stop();
			rhythm_game.SetGameMode(Rhythm.GameMode.normal);
			rhythm_game.enabled = true;
            this.enabled = false;
		}
		else if (Input.GetKeyDown("right")) //Intermediate
		{
			source.Stop();
			rhythm_game.SetGameMode(Rhythm.GameMode.hard);
			rhythm_game.enabled = true;
            this.enabled = false;
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
