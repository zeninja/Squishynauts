using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	public float delayBeforeExplosion = 1;
	public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
		Invoke("HandleDeath", delayBeforeExplosion);
	}
	
	void Explode() {
		GameObject explosion = Instantiate(explosionPrefab) as GameObject;
		explosion.transform.position = transform.position;
	}
	
	void HandleDeath() {
		CancelInvoke("HandleDeath");
		Explode();
		Destroy(gameObject);
	}
}
