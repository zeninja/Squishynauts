using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GizmoManager : MonoBehaviour {
	public bool drawGizmos;

	void Update() {
		GameManager.drawGizmos = drawGizmos;
	}
}
