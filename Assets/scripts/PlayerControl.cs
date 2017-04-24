using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerControl : NetworkBehaviour {

	//Standard player movement constants
	//Will have to play with these numbers
	public float maxSpeed = 50f;
	public float moveForce = 900f;
	public float jumpForce = 5000f;

	//Conditions for direction faced and ability to jump
	//[SyncVar]
	public bool facingRight = true;

	//If some other force is moving the player
	private bool ungrounded = false; //Checks if force should be added to the player in the x direction if player is in air

	//Conditions for checking whether or not the character is grounded
	private Transform groundCheck;

	//Checks if the player is on the ground
	private bool grounded = false;

	private float groundRadius = 0.2f;

	private float nextFire;

	//Determines what player can stand on
	public LayerMask whatIsGround;

	//Stores player mouse position
	[SyncVar]
	private Vector3 mousePos;

	//Stores the object of the players camera
	public Camera playerCam;

	//Keeps track of the players health bar
	private HealthBar playerHealthBar;

	//NetTracker keeps track of some extra player variables on the network
	public NetTracker netTracker;

	private Gun gun;

	private Animator anim;

	private bool jumped = false;

	[SyncVar]
	public bool canMove;

	// This method is run as soon as the script is compiled
	void Awake () {

		Application.runInBackground = true;
		anim = GetComponent<Animator> ();

		nextFire = 0f;

		canMove = true;

		Debug.developerConsoleVisible = true;

		playerCam = GetComponentInChildren<Camera> ();
		playerCam.gameObject.SetActive (false);
	}

	//This method is called when the player object is created in the world
	//Only runs on the local client and not the server
	public override void OnStartLocalPlayer(){
		//Initializes a NetTracker object to store networked variables
		this.netTracker = GetComponent<NetTracker> ();

		//Initializes HUDCanvas object for showing Health Bars
		GameObject.Find("HUDCanvas").GetComponent<Canvas>();

		//Initializes object to check if player is on the ground
		groundCheck = transform.Find("groundCheck");

		//Activates this players camera
		playerCam.gameObject.SetActive(true);
		//Links the camera to this player
		playerCam.GetComponent<CameraFollow> ().setTarget (this.gameObject);

		//Links a health bar object to this player
		playerHealthBar = GetComponent<HealthBar> ();
		playerHealthBar.setPlayer (this.gameObject);

		gun = GetComponentInChildren<Gun> ();

		//Must force a flip call a fraction of a second after starting the game or else the client 
		//will not register the flip
		StartCoroutine (forceFlip ());

		//Colors sprites white to tell difference between  players
		foreach (var sprite in this.gameObject.GetComponentsInChildren<SpriteRenderer>()) {
			sprite.color = Color.white;
		}
	}


	//Update is called on every action or input that takes place
	void Update() {
		//Makes sure the updates are only being made on the local client
		if (!isLocalPlayer) {
			return;
		}
			
		//Instantiates a zero vector for the mouse
		Vector3 mousePosition = new Vector3 (0, 0, 0);
		bool mouseNull = false;

		//Tries to get the current mouse position on the screen
		try{
			mousePosition = Input.mousePosition;
		}
		catch(Exception e){
			//If the mouse position cannot be set, set mouse to null
			mouseNull = true;
			print (e);
		}
			
		//Must set the z axis =/= 0 to use ScreenToWorldPoint
		mousePosition.z = 10;
		//Debug.Log (mousePosition);
		//Sets the current mouse position for other methods to use
		setMousePos (mousePosition);

		if (canMove) {
			if (!mouseNull) {
				checkFlip (this.gameObject, playerCam.ScreenToWorldPoint(getMousePos()));
			}


			//Checks the fire input and runs the CmdFire method
			checkFire ();

			//Checks horizontal inputs and applies movements
			applyHorizontalMovement ();

			//Checks jump input and applies jump if grounded
			checkJump ();
		}
	}

	//Checks what inputs are down and applies horizontal movement
	void applyHorizontalMovement(){
		float moveH = Input.GetAxisRaw ("Horizontal");

		//If the player is on the ground and there is not horizontal movement, set x axis velocity to zero
		if (!ungrounded && moveH == 0) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
		}

		//Sets the speed of the movement animation
		anim.SetFloat ("Speed", Mathf.Abs(moveH));

		//If horizontal velocity is < maxspeed
		if(moveH * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			//Increase horizontal velocity by adding a force to player
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveH * moveForce);

		// If horizontal velocity > maxspeed
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			//Set horizontal velocity to maxspeed
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
		
	}

	//Checks if the player is on the ground and calls jump
	void checkJump(){
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		//Using GetButton instead of GetButtonDown
		if (Input.GetButton ("Jump") && grounded && !jumped) {
			//Runs the jump process with a coroutine so the IEnumerator can run the timer to reset jump	(inefficient)
			StartCoroutine(applyJump ());
		}
	}

	//Applies the jump force to the player
	IEnumerator applyJump(){

		//Triggers the jump animation to play
		anim.SetTrigger ("Jump");
		//Adds force vector upwards to player
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0f, jumpForce));

		//Sets a timer so the player cannot jump again for .01s
		jumped = true;
		yield return new WaitForSeconds(0.01f);
		jumped = false;
	}

	//Checks if the the player should flip its sprite direction
	void checkFlip(GameObject player, Vector3 mouse){
		bool mouseRight = true;
		//If mouse is actually to the left of the player then set mouseRight to false
		if(mouse.x < player.transform.position.x) {
			mouseRight = false;
		}
		//If the mouse is on the wrong side of the player then flip the player
		if (mouseRight != facingRight) {
			Flip (player, mouseRight);
		}

	}
	//Tells the NetTracker component to switch the direction the player is facing
	void Flip(GameObject player, bool facing) {
		//Sets the client facing varibale
		facingRight = facing;
		//Tells the server to flip this player
		netTracker.CmdFlipSprite(this.gameObject, facingRight);
	}

	//Forces the player to flip when the game starts after .05s
	IEnumerator forceFlip(){
		yield return new WaitForSeconds(0.05f);
		checkFlip (this.gameObject, getMousePos ());

	}


	//Runs the fire command if the player has pressed fire
	void checkFire(){
		if (Time.time > nextFire && Input.GetButton ("Fire1")) {
			CmdFire (this.gameObject, playerCam.ScreenToWorldPoint(getMousePos()));
			nextFire = Time.time + gun.getFireRate();
		}
	}

	//Command methods are run on the server but called on the client. This method instantiates a bullet object from the
	//referenced player's gun in the direction that their mouse is facing and uses NetworkServer.Spawn to spawn
	//the bullet on the server
	[Command]
	void CmdFire(GameObject player, Vector3 cursor){

		//Get the players gun object
		Gun cg = player.GetComponentInChildren<Gun> ();

		//Gets the position of the gunTip object
		Vector3 gunTip = cg.getGunTipPos();

		//Creates the position for the bullet to spawn from
		Vector3 bulletSpawn = gunTip;

		//Creates the direction vector on which the bullet will travel and normalizes it
		Vector3 direction = cursor - bulletSpawn;
		direction.z = 0;
		direction.Normalize();

		//Creates the bullet projectile
		var projectile = Instantiate(cg.bullet, bulletSpawn, Quaternion.identity);

		//Sets the angle of travel and rotation for the bullet
		var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		projectile.transform.rotation = Quaternion.AngleAxis (angle,Vector3.forward);

		//Sets the source of the bullet to this player
		projectile.GetComponent<Bullet> ().setSource(player);

		//Sets the velocity of the bullet
		projectile.GetComponent<Rigidbody2D>().velocity = (direction * cg.speed) * 10;

		//Tells the NetworkServer to spawn the bullet and keep track of it for all clients
		NetworkServer.Spawn (projectile);
	}


	//Sets the player unable to move
	public void setCanMove(bool canMove) {
		this.canMove = canMove;
	}

	//Sets the mouse position
	private void setMousePos(Vector3 m){
		mousePos = m;
	}

	//Returns the current position of the mouse
	public Vector3 getMousePos(){
		return(mousePos);
	}

}