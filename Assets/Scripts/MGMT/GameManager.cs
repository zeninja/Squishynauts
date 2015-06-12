using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	bool isPaused = false;
	public static bool gameOver = false;

	public static bool drawGizmos = true;

	GameObject[] players;

	void Awake() {

	}

	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			PauseGame();
		}

		if (Input.GetKeyDown (KeyCode.G)) {
			drawGizmos = !drawGizmos;
		}
	}

	void OnGUI() {
		if (gameOver) {
			GUI.Label (new Rect (0, 0, Screen.width, Screen.height), "ALL SQUISHIES DEAD =(");
		}
	}

	void PauseGame() {
		isPaused = !isPaused;
		Time.timeScale = isPaused ? 0 : 1.0f;
	}

	public void CheckGameOver() {
		int numPlayersDead = 0;

		for(int i = 0; i < players.Length; i++) {
			if(!players[i].GetComponent<PlayerController>().alive) {
				numPlayersDead++;
			}
		}

		if (numPlayersDead == players.Length) {
			gameOver = true;
		}


	}
}
