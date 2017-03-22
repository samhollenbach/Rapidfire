using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

	public int bulletDamage = 20;
	public GameObject playerSource;

	void Start(){
		
	}
		
//	void OnCollisionEnter2D(Collider2D col) {
//		if (col.gameObject.tag == "Player") {
//			col.gameObject.SendMessage ("Hurt", bulletDamage);
//			Destroy (gameObject);
//		} else if (col.gameObject.tag == "ground") {
//			Destroy (gameObject);
//		} else {
//			Destroy (gameObject);
//		}
//	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="col">Col.</param>
		
	void OnTriggerEnter2D(Collider2D col){
		//Might need to use col.tag, need to fix gunscript to test
		if (col.tag == "Player") {
			GameObject p = col.gameObject;
			if (p != playerSource){
				p.GetComponent<PlayerHealth> ().Hurt (bulletDamage);
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
