using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public enum EnemyType { melee, projectile }
	public EnemyType enemyType = EnemyType.projectile;

	public float moveSpeed = 10f;

	public GameObject projectilePrefab;

	GameObject[] players;
	GameObject target;

	Quaternion aimDirection;

	public float spread = 0;

	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player");
		InvokeRepeating("StartBurst", 3, 3);

	}
	
	// Update is called once per frame
	void Update () {
//		transform.Translate(transform.right * Mathf.Sign(transform.localScale.x) * moveSpeed * Time.deltaTime);

		FindClosestTarget();

	}
	
	void ChangeDirection() {
		transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
		Invoke("ChangeDirection", 3);	
	}

	void FindClosestTarget() {
		float shortestDistanceToTarget = Mathf.Infinity;

		for(int i = 0; i < players.Length; i++) {
			if(players[i].GetComponent<PlayerController>().alive) {
				float distanceToPlayer = (transform.position - players[i].transform.position).magnitude;

				if(distanceToPlayer < shortestDistanceToTarget) {
					shortestDistanceToTarget = distanceToPlayer;
					target = players[i];
				}
			}
		}

		aimDirection = RotationHelper.RotateTowardsTarget2D(target.transform.position - transform.position);
		transform.FindChild("BeamGun").transform.rotation = aimDirection;
	}

	void Fire() {
		GameObject projectile = Instantiate(projectilePrefab) as GameObject;
		projectile.transform.position = transform.position;

		Vector3 newAim = aimDirection.eulerAngles;

		float randomSpread = Random.Range(-spread/2, spread/2);
		newAim = new Vector3(0, 0, newAim.z + randomSpread);
		projectile.transform.rotation = Quaternion.Euler(newAim);
	}

	void StartBurst() {
		StartCoroutine("FireBurst");
	}

	IEnumerator FireBurst() {
		for(int i = 0; i < 3; i++) {
			Fire();
			yield return new WaitForSeconds(.1f);
		}
		yield return new WaitForSeconds(3);
	}
}
