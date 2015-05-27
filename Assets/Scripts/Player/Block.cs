using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	public bool destructible = false;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void HandleDeath() {
		if (destructible) {
			Destroy(gameObject);
		}
	}
}
