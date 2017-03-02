using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerControl : NetworkBehaviour {


	//Will have to play with these numbers
	public float maxSpeed = 50f;
	public float moveForce = 900f;
	public float jumpForce = 5000f;

	public int HP = 100;
	public bool dead = false;

	//Conditions for direction faced and ability to jump
	bool facingRight = true;
	bool jump = false;

	//If some other force is moving the player
	public bool ungrounded = false;

	//Conditions for checking whether or not the character is grounded
	public Transform groundCheck;
	private bool grounded = false;

	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	//Links the character to animations
	//private Animator anim;

	public Transform gun;


	[SyncVar]
	public Vector3 mousePos;

	private GameObject playerGun;


	/// <summary>
	/// Start this instance.
	/// </summary>
	// Use this for initialization
	void Start () {
		
	}

	public override void OnStartLocalPlayer()
	{
		
		groundCheck = transform.Find("groundCheck");

		//anim = GetComponent<Animator>();
		gun = transform.Find ("Gun");
		//Vector3 gunSpawnPosition = transform.position + new Vector3 (2,5,0);
		//playerGun = (GameObject)Instantiate (gun.gameObject, gunSpawnPosition, Quaternion.identity);
		//currentGun.GetComponent<Gun>().playerControl = this;
		Camera.main.GetComponent<CameraFollow>().setTarget(this.transform);

		GetComponent<SpriteRenderer>().color = Color.yellow;

	}


	// Update is called once per frame
	void Update() {
		if (!isLocalPlayer) {
			
			return;
		}

		setMousePos(Input.mousePosition);
		//Updates to check for the player's grounded status
		//Grounded if linecast to groundcheck position hits anything on the ground
		//grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground"));
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		if (Input.GetButtonDown ("Jump") && grounded) {
			jump = true;
		}

		if (Input.GetButtonDown ("Fire1")) {
			if (gun != null) {
				print (mousePos);
				CmdFire (gameObject, mousePos);
			
			}
		}

	}


	public Gun getCurrentGun(){
		return(gun.GetComponent<Gun>());
	}

	private void setMousePos(Vector3 m){
		mousePos = m;
	}

	public Vector3 getMousePos(){
		return(mousePos);
	}

	void FixedUpdate () {
		if (!isLocalPlayer)
			return;


		
		float moveH = Input.GetAxisRaw ("Horizontal");

		if (!ungrounded && moveH == 0) {
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





	[Command]
	void CmdFire(GameObject player, Vector3 mouse){

		Gun cg = player.GetComponentInChildren<Gun> ();
		//print (cg.transform.position);
		Vector3 playerPos = player.transform.position + new Vector3 (2,5,0);

		var mousePos = mouse;
		mousePos.z = 10;


		Vector3 cursorInWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
		Vector3 direction = cursorInWorldPos - playerPos;
		direction.z = 0;
		direction.Normalize();
		cg.bullet.GetComponent<Bullet>().setSource (player);
		if ((direction.x < 0 && player.GetComponent<PlayerControl> ().facingRight) || (direction.x > 0 && !player.GetComponent<PlayerControl> ().facingRight)) {
			player.GetComponent<PlayerControl> ().Flip ();
		}
		Vector3 gunPos = new Vector3 (3, 0, 0);
		if (!player.GetComponent<PlayerControl> ().facingRight) {
			gunPos.x = -gunPos.x;
		}

		var projectile = Instantiate(cg.bullet, cg.transform.position + gunPos, Quaternion.identity);
		projectile.GetComponent<Rigidbody2D>().velocity = (direction * cg.speed) * 5;
		if (projectile.GetComponent<Rigidbody2D> ().velocity.x < 0) {
			projectile.GetComponent<SpriteRenderer> ().flipX = true;
		}
		//print(player.transform.position);
		NetworkServer.Spawn (projectile);
	}

	public void Hurt(int damage) {
		HP -= damage;
		Death ();
	}

	public void Death() {
		if (HP <= 0) {
			dead = true;
			Destroy (gameObject);
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
