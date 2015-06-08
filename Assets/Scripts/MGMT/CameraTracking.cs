using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour {

//	public GameObject player;
	
	public GameObject[] players;
	
//	float hBound;
//	float vBound;

	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player");
		
//		hBound = camera.orthographicSize;
//		vBound = camera.orthographicSize/2;
	}
	
	// Update is called once per frame
	void Update () {
		FindTargetPosition();
	}
	
	void FindTargetPosition() {
		Vector3 targetPos = Vector3.zero;
		
		for(int i = 0; i < players.Length; i++) {
			targetPos += players[i].transform.position;
		}
		
		targetPos /= players.Length;
	
		transform.position = targetPos + new Vector3(0, 0, -10);
	}
}
