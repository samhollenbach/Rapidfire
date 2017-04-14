using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Gun : MonoBehaviour {

	//Prefab of which bullet this gun fires
	public GameObject bullet;

	//Speed at which bullet leaves gun
	public float speed = 1000f;

	//Player holding this gun
	public PlayerControl playerControl;

	//Gun animator object
	//private Animator anim;

	//Checks if the gun as already fired (for fire rate cooldown/reload)
	//private bool fired = false;

	// Use this for initialization
	void Start () {
		//anim = transform.root.gameObject.GetComponent<Animator> ();
		playerControl = transform.root.GetComponent<PlayerControl> ();

	}

	//Used for fire-rate and reload (NOT CURRENTLY IN USE)
	public void fire (){
		//fired = true;
	}




}
