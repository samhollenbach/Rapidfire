using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetTracker : NetworkBehaviour {

	[SyncVar(hook = "FacingCallback")]
	public bool netFacingRight = true;

	[Command]
	public void CmdFlipSprite(bool facing)
	{
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

	void FacingCallback(bool facing)
	{
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
