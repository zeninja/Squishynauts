using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	bool isPaused = false;
	
	public static bool drawGizmos = true;

	void Awake() {

	}

	// Use this for initialization
	void Start () {
	
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

	void PauseGame() {
		isPaused = !isPaused;
		Time.timeScale = isPaused ? 0 : 1.0f;
	}
}
