using UnityEngine;
using System.Collections;

public class HealAttack : MonoBehaviour {

	public int numHealthKitsToDrop = 4;

	public GameObject healthKitPrefab;

	void HandleAttack() {
		for(int i = 0; i < numHealthKitsToDrop; i++) {
			GameObject healthKit = Instantiate(healthKitPrefab) as GameObject;
			healthKit.transform.position = transform.position;
			
			Vector3 medkitMoveDirection = Vector3.zero;
			
			if(i == 0) {
				medkitMoveDirection = new Vector3(1, 1, 0);
			}
			
			if(i == 1) {
				medkitMoveDirection = new Vector3(1, -1, 0);
			}
			
			if(i == 2) {
				medkitMoveDirection = new Vector3(-1,-1, 0);
			}
			
			if(i == 3) {
				medkitMoveDirection = new Vector3(-1, 1, 0);
			}
			
			healthKit.GetComponent<Medkit>().moveDirection = medkitMoveDirection;
		}
	}
}