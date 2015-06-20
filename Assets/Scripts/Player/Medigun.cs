using UnityEngine;
using System.Collections;

public class Medigun : MonoBehaviour {

	SquishyController squishyController;

	public float beamDistance = 3;
	public float healRate = 1;
	public float healThreshold = 1;
	float healValue = 0;

	GameObject healTarget;
	GameObject oldHealTarget;

	int layer;

	GameObject graphics;

	void Start() {
		squishyController = transform.parent.GetComponent<SquishyController> ();
		layer = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Obstacle");
		graphics = transform.FindChild ("Graphics").gameObject;
	}
	
	void Update() {
		if(squishyController.inputFireHold) {
//			squishyController.canMove = false;

			Vector3 aimDirection = squishyController.moveDirection;

			if (aimDirection == Vector3.zero) {
				aimDirection = transform.right;//new Vector3(transform.localScale.x, 0, 0);
			}

			graphics.SetActive(true);
			transform.rotation = RotationHelper.LookAt2D(squishyController.moveDirection);

			//RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, aimDirection, beamDistance, layer);
			float beamLength = beamDistance;
			float distanceToTarget = Mathf.Infinity;
			RaycastHit2D[] beamTargets = Physics2D.RaycastAll(transform.position, aimDirection, beamDistance, layer);
			healTarget = null;

			for(int i = 0; i < beamTargets.Length; i++) {
				if (beamTargets[i].distance < distanceToTarget && beamTargets[i].collider.gameObject != transform.parent.gameObject) {
					distanceToTarget = beamTargets[i].distance;
					healTarget = beamTargets[i].transform.root.gameObject;
				}
			}

			if (oldHealTarget == healTarget && healTarget != null) {
				healValue += Time.deltaTime;

				if (healValue >= healThreshold) {
					healTarget.SendMessage("HandleDamage", -healRate, SendMessageOptions.DontRequireReceiver);
					healValue = 0;
				}
			} else {
				healValue = 0;
			}

			oldHealTarget = healTarget;

			int scrollSpeed = 1;
			
			float offset = Time.time * scrollSpeed * -1;

			graphics.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset * transform.localScale.x, 0));
			graphics.GetComponent<Renderer>().material.mainTextureScale = new Vector2(beamLength, 1);

			graphics.transform.localScale = new Vector3(beamLength * transform.localScale.x, .5f, 1);

//			transform.position = transform.position + aimDirection.normalized * beamLength/2;
		} else {
//			squishyController.canMove = true;
			graphics.SetActive(false);
		}
	}
}