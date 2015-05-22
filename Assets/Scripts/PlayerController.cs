using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	int hp = 10;

	public int playerNum;

	public float moveSpeed;
	
	float inputHorizontal;
	float inputVertical;
	bool inputFire;

	public bool canMove = true;
	
	public Vector3 moveDirection;
	
	public bool invincible = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ManageInput();
	}
	
	void ManageInput() {
		moveDirection = Vector3.zero;
		
		inputHorizontal = Input.GetAxisRaw("P" + playerNum + "_Horizontal");
		inputVertical = Input.GetAxisRaw("P" + playerNum + "_Vertical");
		inputFire = Input.GetButtonDown("P" + playerNum + "_Fire");
	
		if (inputHorizontal != 0) {
			moveDirection.x += inputHorizontal * moveSpeed * Time.deltaTime;
		}
		
		if (inputVertical != 0) {
			moveDirection.y += inputVertical * moveSpeed * Time.deltaTime;
		}
		
		if (canMove) {
			transform.Translate(moveDirection);
			
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
	
	void HandleDeath() {
		if(!invincible) {
			Destroy(gameObject);
		}
	}
}
