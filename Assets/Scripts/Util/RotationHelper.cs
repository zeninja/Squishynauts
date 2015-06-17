using UnityEngine;
using System.Collections;

public class RotationHelper : MonoBehaviour {

	public static Quaternion LookAt2D(Vector3 aimTarget) {

		float x = aimTarget.x;
		float y = aimTarget.y;
		
		float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
		return Quaternion.Euler(0f, 0f, angle);
	}
}
