using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : Pathfinding2D {

	public enum EnemyState { still, attacking, moving }
	[System.NonSerialized]
	public EnemyState state  = EnemyState.moving;
	public int stateIndex = 0;
	
	bool alive = true;

	public bool usePathfinding;

	public bool playerInAttackRange;
	public bool playerInSeekRange;
	public bool targetIsVisible;

	public Stats stats;
	[System.Serializable]
	public class Stats 
	{
		[System.NonSerialized]
		public int maxHP;
		public int HP = 3;
		public int damageOnTouch = 3;
		public float seekRange = 15;
		public float attackRange = 10;
		public bool playerInRange = false;
		public Vector2 timeToSpendChasing;
		public Vector2 timeToSpendAttacking;
		public Vector2 timeToSpendPaused;
	}

	public Movement movement;
	[System.Serializable]
	public class Movement
	{
		public bool canMove;
		public enum MoveState { move, pause }
		public MoveState moveState = MoveState.pause;
		public float moveSpeed = 10f;
		[System.NonSerialized]
		public Vector3 moveDirection;
	}

	public Attack attack;
	[System.Serializable]
	public class Attack
	{
		public GameObject projectilePrefab;
		[System.NonSerialized]
		public GameObject target;
		public int damageByProjectile = 3;
		public int damageOnTouch = 2;
		public int spread = 3;
		public int numShots = 1;
		public float delayBetweenShots = .1f;
		public float delayBetweenBursts = 1f;
		public bool canAttackWhile = false;
		[System.NonSerialized]
		public Quaternion aimDirection;

	}
	
	GameObject[] players;
	GameObject target;

	bool attacking = false;

	public float spread = 0;

	void Awake() {
	}

	// Use this for initialization
	void Start () {
		stats.maxHP = stats.HP;
		players = GameObject.FindGameObjectsWithTag("Player");

		FindClosestTarget ();
		FindPath (transform.position, target.transform.position);
		//		StartCoroutine("Routine");
	}

	void OnDrawGizmos () {
		if (GameManager.drawGizmos) {
			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.DrawWireDisc (transform.position, Vector3.forward, stats.attackRange);

			UnityEditor.Handles.color = Color.green;
			UnityEditor.Handles.DrawWireDisc (transform.position, Vector3.forward, stats.seekRange);


			if(Path.Count > 0) {
				UnityEditor.Handles.color = Color.blue;
				UnityEditor.Handles.DrawWireDisc (Path[0], Vector3.forward, 1);
			}
		}
	}

	#region EnemyState
	void Update() {
		if(!alive) { return; }

		FindClosestTarget();

		HandleBehavior ();

		//HandleMovement ();
	}

	void HandleBehavior() {
		FindClosestTarget ();

		if (target != null) {
			if(playerInAttackRange) 
			{
				HandleAttack();
			}
			else if(playerInSeekRange) 
			{
				HandleMovement();
			}
		} else {
			Idle();
		}
	}

	void Idle() {

	}
	#endregion
	
	#region Movement
	void HandleMovement() {
		if(!usePathfinding) {
			FindMoveDirection();
			//Move();
		} else {
			for(int i = 0; i < Path.Count - 2; i++) {
				if(Path.Count >= 2) {
					Debug.DrawLine(Path[i], Path[i+1], Color.red);
				}
			}

			Move();
		}
	}

	void FindClosestTarget() {
		float shortestDistanceToTarget = Mathf.Infinity;

		for(int i = 0; i < players.Length; i++) {
			// Only look for living players
			if(players[i].GetComponent<PlayerController>().alive) {
				float distanceToPlayer = (transform.position - players[i].transform.position).magnitude;
				
				if(distanceToPlayer < shortestDistanceToTarget) {
					shortestDistanceToTarget = distanceToPlayer;
					target = players[i];
				}
			}
		}

		if (target != null) {
			float distanceToTarget = (target.transform.position - transform.position).magnitude;

			playerInAttackRange = distanceToTarget < stats.attackRange;
			playerInSeekRange   = distanceToTarget < stats.seekRange;

			if (distanceToTarget > stats.seekRange) {
				target = null;
			}
		}

		stats.playerInRange = target != null;
	
		if (target != null) {
			// Ignore the enemy layer so that we can raycast from inside the enemy collider. 
			// Also means enemy can see through other enemies
			int layer = ~(1 << LayerMask.NameToLayer ("Enemy"));
			RaycastHit2D visibleCheck = Physics2D.Raycast (transform.position, target.transform.position - transform.position, Mathf.Infinity, layer);
			
			if (visibleCheck) {
				targetIsVisible = visibleCheck.collider.transform.root.gameObject == target;
			}

			if (usePathfinding && Path.Count == 0) {
				//FindPath(transform.position, target.transform.position);
			}

			if(Path.Count > 0) {
				if(Vector3.Distance(target.transform.position, Path[Path.Count - 1]) > 0.4F && usePathfinding) {
					FindPath(transform.position, target.transform.position);
				}
			}

		//	FindPath(transform.position, target.transform.position);
		}
	}

	void FindMoveDirection() {
		// TODO: REMOVE. NOT USING THIS APPROACH ANY MORE SINCE MOVEMENT IS CURRENTLY PATHFINDING-BASED
		movement.moveDirection = (target.transform.position - transform.position).normalized;
	}

	void Move() {
		if (!usePathfinding) {
			transform.Translate (movement.moveDirection * movement.moveSpeed * Time.deltaTime);
			transform.localScale = new Vector3 (Mathf.Abs (transform.localScale.x) * Mathf.Sign (movement.moveDirection.x), transform.localScale.y, transform.localScale.z);
		} else {
			if (Path.Count > 0)
			{
				transform.position = Vector3.MoveTowards(transform.position, Path[0], Time.deltaTime * movement.moveSpeed);

				if (Vector3.Distance(transform.position, Path[0]) < 0.4F)
				{
					Path.RemoveAt(0);
				}
			}
		}
	}

	void UpdatePath() {
		FindPath (transform.position, target.transform.position);
	}
	#endregion

	#region Attack
	void HandleAttack() {
		if (!attacking) {
			StartCoroutine ("FireShots");
		}
	}

	IEnumerator FireShots() {
		attacking = true;
		for (int i = 0; i < attack.numShots; i++) {
			Fire();

			if(attack.numShots > 0) {
				yield return new WaitForSeconds(attack.delayBetweenShots);
			}
		}
		yield return new WaitForSeconds (attack.delayBetweenBursts);
	}

	void Fire() {
		// Instantiate the projectile and move it to the enemy's position (should be object pool eventually)
		GameObject projectile = Instantiate(attack.projectilePrefab) as GameObject;
		projectile.GetComponent<Projectile> ().owner = gameObject;
		projectile.transform.position = transform.position + attack.aimDirection.eulerAngles * 1.25f;

		// Add a random spread
		projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, attack.aimDirection.eulerAngles.z + Random.Range(-spread/2, spread/2)));
		projectile.GetComponent<Projectile>().damage = attack.damageByProjectile;
	}

	IEnumerator FireBurst() {
		for(int i = 0; i < 3; i++) {
			Fire();
			yield return new WaitForSeconds(.1f);
		}
		yield return new WaitForSeconds(3);
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Melee enemies deal damage on touch

		if (other.CompareTag ("Player")) {
			other.SendMessage ("HandleDamage", attack.damageOnTouch, SendMessageOptions.DontRequireReceiver);
		}
	}
	#endregion

	void HandleDamage(int dmg) {
		stats.HP = Mathf.Max (0, stats.HP - dmg);
		if (stats.HP == 0) {
			HandleDeath();
		}
	}

	void HandleDeath() {
		StopAllCoroutines ();
		transform.localScale = new Vector3 (transform.localScale.x, -1, 1);
		alive = false;
	}
}
