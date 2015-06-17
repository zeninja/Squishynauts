using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MineAttack : MonoBehaviour {

	public GameObject minePrefab;
	
	GameObject mine;

	List<GameObject> activeMines;

	float nextAttackTime;
	float attackDelay = 2;

	void Start() {
		activeMines = new List<GameObject> ();
	}

	void HandleAttack() {
		if(mine == null || !mine.activeInHierarchy) {
			if(Time.time > nextAttackTime) {
				mine = Instantiate(minePrefab) as GameObject;
				mine.transform.position = transform.position;

				activeMines.Add(mine);

				nextAttackTime = Time.time + attackDelay;
			}
		} else {
			for(int i = 0; i < activeMines.Count; i++) {
				activeMines[i].GetComponent<Mine>().Explode();
			}

			activeMines.Clear();
			//mine.GetComponent<Mine>().Explode();
		}
	}
}
