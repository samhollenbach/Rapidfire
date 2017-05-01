using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

	public int HP = 100;

	[SyncVar]
	public int currentHP;

	private bool hurt = false;
	private bool dead = false;

	private HealthBar playerHealthBar;
	private PlayerControl plyrControl;

	// Use this for initialization
	void Start () {
		currentHP = HP;

		playerHealthBar = this.gameObject.GetComponent<HealthBar> ();
		playerHealthBar.maxHealth = HP;
		playerHealthBar.currentHealth = currentHP;

		plyrControl = this.gameObject.GetComponent<PlayerControl> ();
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

	public void Hurt(int damage) {
		if (!isServer) {
			return;
		}
		hurt = true;
		currentHP -= damage;
		if (currentHP <= 0) {
			playerHealthBar.currentHealth = 1;
		} else {
			playerHealthBar.currentHealth = currentHP;
		}
		if (!dead) {
			Death ();
		}
	}

	public void Death() {
		if (!isServer) {
			return;
		}
		if (currentHP <= 0) {
			dead = true;
			RpcDeathAnimation(this.gameObject);
		}
	}

	//Pause for 5 seconds before sending all players to end game screen
	IEnumerator waitForDeath () {
		//Wait for 5 seconds
		yield return new WaitForSeconds(5.0f);
		//Call RpcDeath on all clients

		GetComponent<PlayerControl> ().RpcEndGame ();
	}

	[ClientRpc]
	public void RpcDeathAnimation(GameObject player) {
		player.GetComponent<PlayerHealth>().playerHealthBar.enabled = false;
		player.GetComponent<NetworkAnimator> ().SetTrigger ("Death");
	}
}
