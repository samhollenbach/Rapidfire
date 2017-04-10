using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
	
	private const int BULLET_DAMAGE = 20;

	[SyncVar]
	public GameObject playerSource;
		
	void OnTriggerEnter2D(Collider2D col){
		//Might need to use col.tag, need to fix gunscript to test
		if (col.tag == "Player") {
			GameObject p = col.gameObject;
			if (p != playerSource){
				p.GetComponent<PlayerHealth> ().Hurt (BULLET_DAMAGE);
				NetworkServer.Destroy (gameObject);
			}
		} else if (col.tag == "ground") {
			NetworkServer.Destroy (gameObject);
		}
		 
	}

	public void setSource(GameObject source){
		this.playerSource = source;
	}

}
