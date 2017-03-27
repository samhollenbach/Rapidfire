using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Gun : MonoBehaviour {

	public GameObject bullet;
	public float speed = 20f;

	public PlayerControl playerControl;
	private Animator anim;

	private bool fired = false;

	// Use this for initialization
	void Start () {
		anim = transform.root.gameObject.GetComponent<Animator> ();
		playerControl = transform.root.GetComponent<PlayerControl> ();

	}
	/// <summary>
	/// /
	/// </summary>
	
	// Update is called once per frame
	void Update () {
		


	}

	public void fire (){
		fired = true;
	}




}
