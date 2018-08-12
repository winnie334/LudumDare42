using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour {
	
	public static bool paused;

	public GameObject pauseMenuUI;
	public Text button1Text;
	public Text topFinishText;
	public Text badJokeText;
	public Text endText;
	

	
	
	// Use this for initialization
	void Start () {
		paused = true;
		pauseMenuUI.SetActive(true);	// needed to get components
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			pauseMenuUI.SetActive(!paused);
			paused = !paused;
		}
	}

	public void Resume() {
		if (!paused) return;
		paused = false;
		pauseMenuUI.SetActive(false);
		button1Text.text = "Resume";
	}


	public void QuitGame() {
		// Absolutely WIPES this games' existence from your screen
		// aka closing it
		Debug.Log("Quitting game");
		Application.Quit();
	}

	public void theEnd() {
		Debug.Log("got here");
		topFinishText.enabled = true;
		StartCoroutine("delayedEnd");
	}

	private IEnumerator delayedEnd() {
		yield return new WaitForSeconds(2.5f);
		badJokeText.enabled = true;
		yield return new WaitForSeconds(3);
		endText.enabled = true;
	}

}
