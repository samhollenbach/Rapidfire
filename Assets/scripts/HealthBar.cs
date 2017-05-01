using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//This class was modeled on the HealthBar class found in the unity documentation here:
//https://docs.unity3d.com/Manual/UNetSetup.html

//Networking was applied using the Unity networker, documentation found here:
//https://unity3d.com/learn/tutorials/topics/multiplayer-networking


public class HealthBar : NetworkBehaviour{

	GUIStyle healthStyle;
	GUIStyle backStyle;

	public GameObject player; //The player associated with this healthBar
	public float maxHealth; 

	public BoxCollider2D collider; //The 2d collision box of the player, used to calculate position

	[SyncVar]
	public float currentHealth; //The value for the current health of the player as represented by the HealthBar

	[SyncVar]
	Vector3 pos; //The position of the HealthBar

	void Update() {
		collider = this.gameObject.GetComponent<BoxCollider2D> ();
	}


	void OnGUI()
	{
		InitStyles();

		// Draw a Health Bar

		Vector3 HealthBarPosition = new Vector3(0,0,0);

		//Sets the position of the HealthBar using the collider size
		try{
			HealthBarPosition = transform.position + new Vector3 (0, collider.size.y * 3.5f, 0);
		}catch(Exception e){
			print (e);
		}

		//Converts HealthBar position to ScreenSpace
		pos = Camera.main.WorldToScreenPoint(HealthBarPosition);
		// draw health bar background
		GUI.color = Color.grey;
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(pos.x-26, Screen.height - pos.y, maxHealth/2, 7), ".", backStyle);

		// draw health bar amount
		GUI.color = Color.green;
		GUI.backgroundColor = Color.green;
		GUI.Box(new Rect(pos.x-25, Screen.height - pos.y, currentHealth/2, 5), ".", healthStyle);
	}

	void InitStyles()
	{
		if( healthStyle == null )
		{
			healthStyle = new GUIStyle( GUI.skin.box );
			healthStyle.normal.background = MakeTex( 2, 2, new Color( 0f, 1f, 0f, 1.0f ) );
		}

		if( backStyle == null )
		{
			backStyle = new GUIStyle( GUI.skin.box );
			backStyle.normal.background = MakeTex( 2, 2, new Color( 0f, 0f, 0f, 1.0f ) );
		}
	}

	Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}

	public void setPlayer(GameObject p) {
		this.player = p;
	}
}
