using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public GameObject owner;
	public float moveSpeed = 10;
	public float damage = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.root.gameObject != owner) {
			Debug.Log(other.gameObject.name);

			if (other.CompareTag("Reflective")) {
				Debug.Log("Hit something reflective");
				Vector3.Reflect(transform.rotation.eulerAngles, other.transform.forward);
			} else {
				other.SendMessage ("HandleDamage", damage, SendMessageOptions.DontRequireReceiver);
				Destroy (gameObject);
			}
		}
	}
}
