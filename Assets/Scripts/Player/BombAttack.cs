using UnityEngine;
using System.Collections;

public class BombAttack : MonoBehaviour {

	public GameObject bombPrefab;
	
	void HandleAttack() {
		GameObject bomb = Instantiate(bombPrefab) as GameObject;
		bomb.transform.position = transform.position;
	}
}