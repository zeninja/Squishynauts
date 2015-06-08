using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {
		
	public GameObject explosionPrefab;
	
	public void Explode() {
		GameObject explosion = Instantiate(explosionPrefab) as GameObject;
		explosion.transform.position = transform.position;
		HandleDeath();
	}
	
	void HandleDeath() {
		Destroy(gameObject);
	}
}
