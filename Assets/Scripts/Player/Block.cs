using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	public bool destructible = false;


	// THIS FUNCTION COMES FROM THE EXPLOSION SCRIPT.
	// OTHER GAMEOBJECTS ALSO SEND THE SAME MESSAGE (SWORD, EVERYTHING?)
	// THIS SHOULD MAYBE BE CHANGED??
	void HandleDamage(int dmg) {
		if (destructible) {
			Destroy(gameObject);
			Pathfinder2D.Instance.Create2DMap();
		}
	}
}
