using UnityEngine;
using System.Collections;

public class Medkit : MonoBehaviour {

	public float hpRestored = 2;

	[System.NonSerialized]
	public Vector2 moveDirection;
	
	public float spreadForce = 100;

	// Use this for initialization
	void Start () {
		spreadForce += Random.Range(-40, 40);
		rigidbody2D.AddForce(moveDirection * spreadForce);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player") && other.gameObject.name != "Doctor") {
			other.SendMessage("Heal", hpRestored, SendMessageOptions.DontRequireReceiver);
			Destroy(gameObject);
		}
	}
}
