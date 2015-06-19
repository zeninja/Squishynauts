using UnityEngine;
using System.Collections;

public class SquishyController : MonoBehaviour {

	public bool invincible = true;
	public int playerNum;
	
	public int hp = 10;
	int maxHealth = 10;

	public float moveSpeed;
	[System.NonSerialized]
	public float defaultMoveSpeed;
	
	float inputHorizontal;
	float inputVertical;
	[System.NonSerialized]
	public bool inputFire;
	//[System.NonSerialized]
	public bool inputFireHold;

	public bool canMove = true;

	[System.NonSerialized]
	public Vector3 moveDirection;
	

	[System.NonSerialized]
	public bool alive = true;

	public GameObject abilityPrefab;
	GameObject myAbility;

	void Awake() {
		// TODO: COULD PROBABLY CHANGE THIS TO SOME KIND OF HASHTABLE (?) THAT JUST CONTAINS THE SCRIPT TO BE ATTACHED AND THE SPRITE??
		myAbility = Instantiate (abilityPrefab) as GameObject;
		myAbility.transform.parent = transform;
		myAbility.transform.position = transform.position;
//		myAbility.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		maxHealth = hp;
		defaultMoveSpeed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		ManageInput();
	}
	
	void ManageInput() {
		if(!alive) { return; }

		inputHorizontal = Input.GetAxisRaw("P" + playerNum + "_Horizontal");
		inputVertical   = Input.GetAxisRaw("P" + playerNum + "_Vertical");
		inputFire	    = Input.GetButtonDown("P" + playerNum + "_Fire");
		inputFireHold   = Input.GetButton("P" + playerNum + "_Fire");
	
//		if (inputHorizontal != 0) {
//			moveDirection.x = inputHorizontal * moveSpeed;
//		} else {
//			moveDirection.y = 0;
//		}
//		
//		if (inputVertical != 0) {
//			moveDirection.y = inputVertical;
//		} else {
//			moveDirection.y = 0;
//		}

		if(inputHorizontal != 0) {
			//moveDirection.x = Mathf.Sign(inputHorizontal);
			moveDirection.x = inputHorizontal;
		} else {
			moveDirection.x = 0;
		}

		if(inputVertical != 0) {
//			moveDirection.y = Mathf.Sign(inputVertical);
			moveDirection.y = inputVertical;
		} else {
			moveDirection.y = 0;
		}

		if (canMove) {
			transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
			
			if (inputHorizontal != 0) {
			//	transform.FindChild("Graphics").localScale = new Vector3(Mathf.Sign(moveDirection.x), 1, 1);
//				transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(moveDirection.x), transform.localScale.y, transform.localScale.z);
			}
		}
	
		if (inputFire) {
			SendMessage("HandleAttack", moveDirection, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void Heal(int hpToRestore) {
		hp += hpToRestore;
	}

	public void RestoreMoveSpeed() {
		moveSpeed = defaultMoveSpeed;
	}

	public void ReduceMoveSpeed() {
		iTween.ValueTo(gameObject, iTween.Hash("from", defaultMoveSpeed, "to", 0, "time", GetComponent<SwordAttack>().chargeDelay/60, "onupdate", "UpdateMoveSpeed"));
	}

	void UpdateMoveSpeed(float newSpeed) {
		moveSpeed = newSpeed;
	}

	void HandleDamage(int dmg) {
		//Handle damage
		hp = Mathf.Max (0, hp - dmg);

		//Handle healing
		hp = Mathf.Min (hp, maxHealth);

		if (hp == 0) {
			HandleDeath();
		}
	}

	void HandleDeath() {
		if(!invincible) {
			alive = false;
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
			GetComponent<Collider2D>().enabled = false;
			GameObject.Find("GameManager").GetComponent<GameManager>().CheckGameOver();
		}
	}
}
