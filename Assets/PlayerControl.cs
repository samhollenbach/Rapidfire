using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	//Will have to play with these numbers
	public float maxSpeed = 50f;
	public float moveForce = 900f;
	public float jumpForce = 5000f;

	//Conditions for direction faced and ability to jump
	bool facingRight = true;
	bool jump = false;

	//Conditions for checking whether or not the character is grounded
	public Transform groundCheck;
	private bool grounded = false;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	//Links the character to animations
	private Animator anim;

	// Use this for initialization
	void Start () {
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}


	// Update is called once per frame
	void Update() {
		//Updates to check for the player's grounded status
		//Grounded if linecast to groundcheck position hits anything on the ground
		//grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground"));
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		if (Input.GetButtonDown ("Jump") && grounded) {
			jump = true;
		}
//		if(Input.GetKeyDown(KeyCode.Space) && grounded) {
//			jump = true;
//		}
	}
	

	void FixedUpdate () {
		float moveH = Input.GetAxisRaw ("Horizontal");

		if (grounded && moveH == 0) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
		}

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		//anim.SetFloat("Speed", Mathf.Abs(moveH));

		//If horizontal velocity is < maxspeed
		if(moveH * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			//increase horizontal velocity by adding a force to player
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveH * moveForce);

		// If horizontal velocity > maxspeed
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			//set horizontal velocity to maxspeed
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		//If player is moving RIGHT but facing left, flip sprite
		if(moveH > 0 && !facingRight)
			Flip();
		//If player is moving LEFT but facing right, flip sprite
		else if(moveH < 0 && facingRight)
			Flip();

		if(jump)
		{
			//Tells the animator when to play the jump script
			//anim.SetTrigger("Jump");

			//Adds vertical jump force to the player
			//Left parameter is x direction force
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			// Reset jump to ensure player can't jump again unless jump condition is satisfied
			jump = false;
		}
	}

	void Flip() {
		//Change the direction boolean
		facingRight = !facingRight;

		Vector3 playerDirection = transform.localScale;
		//Flips the players direction 
		playerDirection.x *= -1;
		transform.localScale = playerDirection;
	}
}
