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
		StartCoroutine("Shield", direction);
	}
	
	IEnumerator Shield() {
		shield.transform.rotation =  Quaternion.Euler( new Vector3(0, 0, Vector3.Angle(transform.up, player.moveDirection.normalized)));
		for(int i = 0; i < shield.transform.childCount; i++) {
			shield.transform.GetChild(i).transform.rotation = Quaternion.Euler(Vector3.zero);
		}
	
		shield.SetActive(true);
		GetComponent<PlayerController>().canMove = false;
		yield return new WaitForSeconds(1);
		shield.SetActive(false);
		GetComponent<PlayerController>().canMove = true;
	}
}