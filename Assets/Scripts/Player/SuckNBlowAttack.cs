using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuckNBlowAttack : MonoBehaviour {

	SquishyController squishyController;
	GameObject graphics;

	public float carryModifier = .3f;
	[System.NonSerialized]
	public bool carryingSomething;
	//[System.NonSerialized]
	public GameObject carriedObject;

	public float spitSpeed = 10;
	public float numSpitFrames = 60;

	Vector3 suckDirection;
	Vector3 spitDirection;
	public bool canShoot = false;

	bool blowing = false;

	void Start() {
		squishyController = transform.parent.GetComponent<SquishyController> ();
		graphics = transform.FindChild ("Graphics").gameObject;
	}

	// Update is called once per frame
	void Update () {
		if (carryingSomething) {
			carriedObject.transform.position = transform.position;
			
			if (squishyController.inputFire && canShoot) {
				Blow ();
			}
		}

		if (squishyController.inputFire && !carryingSomething) {
			suckDirection = squishyController.moveDirection;
			
			if (suckDirection == Vector3.zero) {
				suckDirection = transform.right * Mathf.Sign(transform.localScale.x);
			}
			
			transform.rotation = RotationHelper.LookAt2D (suckDirection);
		}

		if (squishyController.inputFireHold) {
			squishyController.canMove = false;

			if (!carryingSomething) {
				Suck();
			}

			canShoot = false;
		} else {
			squishyController.canMove = true;
			graphics.SetActive(false);
			canShoot = true;
		}
	}

	GameObject target;

	void Suck() {
		if (!blowing) {
			graphics.SetActive (true);
			List<RaycastHit2D> hits = graphics.GetComponent<RaycastCollision> ().hitTargets;

			float distanceToTarget = Mathf.Infinity;

			for (int i = 0; i < hits.Count; i++) {
				if (hits [i].distance < distanceToTarget && hits [i].collider != GetComponent<Collider2D> ()) {
					distanceToTarget = hits [i].distance;
					target = hits [i].collider.transform.root.gameObject;
				}
			}

			if (target != null) {
				Carry (target);
			}
		}
	}

	public void Carry(GameObject target) {
		if (!carryingSomething) {
			carryingSomething = true;
			carriedObject = target;
			carriedObject.SetActive (false);
			graphics.SetActive(false);
			target = null;
		}
		squishyController.moveSpeed *= carryModifier;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!carryingSomething && !blowing) {
			Carry(other.transform.root.gameObject);
		}
	}

	void Blow() {
		carryingSomething = false;
		carriedObject.SetActive (true);
		squishyController.moveSpeed = squishyController.defaultMoveSpeed;

		spitDirection = squishyController.moveDirection;
		if (spitDirection == Vector3.zero) {
			spitDirection = transform.right * Mathf.Sign(transform.localScale.x);
		}

		StartCoroutine ("BlowOutObject");
	}

	IEnumerator BlowOutObject() {
		GameObject spitObject = carriedObject;
		carriedObject = null;

		blowing = true;

		for (int i = 0; i < numSpitFrames; i++) {
			if (spitObject != null) {
				spitObject.transform.position = Vector3.MoveTowards(spitObject.transform.position, spitObject.transform.position + spitDirection * spitSpeed, Time.deltaTime * spitSpeed);
			}

			yield return new WaitForEndOfFrame();
		}

		blowing = false;
	}

}
