using UnityEngine;
using System.Collections;

public class SuckNBlowAttack : MonoBehaviour {

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
	
	// Update is called once per frame
	void Update () {
		if (carryingSomething) {

			carriedObject.transform.position = transform.position;
			
			if (GetComponent<PlayerController>().inputFire && canShoot) {
				Blow ();
			}
		}

		if (GetComponent<PlayerController> ().inputFire && !carryingSomething) {
			suckDirection = GetComponent<PlayerController>().moveDirection;
			
			if (suckDirection == Vector3.zero) {
				suckDirection = transform.right * Mathf.Sign(transform.localScale.x);
			}
			
			transform.FindChild ("Attack").transform.rotation = RotationHelper.LookAt2D (suckDirection);
		}

		if (GetComponent<PlayerController>().inputFireHold) {
			GetComponent<PlayerController>().canMove = false;

			if (!carryingSomething) {
				Suck();
			}

			canShoot = false;
		} else {
			GetComponent<PlayerController> ().canMove = true;
			//transform.FindChild("Attack").gameObject.SetActive(false);

			canShoot = true;
		}
	}

	public int numRays = 10;

	void Suck() {
		Vector2 raycastOrigin;

		for (int i = 0; i < numRays; i++) {
			raycastOrigin = (Vector2)transform.position + new Vector2(0, (float)(i - numRays/2)/numRays);
			Physics2D.Raycast(raycastOrigin, transform.right, 4);
			Debug.DrawRay(raycastOrigin, transform.right);
		}
	}

	void Carry(GameObject target) {
		if (!carryingSomething) {
			carryingSomething = true;
			carriedObject = target;
			carriedObject.SetActive (false);
			transform.FindChild("Attack").gameObject.SetActive(false);
		}
		GetComponent<PlayerController> ().moveSpeed *= carryModifier;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!carryingSomething && !blowing) {
			Carry(other.transform.root.gameObject);
		}
	}

	void Blow() {
		carryingSomething = false;
		carriedObject.SetActive (true);
		GetComponent<PlayerController> ().moveSpeed = GetComponent<PlayerController> ().defaultMoveSpeed;

		spitDirection = GetComponent<PlayerController> ().moveDirection;
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
