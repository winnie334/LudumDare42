using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballController : MonoBehaviour {

	public Vector2 force;

	private Vector2 spawn;
	//private Animator animator;
	
	
	// Use this for initialization
	// great, an entire script for a simple force.
	void Start () {
		GetComponent<Rigidbody2D>().AddForce(force);
		//animator = GetComponent<Animator>();
		spawn = transform.position;
		//if (!GetComponent<SpriteRenderer>().isVisible) animator.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void reset() {
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().AddForce(force);
		transform.position = spawn;
	}

//	private void OnBecameVisible() {
//		animator.enabled = true;
//	}
//
//	private void OnBecameInvisible() {
//		animator.enabled = false;
//	}
}
