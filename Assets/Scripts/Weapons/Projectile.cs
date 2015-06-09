using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

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
		if(other.CompareTag("Reflective")) {
			Vector3.Reflect(Vector3.right, other.transform.root.GetComponent<PlayerController>().moveDirection);
		} else {
			other.SendMessage("HandleShot", damage, SendMessageOptions.DontRequireReceiver);

			Destroy(gameObject);
		}
	}
}
