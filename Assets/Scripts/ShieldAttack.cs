using UnityEngine;
using System.Collections;

public class ShieldAttack : MonoBehaviour {

	GameObject shield;
	public int numFramesActive = 30;
	
	PlayerController player;
	
	void Start() {
		shield = transform.FindChild("Shield").gameObject;
		player = GetComponent<PlayerController>();
	}

	void HandleAttack(Vector3 direction) {
		//transform.rotation = Quaternion.SetFromToRotation(transform.forward, player.moveDirection);
		
		StartCoroutine("Shield", direction);
	}
	
	IEnumerator Shield() {
		shield.SetActive(true);
		GetComponent<PlayerController>().canMove = false;
		yield return new WaitForSeconds(1);
		shield.SetActive(false);
		GetComponent<PlayerController>().canMove = true;
	}
}