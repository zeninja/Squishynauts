using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
	public float numFramesActive = 15;
	public int damage = 3;
	public float explosionForce;
	
	void Start() {
		Invoke("HandleDeath", numFramesActive/60);
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		other.SendMessage ("HandleExplosion", SendMessageOptions.DontRequireReceiver);
		other.SendMessage("HandleDamage", damage, SendMessageOptions.DontRequireReceiver);
	}
	
	void HandleDeath() {
		Destroy(gameObject);
	}
}
