using UnityEngine;
using System.Collections;

public class MineAttack : MonoBehaviour {

	public GameObject minePrefab;
	
	GameObject mine;
	
	void HandleAttack() {
		if(mine == null) {
			mine = Instantiate(minePrefab) as GameObject;
			mine.transform.position = transform.position;
		} else {
			mine.GetComponent<Mine>().Explode();
		}
	}
}
