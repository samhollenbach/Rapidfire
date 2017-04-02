using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

	public int HP = 100;

	[SyncVar]
	public int currentHP;

	public Slider healthSlider;
	public Image hurtImage;
	public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
	public float flashSpeed = 5f;
	public bool hurt = false;
	public bool dead = false;

	public HealthBar playerHealthBar;
//	public GameObject player;

	public GameObject Canvas;
	private GameObject can;
	private Canvas currentCanvas;


	//	private GameObject sl;
	//	private Slider currentSlider;

	// Use this for initialization
	void Start () {
		currentHP = HP;
		playerHealthBar = this.gameObject.GetComponent<HealthBar> ();
		playerHealthBar.maxHealth = HP;
		playerHealthBar.currentHealth = currentHP;
		healthSlider = GameObject.Find ("HUDCanvas/healthGUI/Slider").GetComponent<Slider>();
		hurtImage = GameObject.Find ("HUDCanvas/Image").GetComponent<Image>();
		healthSlider.value = currentHP;
	}

	// Update is called once per frame
	void Update () {
		if (hurt) {
			hurtImage.color = flashColor;
		} else {
			hurtImage.color = Color.Lerp (hurtImage.color, Color.clear, flashSpeed * Time.deltaTime);
		} 
		hurt = false;
	}

	public void Hurt(int damage) {
		if (!isServer) {
			return;
		}
		hurt = true;
		currentHP -= damage;
		print (currentHP);
		healthSlider.value = currentHP;
		playerHealthBar.currentHealth = currentHP;
		if (!dead) {
			Death ();
		}
	}

	public void Death() {
		if (!isServer) {
			return;
		}
		if (currentHP <= 0) {
			playerHealthBar.currentHealth = 0;
			dead = true;
			print (dead);
			NetworkServer.Destroy (gameObject);
		}
	}

//	public void setPlayer(GameObject p) {
//		this.player = p;
//	}
}
