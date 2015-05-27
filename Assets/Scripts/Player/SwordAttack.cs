using UnityEngine;
using System.Collections;

public class SwordAttack : MonoBehaviour {

	public GameObject attack;
	
	void Start() {
		attack = transform.FindChild("Attack").gameObject;
	}
	
	void HandleAttack(Vector3 direction) {
		StartCoroutine("Attack");
	}

	IEnumerator Attack() {
		int numFramesActive = 7;
		attack.SetActive(true);
		
		GetComponent<PlayerController>().canMove = false;
		
		yield return new WaitForSeconds(numFramesActive/60);
		
		attack.SetActive(false);
		GetComponent<PlayerController>().canMove = true;
	}
}
