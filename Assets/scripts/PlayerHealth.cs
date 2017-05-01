using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//This class was modeled on the Unity 2D project Health script found here:
//https://www.assetstore.unity3d.com/en/#!/content/11228

//Networking was applied by using the unity networking documentation found here:
//https://unity3d.com/learn/tutorials/topics/multiplayer-networking

public class PlayerHealth : NetworkBehaviour {

	private const float HP = 100.0f; //The player's initial HP

	[SyncVar]
	public float currentHP; //The player's current HP

	private bool hurt = false;
	private bool dead = false;

	private HealthBar playerHealthBar;
	private PlayerControl plyrControl;

	// Use this for initialization
	void Start () {
		currentHP = HP;

		playerHealthBar = this.gameObject.GetComponent<HealthBar> (); //This player's HealthBar
		playerHealthBar.maxHealth = HP;
		playerHealthBar.currentHealth = currentHP;

		plyrControl = this.gameObject.GetComponent<PlayerControl> (); //This player's PlayerController
	}

	// Update is called once per frame
	void Update () {
		if (hurt) {
			hurt = false;
		}
		if (dead) {
			plyrControl.setCanMove(false); 
			StartCoroutine(waitForDeath());
		}
	}

	//Called when damage should be applied to the player
	public void Hurt(int damage) {
		//Only runs health calculations on the server
		if (!isServer) {
			return;
		}

		hurt = true;
		currentHP -= damage;
		//Sets the health value to be displayed by the healthbar
		playerHealthBar.currentHealth = currentHP;

		if (!dead) {
			checkDeath ();
		}
	}

	//Checks for player death 
	public void checkDeath() {
		if (!isServer) {
			return;
		}
		if (currentHP <= 0) {
			dead = true;
			//Plays the death animation if dead
			RpcDeathAnimation(this.gameObject);
		}
	}

	//Pause for 5 seconds before sending all players to end game screen
	IEnumerator waitForDeath () {
		//Wait for 5 seconds
		yield return new WaitForSeconds(5.0f);

		//Call RpcEndGame on all clients
		GetComponent<PlayerControl> ().RpcEndGame ();
	}

	[ClientRpc]
	public void RpcDeathAnimation(GameObject player) {
		//Deletes the dead player's HealthBar
		player.GetComponent<PlayerHealth>().playerHealthBar.enabled = false;
		//Triggers the Death animation over the network
		player.GetComponent<NetworkAnimator> ().SetTrigger ("Death");
	}
}
