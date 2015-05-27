using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float moveSpeed = 10f;

	// Use this for initialization
	void Start () {
		Invoke("ChangeDirection", 3);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(transform.right * Mathf.Sign(transform.localScale.x) * moveSpeed * Time.deltaTime);
	}
	
	void ChangeDirection() {
		transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
		Invoke("ChangeDirection", 3);
		
	}
}
