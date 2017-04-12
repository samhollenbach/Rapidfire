using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetTracker : NetworkBehaviour {

	//Hook calls the FacingCallback function whenever netFacingRight is changed
	[SyncVar(hook = "FacingCallback")]
	public bool netFacingRight = true;


	//Command runs on the server and tells the facing right variable to change
	[Command]
	public void CmdFlipSprite(bool facing)
	{
		//Tells the sprite to flip directions depending on which directions the player is facing
		netFacingRight = facing;
		if (netFacingRight)
		{
			Vector3 SpriteScale = transform.localScale;
			float scaleX = SpriteScale.x;
			SpriteScale.x = Mathf.Abs(scaleX);
			transform.localScale = SpriteScale;
		}
		else
		{
			Vector3 SpriteScale = transform.localScale;
			float scaleX = SpriteScale.x;
			SpriteScale.x = -Mathf.Abs(scaleX);
			transform.localScale = SpriteScale;
		}
	}

	//The callback function makes the facingRight change to all clients
	void FacingCallback(bool facing)
	{
		//Does same as CmdFlipSprite but runs on the clients
		netFacingRight = facing;
		if (netFacingRight)
		{
			Vector3 SpriteScale = transform.localScale;
			float scaleX = SpriteScale.x;
			SpriteScale.x = Mathf.Abs(scaleX);
			transform.localScale = SpriteScale;
		}
		else
		{
			Vector3 SpriteScale = transform.localScale;
			float scaleX = SpriteScale.x;
			SpriteScale.x = -Mathf.Abs(scaleX);
			transform.localScale = SpriteScale;
		}
	}
}
