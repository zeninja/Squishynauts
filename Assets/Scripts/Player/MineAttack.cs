using UnityEngine;
using System.Collections;

public class MineAttack : MonoBehaviour {

	public GameObject minePrefab;
	
	GameObject mine;

	float nextAttackTime;
	float attackDelay = 2;
	
	void HandleAttack() {
		if(mine == null) {
			if(Time.time > nextAttackTime) {
				mine = Instantiate(minePrefab) as GameObject;
				mine.transform.position = transform.position;
				nextAttackTime = Time.time + attackDelay;
			}
		} else {
			mine.GetComponent<Mine>().Explode();
		}
	}
}
