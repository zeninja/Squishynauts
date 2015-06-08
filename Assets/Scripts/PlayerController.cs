using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public int hp = 10;
	int maxHealth = 10;
	int healRate = 1;

	public int playerNum;

	public float moveSpeed;
	
	float inputHorizontal;
	float inputVertical;
	bool inputFire;
	[System.NonSerialized]
	public bool inputFireHold;

	public bool canMove = true;
	
	public Vector3 moveDirection;
	
	public bool invincible = true;

	[System.NonSerialized]
	public bool alive = true;

	// Use this for initialization
	void Start () {
		maxHealth = hp;
	}
	
	// Update is called once per frame
	void Update () {
		ManageInput();
	}
	
	void ManageInput() {
		inputHorizontal = Input.GetAxisRaw("P" + playerNum + "_Horizontal");
		inputVertical = Input.GetAxisRaw("P" + playerNum + "_Vertical");
		inputFire = Input.GetButtonDown("P" + playerNum + "_Fire");
		inputFireHold = Input.GetButton("P" + playerNum + "_Fire");
	
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
				transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(moveDirection.x), transform.localScale.y, transform.localScale.z);
			}
		}
	
		if (inputFire) {
			SendMessage("HandleAttack", moveDirection);
		}
	}
	
	void Heal(int hpToRestore) {
		hp += hpToRestore;
	}

	void HandleMedicBeam() {
		Debug.Log(gameObject.name + " being healed");
		hp += healRate;
		hp = Mathf.Min(hp, maxHealth);
	}
	
	void HandleDeath() {
		if(!invincible) {
			Destroy(gameObject);
		}
	}
}
