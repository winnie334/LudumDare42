using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class soundPanelController : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler{
	// will be used as a way to call functions when volume slider is altered

	public Image soundIcon;
	public Text reactionText;
	private int imageAmount = 4;
	private Sprite[] soundIcons;
	private bool doneTheHotlineJoke;

	public void Start() {
		soundIcons = new Sprite[imageAmount];
		for (int i = 0; i < imageAmount; i++) {
			soundIcons[i] = Resources.Load<Sprite>("sound_" + i);
			//soundIcons[i] = (Sprite) AssetDatabase.LoadAssetAtPath("Assets/Sprites/sound_" + i + ".png", typeof(Sprite));
		}
	}

	public void volumeChanged(float newValue) {
		changeIcon(newValue);
		changeText(newValue);
	}

	public void changeIcon(float value) {
		// Changes the music icon to something more fitting for the current value.
		// Magic number 0.00001 is to prevent out of bounds error
		soundIcon.sprite = soundIcons[(int) (value * imageAmount - 0.00001)];
	}

	public void changeText(float value) {
		// Changes the text under the slider
		if (value < 0.01) {
			reactionText.text = "Yeah, I understand your choice.";
		} else if (value > 0.01 && value < 0.99) {
			reactionText.text = "";
		} else if (value > 0.99) {
			reactionText.text = "You like the music that much??";
		}
	}

	
	// Code for darkening and lightening the panel on mouse enter.
	public void OnPointerEnter(PointerEventData eventData) {
		GetComponent<Image>().color = new Color32(0, 0, 0, 200);
	}

	public void OnPointerExit(PointerEventData eventData) {
		GetComponent<Image>().color = new Color32(0, 0, 0, 100);
	}
}
