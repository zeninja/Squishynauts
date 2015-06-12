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
		transform.FindChild("Shield").rotation = RotationHelper.RotateTowardsTarget2D(player.moveDirection, transform.localScale.x);//Quaternion.LookRotation(player.moveDirection); //Quaternion.SetFromToRotation(transform.forward, player.moveDirection);
		
		StartCoroutine("Shield", direction);
	}
	
	IEnumerator Shield() {
		shield.SetActive(true);
		for (int i = 0; i < shield.transform.childCount; i++) {
			shield.transform.GetChild(i).rotation = Quaternion.Euler(Vector3.zero);
		}

		GetComponent<PlayerController>().canMove = false;
		yield return new WaitForSeconds((float)numFramesActive/60);
		shield.SetActive(false);
		GetComponent<PlayerController>().canMove = true;
	}
}