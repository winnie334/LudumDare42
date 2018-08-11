using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserController : MonoBehaviour {

	public Vector3 direction;
	public float timeBetweenLaser;
	public float timeLaserOn;
	public float offset;

	private float timer;
	private bool warmingUp;
	private LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		timer = offset;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (warmingUp && timer > timeBetweenLaser) {
			lineRenderer.enabled = true;
			warmingUp = false;
			timer = 0;
		}

		if (!warmingUp && timer > timeLaserOn) {
			lineRenderer.enabled = false;
			warmingUp = true;
			timer = 0;
		}

		if (!warmingUp) fireLaser();

	}

	public void fireLaser() {
		// yay, raycasts
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
		lineRenderer.SetPosition(0, transform.position);
		if (hit.collider != null) {
			lineRenderer.SetPosition(1, hit.point);
			if (hit.collider.gameObject.CompareTag("Player"))
				hit.collider.gameObject.GetComponent<playerController>().die();
		} else lineRenderer.SetPosition(1, transform.position + 1000 * direction);
			
	}
}
