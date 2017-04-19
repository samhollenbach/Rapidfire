using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetTracker : NetworkBehaviour {

	//Synvs facingRight to the server
	[SyncVar]
	public bool netFacingRight = true;


	//Command runs on the server and tells the facing right variable to change
	[Command]
	public void CmdFlipSprite(GameObject player, bool facing)
	{
		//netFacingRight = facing;
		RpcFacingCallback(player, facing);
	}

	//The callback function makes the facingRight change to all clients
	[ClientRpc]
	void RpcFacingCallback(GameObject player, bool facing)
	{
		//Does same as CmdFlipSprite but runs on the clients
		PlayerControl playerControl = player.GetComponent<PlayerControl>();
		player.GetComponent<NetTracker>().netFacingRight = facing;
		Vector3 playerLocalScale = player.transform.localScale;

		//If facing right, scale sprite right and vice versa
		float tempX = netFacingRight ? Mathf.Abs(playerLocalScale.x) : -Mathf.Abs(playerLocalScale.x);

		//Set sprite scale
		player.transform.localScale = new Vector3 (tempX, playerLocalScale.y, playerLocalScale.z);

		//Force camera to reset lock after changing scale
		playerControl.playerCam.GetComponent<CameraFollow>().lockCamera();
	}
}
