using UnityEngine;
using System.Collections;

public class SwordAttack : MonoBehaviour {

	bool attacking = false;
	public GameObject attack;
	public float chargeDelay = 100;
	public float numFramesActive = 20;

	public float attackDelay = 1f;
	float nextAttackTime;

	
	void Start() {
		attack = transform.FindChild("Attack").gameObject;
	}
	
	void HandleAttack(Vector3 direction) {
		if (Time.time > nextAttackTime) {
			StartCoroutine ("Attack");
			nextAttackTime = Time.time + attackDelay;
		}
	}

	IEnumerator Attack() {
		if (!attacking) {
			attacking = true;

			AudioManager.GetInstance ().PlaySound (AudioManager.GetInstance ().arthurGrunt);
			GetComponent<PlayerController>().ReduceMoveSpeed(); 

			yield return new WaitForSeconds (chargeDelay / 60);

			attack.SetActive (true);
			GetComponent<PlayerController> ().canMove = false;
			yield return new WaitForSeconds (numFramesActive / 60);
			attack.SetActive (false);
			GetComponent<PlayerController> ().canMove = true;
			GetComponent<PlayerController>().RestoreMoveSpeed();
			attacking = false;
		}
	}
}