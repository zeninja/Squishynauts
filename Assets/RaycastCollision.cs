using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaycastCollision : MonoBehaviour {

	public int numRays = 10;

	float x, y;
	float xPos, yPos;
	float xSize, ySize;

	Vector2 origin;
	Vector2 offset;

	[System.NonSerialized]
	public List<RaycastHit2D> hitTargets;
	
	// Use this for initialization
	void Start () {
		hitTargets = new List<RaycastHit2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		xSize = GetComponent<BoxCollider2D> ().size.x;
		ySize = GetComponent<BoxCollider2D> ().size.y;

		CheckForCollisions ();
	}

	void CheckForCollisions() {
		hitTargets.Clear();

		xPos = GetComponent<BoxCollider2D> ().transform.position.x - xSize / 2;
		yPos = GetComponent<BoxCollider2D> ().transform.position.y + ySize / 2;

		for (int i = 0; i < numRays; i++) {
			origin = new Vector3(xPos, yPos);

			float ySpacing = (i - numRays + 1) * (ySize / (numRays - 1) );

			offset = new Vector3(0, ySpacing);

//			Debug.DrawRay(origin + offset, transform.right * xSize);

			RaycastHit2D hit = Physics2D.Raycast(origin + offset, transform.right, xSize);
			if (hit && hit.collider != GetComponent<Collider2D>()) {
				Debug.Log(hit.collider.transform.gameObject.name);
				hitTargets.Add(hit);
			}
		}
	}
}
