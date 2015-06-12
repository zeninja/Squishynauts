using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	public int damage = 3;

	void OnTriggerEnter2D(Collider2D other) {
		other.SendMessage("HandleDamage", damage, SendMessageOptions.DontRequireReceiver);
	}
}