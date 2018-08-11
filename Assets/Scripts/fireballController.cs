using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballController : MonoBehaviour {

	public Vector2 force;

	private Vector2 spawn;
	
	
	// Use this for initialization
	// great, an entire script for a simple force.
	void Start () {
		GetComponent<Rigidbody2D>().AddForce(force);
		spawn = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void reset() {
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().AddForce(force);
		transform.position = spawn;
	}
}
