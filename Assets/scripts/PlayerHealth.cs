using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerHealth : NetworkBehaviour {

	public int HP = 100;

	[SyncVar]
	public int currentHP;

//	public Image hurtImage;
//	public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
//	public float flashSpeed = 5f;

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

//		hurtImage = GameObject.Find ("HUDCanvas/Image").GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () {
		if (hurt) {
//			hurtImage.color = flashColor;
		} else {
//			hurtImage.color = Color.Lerp (hurtImage.color, Color.clear, flashSpeed * Time.deltaTime);
		} 
		hurt = false;

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
//			playerHealthBar.currentHealth = 0;
			dead = true;
			RpcDeathAnimation(this.gameObject);
		}
	}

	//Pause for 5 seconds before sending all players to end game screen
	IEnumerator waitForDeath () {
		//Wait for 5 seconds
		yield return new WaitForSeconds(5.0f);
		//Call RpcDeath on all clients


		RpcDeath ();

	}

	//Tells all clients to disconnect from the server and load the end screen
	[ClientRpc]
	public void RpcDeath() {
		//Stops the client connection to the server
		NetworkLobbyManager.singleton.StopClient ();
		//Closes the network manager HUD
		NetworkLobbyManager.singleton.GetComponent<NetworkManagerHUD> ().enabled = false;

		//Stops the server host
//		if (isServer) {
//			
//		}
		//Loads the end game screen
		SceneManager.LoadScene (3);
	}




	[ClientRpc]
	public void RpcDeathAnimation(GameObject player) {
		player.GetComponent<PlayerHealth>().playerHealthBar.enabled = false;
		player.GetComponent<NetworkAnimator> ().SetTrigger ("Death");
	}
}
