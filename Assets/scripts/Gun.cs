using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Gun : MonoBehaviour {

	//Prefab of which bullet this gun fires
	public GameObject bullet;

	//Speed at which bullet leaves gun
	public float speed = 1000f;

	//The location of the gun's tip
	private Transform gunTip;

	//How fast the gun can fire
	private const float FIRE_RATE = 0.3f;

	// Use this for initialization
	void Start () {
		gunTip = transform.Find ("gunTip");
	}

	public float getFireRate() {
		return FIRE_RATE;
	}

	public Vector3 getGunTipPos() {
		return gunTip.position;
	}
}
