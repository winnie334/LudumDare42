using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class laserController : MonoBehaviour {

	public Vector3 direction;
	public float timeBetweenLaser;
	public float timeLaserOn;
	public float offset;
	public ParticleSystem laserParticle;

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
			if (QualitySettings.GetQualityLevel() > 2 && (int) (timer * 100) % 5 == 0) {
				var uggh = laserParticle.shape;
				uggh.rotation = -new Vector3(0, Mathf.Rad2Deg * Mathf.Atan(direction.x / direction.y), 0);
				Instantiate(laserParticle, new Vector3(hit.point.x, hit.point.y, 0) - direction / 10, Quaternion.identity);
			}

			if (hit.collider.gameObject.CompareTag("Player"))
				hit.collider.gameObject.GetComponent<playerController>().die();
		} else lineRenderer.SetPosition(1, transform.position + 1000 * direction);
			
	}
}
