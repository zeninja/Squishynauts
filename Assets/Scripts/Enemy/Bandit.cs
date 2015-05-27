using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SearchForPlayers();
	}
	
	void SearchForPlayers() {
		int numRays = 10;
		for(int i = 0; i < numRays; i++) {
			RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 10);
			
			if (hit) {
				if (hit.transform.root.CompareTag("Player")) {
				
				}
			}
		}
	}
}
