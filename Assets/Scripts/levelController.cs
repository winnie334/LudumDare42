using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class levelController : MonoBehaviour {

	public static int currentLetter;
	public static float timeBusy;	//Time since this level started
	public static int deaths;	// deaths in this level

	private static GameObject[] spawnPoints;
	private static string numberString;
	private static bool finished;

	private static Vector3 camVelocity;
	private static float camSize;
	private static float backgroundVelocity;
	
	private KeyCode[] keyCodesNumpad = {
			KeyCode.Alpha0,
			KeyCode.Alpha1,
			KeyCode.Alpha2,
			KeyCode.Alpha3,
			KeyCode.Alpha4,
			KeyCode.Alpha5,
			KeyCode.Alpha6,
			KeyCode.Alpha7,
			KeyCode.Alpha8,
			KeyCode.Alpha9
		};

	private string[] keyCodesActualNumpad = {
		"[0]",
		"[1]",
		"[2]",
		"[3]",
		"[4]",
		"[5]",
		"[6]",
		"[7]",
		"[8]",
		"[9]"
	};

	private static string[] skipcodes = {
		"6244",
		"1936",
		"0550",
		"0811",
		"134863"
	};
 
		

	public void Start() {
		spawnPoints = new GameObject[6];
		for (int i = 1; i < 7; i++) {
			spawnPoints[i - 1] = GameObject.Find("spawnTeleport" + i);
		}
	}

	public void Update() {
		if (finished) {
			playEnd();
			return;
		}
		
		timeBusy += Time.deltaTime;

		for (int i = 0; i < keyCodesNumpad.Length; i++) {
			if (Input.GetKeyDown(keyCodesNumpad[i]) || Input.GetKeyDown(keyCodesActualNumpad[i])) {
				numberString += i;
				Debug.Log(numberString);
				checkString();
			}
		}

		if (deaths > 11 || timeBusy > 4 * 60 || (deaths > 3 && timeBusy > 1.5 * 60)) {
			GameObject.Find("skipsign" + (currentLetter + 1)).GetComponent<SpriteRenderer>().enabled = true;
		}
	}

	private static void checkString() {
		// checks if there are any skipcodes in the string
		for (int i = 0; i < skipcodes.Length; i++) {
			if (numberString.Contains(skipcodes[i])) {
				currentLetter = i + 1;
				timeBusy = 0;
				deaths = 0;
				GameObject.Find("Player").GetComponent<playerController>().reset();
				numberString = "";
			}
		}
	}

	public static Vector2 getSpawnPoint() {
		return spawnPoints[currentLetter].transform.position;
	}

	public static void increaseLevel() {
		currentLetter += 1;
		timeBusy = 0;
		deaths = 0;
	}

	public static void triggerEnd() {
		finished = true;
		Camera.main.transform.parent = null;
	}

	private static void playEnd() {
		var time = 2f;
		
		Vector3 targetPosition = new Vector3(71.5f, 12, -10);

		// Smoothly move the camera towards that target position
		Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref camVelocity, time);

		var targetSize = 51.6f;
		float newSize = Mathf.SmoothDamp(Camera.main.orthographicSize, targetSize, ref camSize, time);
		Camera.main.orthographicSize = newSize;

		var backgroundTarget = 0.04f;
		float newScale = Mathf.SmoothDamp(Camera.main.transform.GetChild(0).transform.localScale.x, backgroundTarget,
			ref backgroundVelocity, time);
		Camera.main.transform.GetChild(0).transform.localScale = new Vector3(newScale, newScale, 1);
		
	}
}
