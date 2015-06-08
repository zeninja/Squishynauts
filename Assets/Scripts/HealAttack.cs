using UnityEngine;
using System.Collections;

public class HealAttack : MonoBehaviour {

	public int numHealthKitsToDrop = 4;

	public GameObject healthKitPrefab;

	public float beamDistance = 3;
	int layer;

	void Start() {
		layer = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Obstacle");
	}

	void HandleAttack() {
//		for(int i = 0; i < numHealthKitsToDrop; i++) {
//			GameObject healthKit = Instantiate(healthKitPrefab) as GameObject;
//			healthKit.transform.position = transform.position;
//			
//			Vector3 medkitMoveDirection = Vector3.zero;
//			
//			if(i == 0) {
//				medkitMoveDirection = new Vector3(1, 1, 0);
//			}
//			
//			if(i == 1) {
//				medkitMoveDirection = new Vector3(1, -1, 0);
//			}
//			
//			if(i == 2) {
//				medkitMoveDirection = new Vector3(-1,-1, 0);
//			}
//			
//			if(i == 3) {
//				medkitMoveDirection = new Vector3(-1, 1, 0);
//			}
//			
//			healthKit.GetComponent<Medkit>().moveDirection = medkitMoveDirection;
//		
//		}

	}

	void Update() {
		if(GetComponent<PlayerController>().inputFireHold) {
			//GetComponent<PlayerController>().canMove = false;

			Vector3 aimDirection = GetComponent<PlayerController>().moveDirection;
			if (aimDirection == Vector3.zero) {
				aimDirection = new Vector3(transform.localScale.x, 0, 0);
			}

			RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, beamDistance, layer);

			transform.FindChild("Medigun").gameObject.SetActive(true);
			//transform.FindChild("Medigun").transform.rotation.SetFromToRotation(transform.position, transform.position + GetComponent<PlayerController>().moveDirection); //(transform.position + GetComponent<PlayerController>().moveDirection * 10);
			//transform.FindChild("Medigun").transform.LookAt(transform.position + GetComponent<PlayerController>().moveDirection);


			float x = GetComponent<PlayerController>().moveDirection.x;
			float y = GetComponent<PlayerController>().moveDirection.y;

			float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
			transform.FindChild("Medigun").transform.rotation = Quaternion.Euler(0f, 0f, angle * transform.localScale.x);

			float beamLength = beamDistance;

			if (hit) {
				if(hit.collider != null && hit.collider != collider) {
					Debug.Log(hit.collider.name);

					hit.collider.SendMessage("HandleMedicBeam", SendMessageOptions.DontRequireReceiver);
					beamLength = hit.point.x - transform.position.x;
				}
			}

			int scrollSpeed = 1;
			
			float offset = Time.time * scrollSpeed * -1;
			transform.FindChild("Medigun").renderer.material.SetTextureOffset("_MainTex", new Vector2(offset * transform.localScale.x, 0));
			transform.FindChild("Medigun").renderer.material.mainTextureScale = new Vector2(beamLength, 1);
			transform.FindChild("Medigun").localScale = new Vector3(beamLength, .5f, 1);
			transform.FindChild("Medigun").transform.position = transform.position + aimDirection.normalized * beamLength/2;

		} else {
			GetComponent<PlayerController>().canMove = true;
			transform.FindChild("Medigun").gameObject.SetActive(false);

		}
	}
}