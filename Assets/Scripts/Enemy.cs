using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	GameObject[] players;
	GameObject target;

	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FindTarget() {
		float distanceToPlayer = Mathf.Infinity;

		for(int i = 0; i < players.Length; i++) {
			if (players[i].GetComponent<PlayerController>().alive) {
				float checkDistace = (players[i].transform.position - transform.position).magnitude;

				if(checkDistace < distanceToPlayer) {
					distanceToPlayer = checkDistace;
					target = players[i];
				}
			}
		}
	}
}
