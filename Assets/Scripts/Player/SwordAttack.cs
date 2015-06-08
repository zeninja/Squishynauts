using UnityEngine;
using System.Collections;

public class SwordAttack : MonoBehaviour {

	public GameObject attack;
	public float chargeDelay = 100;
	public float numFramesActive = 20;
	
	void Start() {
		attack = transform.FindChild("Attack").gameObject;
	}
	
	void HandleAttack(Vector3 direction) {
		StartCoroutine("Attack");
	}

	IEnumerator Attack() {
		AudioManager.GetInstance().PlaySound(AudioManager.GetInstance().arthurGrunt);
		yield return new WaitForSeconds(chargeDelay/60);

		attack.SetActive(true);
		GetComponent<PlayerController>().canMove = true;
		yield return new WaitForSeconds(numFramesActive/60);
		attack.SetActive(false);
		GetComponent<PlayerController>().canMove = false;
	}
}