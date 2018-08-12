using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	public float speed;
	public float jumpForce;
	public float timeBetweenClones;
	public GameObject clone;
	public ParticleSystem deadParticle;
	public Canvas canvas;

	private Rigidbody2D rb2D;
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private Collider2D myCollider;
	private bool canJump = true;
	private bool pressedJump;
	private bool alive = true;
	private float cloneTimer;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rb2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		myCollider = GetComponent<Collider2D>();
		audioSource = GetComponent<AudioSource>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Update() {
		processInput();

		if (stuck()) die();

		processTimers();

		rayCastToGround();

	}

	private void processInput() {
		if (pauseMenu.paused) return;
		
		if (Input.GetKeyDown("r")) {
			reset();
		}

		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			cloneTimer = timeBetweenClones;
		}

		if (Input.GetKey(KeyCode.Mouse0)) {
			cloneTimer += Time.deltaTime;
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			pressedJump = true;
		}
	}

	private bool stuck() {
//		// Beautiful name
//		// checks if able to move
//		Debug.DrawRay(transform.position, 0.55f * Vector2.down, Color.green);
//		Debug.DrawRay(transform.position, 0.55f * Vector2.up, Color.green);
//		Debug.DrawRay(transform.position, 0.55f * Vector2.left, Color.green);
//		Debug.DrawRay(transform.position, 0.55f * Vector2.right, Color.green);
//		RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.down, 0.55f);
//		RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left, 0.55f);
//		RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.up, 0.55f);
//		RaycastHit2D hit4 = Physics2D.Raycast(transform.position, Vector2.right, 0.55f);
//		return hit1.collider != null && hit2.collider != null && hit3.collider != null && hit4.collider != null;
//		return hit1.collider.Distance(colli).distance < 0.1f &&
//		       hit2.collider.Distance(colli).distance < 0.1f &&
//		       hit3.collider.Distance(colli).distance < 0.1f &&
//		       hit4.collider.Distance(colli).distance < 0.1f;
		return false;
	}

	private void processTimers() {
		if (cloneTimer > timeBetweenClones) {
			var copy = Instantiate(clone, transform.position, Quaternion.identity);
			copy.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
			copy.GetComponent<deadScript>().setPlayer(gameObject);
			cloneTimer = 0;
		}
	}

	private void rayCastToGround() {
		Debug.DrawRay(transform.position + new Vector3(0.43f, 0, 0), 0.55f * Vector2.down, Color.red);
		Debug.DrawRay(transform.position - new Vector3(0.43f, 0, 0), 0.55f * Vector2.down, Color.red);
		Debug.DrawRay(transform.position, 0.55f * Vector2.down, Color.red);
		RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(0.43f, 0, 0), Vector2.down, 0.55f);
		RaycastHit2D hit2 = Physics2D.Raycast(transform.position - new Vector3(0.43f, 0, 0), Vector2.down, 0.55f);
		RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, 0.55f);
		canJump = hit1.collider != null || hit2.collider != null || hit3.collider != null;
	}

	private void FixedUpdate() {
		movement();
		jump();
	}

	private void movement() {
		if (pauseMenu.paused || !alive) return;
		var horizontal = (int) Input.GetAxisRaw("Horizontal");
		transform.position = new Vector2(transform.position.x + horizontal * speed, transform.position.y);
		//rb2D.velocity = new Vector2(horizontal * speed, rb2D.velocity.y);
		if (horizontal != 0) {
			spriteRenderer.flipX = horizontal < 0;
			animator.SetInteger("moveState", 1);
		} else {
			animator.SetInteger("moveState", 0);
		}
	}

	private void jump() {
		if (pressedJump && canJump) {
			rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
			canJump = false;
		}
		pressedJump = false;
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Danger")) {
			die();
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Finish")) {
			levelController.increaseLevel();
			reset();
		}

		if (other.gameObject.CompareTag("Speedsign")) {
			speed += 0.15f;
		}

		if (other.gameObject.CompareTag("Endgame")) {
			Debug.Log("great..");
			rb2D.gravityScale = 0;
			var currentPos = transform.position;
			reset();
			transform.position = currentPos;
			animator.enabled = false;
			spriteRenderer.enabled = false;
			levelController.triggerEnd();
			canvas.GetComponent<pauseMenu>().theEnd();
		}
	}

	public void die() {
		alive = false;
		Instantiate(deadParticle, transform.position, Quaternion.identity);
		audioSource.Play();
		levelController.deaths += 1;
		spriteRenderer.enabled = false;
		myCollider.enabled = false;
		animator.enabled = false;
		rb2D.gravityScale = 0;
		rb2D.velocity = Vector2.zero;
		
		StartCoroutine(delayedReset());
	}

	IEnumerator delayedReset() {
		yield return new WaitForSeconds(1.3f);
		reset();
		spriteRenderer.enabled = true;
		myCollider.enabled = true;
		animator.enabled = true;
		rb2D.gravityScale = 3;
	}

	public void reset() {
		alive = true;
		transform.position = levelController.getSpawnPoint();
		GameObject.Find("spaceBackground").transform.position = Camera.main.transform.TransformPoint(new Vector3(0, 0, 0.46f)); // todo
		rb2D.velocity = Vector2.zero;
		cloneTimer = 0;
		
		
		foreach (var copy in GameObject.FindGameObjectsWithTag("Dead")) {
			Destroy(copy);
		}

		foreach (var thing in FindObjectsOfType<GameObject>()) {
			try {
				thing.GetComponent<fireballController>().reset();
			} catch {
				// Object was not a fireball
			}
		}
	}
}
