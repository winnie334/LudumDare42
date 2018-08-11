using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadScript : MonoBehaviour {

	
	public Texture2D texture;
	public float timeTillSolid;


	private SpriteRenderer spriteRenderer;
	private Collider2D colliderComponent;
	private GameObject player;

	private float initialAlpha;
	private float timeAlive;
	private bool initialized;
	
	
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		colliderComponent = GetComponent<Collider2D>();
		
		Sprite[] sprites = Resources.LoadAll<Sprite>(texture.name);
		spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
		initialAlpha = spriteRenderer.color.a;
	}
	
	// Update is called once per frame
	void Update () {
		timeAlive += Time.deltaTime;
		if (!initialized) changeColor();
//		if (timeAlive > 2) {
//			Destroy(gameObject);
//		}
	}

	private void changeColor() {
		var newAlpha = timeAlive / timeTillSolid * (1 - initialAlpha);
		spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, initialAlpha + newAlpha);
		initialized = timeAlive > timeTillSolid;
		if (initialized) {
			colliderComponent.enabled = true;
//			if (Vector2.Distance(player.transform.position, transform.position) < 0.05f)
//				player.GetComponent<playerController>().reset();
		}
	}

	public void setPlayer(GameObject parent) {
		player = parent;
	}
}
