using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuckNBlowAttack : MonoBehaviour {

	enum AttackState {neutral, sucking, carrying, blowing}
	AttackState state = AttackState.neutral;

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
	
	bool inputFire;
	bool inputFireHold;

	void Start() {
		squishyController = transform.parent.GetComponent<SquishyController> ();
		graphics = transform.FindChild ("Graphics").gameObject;
	}

	// Update is called once per frame
	void Update () {
		ManageInput ();
//
//		if (carryingSomething) {
//			carriedObject.transform.position = transform.position;
//			
//			if (squishyController.inputFire && canShoot) {
//				Blow ();
//			}
//		}
//
//		if (squishyController.inputFire && !carryingSomething) {
//			suckDirection = squishyController.moveDirection;
//			
//			if (suckDirection == Vector3.zero) {
//				suckDirection = transform.right * Mathf.Sign(transform.localScale.x);
//			}
//			
//			transform.rotation = RotationHelper.LookAt2D (suckDirection);
//		}
//
//		if (squishyController.inputFireHold) {
//			squishyController.canMove = false;
//
//			if (!carryingSomething) {
//				Suck();
//			}
//
//			canShoot = false;
//		} else {
//			squishyController.canMove = true;
//			graphics.SetActive(false);
//			canShoot = true;
//		}
	}

	void ManageInput() {
		inputFire	  = squishyController.inputFire;
		inputFireHold = squishyController.inputFireHold;

		switch (state) {
			case AttackState.neutral:
				ManageNeutralInput();
				break;
			case AttackState.sucking:
				ManageSuckingInput();
				break;
			case AttackState.carrying:
				ManageCarryingInput();
				break;
			case AttackState.blowing:
				ManageBlowingInput();
				break;
		}
	}

	void SetState(AttackState newState) {
		state = newState;
	}

	void ManageNeutralInput() {
		graphics.SetActive (false);
		squishyController.canMove = true;

		if (inputFire) {
			suckDirection = squishyController.moveDirection;
			SetState(AttackState.sucking);
		}
	}

	void ManageSuckingInput() {
		if (inputFireHold) {
			Suck ();
		} else {
			SetState(AttackState.neutral);
		}
	}

	void ManageCarryingInput() {
		if (inputFire) {
			SetState(AttackState.blowing);
			Blow();
		}
	}

	void ManageBlowingInput() {
		// Not being used at the moment, but may be useful if we decide to add more layers to the blowing
	}

	void Suck() {
		graphics.SetActive (true);
		squishyController.canMove = false;
		transform.rotation = RotationHelper.LookAt2D (suckDirection);

		List<RaycastHit2D> hits = graphics.GetComponent<RaycastCollision> ().hitTargets;

		float distanceToTarget = Mathf.Infinity;
		GameObject target = null;

		for (int i = 0; i < hits.Count; i++) {
			if (hits [i].distance < distanceToTarget && hits [i].collider != GetComponent<Collider2D> ()) {
				distanceToTarget = hits [i].distance;
				target = hits [i].collider.transform.root.gameObject;
			}
		}

		if (target != null) {
			SetState(AttackState.carrying);
			Carry (target);
		}
	}

	public void Carry(GameObject target) {
		squishyController.canMove = true;
		squishyController.moveSpeed *= carryModifier;
		carriedObject = target;
		carriedObject.SetActive (false);
		graphics.SetActive(false);
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

	//TODO: THIS SEEMS REALLY FRAGILE SINCE IT MIGHT GET FUCKED UP IF SOMEONE KILLS LIPS WHILE THE SPIT THING IS GETTING SPIT
	// SHOULD TRY TO FIGURE OUT AN ALTERNATIVE APPROACH (PROBABLY JUST CHANGE THE DIRECTION OF THE MOTOR FOR A CERTAIN AMOUNT OF TIME)
	IEnumerator BlowOutObject() {
//		GameObject spitObject = carriedObject;
		carriedObject.SetActive (true);

		for (int i = 0; i < numSpitFrames; i++) {
			if (carriedObject != null) {
				carriedObject.transform.position = Vector3.MoveTowards(carriedObject.transform.position, carriedObject.transform.position + spitDirection * spitSpeed, Time.deltaTime * spitSpeed);
			}

//			if (spitObject != null) {
//				spitObject.transform.position = Vector3.MoveTowards(spitObject.transform.position, spitObject.transform.position + spitDirection * spitSpeed, Time.deltaTime * spitSpeed);
//			}

			yield return new WaitForEndOfFrame();
		}
		carriedObject = null;

		SetState (AttackState.neutral);
	}

}
