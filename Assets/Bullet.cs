using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

	//Bullet damage
	private const int BULLET_DAMAGE = 20;

	//Sync the player source across the server so each client knows who the bullet belongs to
	[SyncVar]
	public GameObject playerSource;
		
	//Built in function is called when the bullet collides with an object in the world
	void OnTriggerEnter2D(Collider2D col){
		//Checks if the object hit is a player
		if (col.tag == "Player") {
			GameObject playerHit = col.gameObject;
			//If the object is a player, check if it is the player who shot the bullet
			if (playerHit != playerSource){
				//If not the same player, apply damage and destroy the bullet
				playerHit.GetComponent<PlayerHealth> ().Hurt (BULLET_DAMAGE);
				NetworkServer.Destroy (gameObject);
			}
		//If bullet strikes ground, despawn it
		} else if (col.tag == "ground") {
			NetworkServer.Destroy (gameObject);
		}
		 
	}

	//Set the source of the player who shot this bullet
	public void setSource(GameObject source){
		this.playerSource = source;
	}

}
