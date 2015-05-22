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
		for(int i = 0; i < numFramesActive; i++) {
			yield return new WaitForSeconds(1/60);
		}
		
		attack.SetActive(false);
	}
}
