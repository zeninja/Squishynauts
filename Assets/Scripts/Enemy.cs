using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float moveSpeed = 10;

	GameObject[] players;
	GameObject target;

	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Move();
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

	void Move() {
		transform.Translate(transform.right * transform.localScale.x * moveSpeed * Time.deltaTime);
	}
}
