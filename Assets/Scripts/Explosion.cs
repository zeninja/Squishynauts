using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
	public float explosionDuration = .25f;
	
	void Start() {
		Invoke("HandleDeath", explosionDuration);
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log(other.gameObject.name);
		other.SendMessage("HandleDeath", SendMessageOptions.DontRequireReceiver);
	}
	
	void HandleDeath() {
		Destroy(gameObject);
	}
}
