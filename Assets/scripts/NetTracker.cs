using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//This was built using the Unity multiplayer networking API
//Documentation for the Unity multiplayer networking can be found here:
//https://unity3d.com/learn/tutorials/topics/multiplayer-networking
public class NetTracker : NetworkBehaviour {

	//Synvs facingRight to the server
	[SyncVar]
	public bool netFacingRight = true;


	//Command runs on the server and tells the facing right variable to change
	//https://docs.unity3d.com/ScriptReference/Networking.CommandAttribute.html
	[Command]
	public void CmdFlipSprite(GameObject player, bool facing)
	{
		//The server calls back the flip to run on all the clients
		RpcFacingCallback(player, facing);
	}

	//The callback function makes the facingRight change to all clients
	//https://docs.unity3d.com/ScriptReference/Networking.ClientRpcAttribute.html
	[ClientRpc]
	void RpcFacingCallback(GameObject player, bool facing)
	{
		//Stores the player control component for use below
		PlayerControl playerControl = player.GetComponent<PlayerControl>();

		//Sets the network facing right variable
		player.GetComponent<NetTracker>().netFacingRight = facing;

		//Gets the player local scale for use below
		Vector3 playerLocalScale = player.transform.localScale;

		//If facing right, scale sprite right and vice versa
		float tempX = netFacingRight ? Mathf.Abs(playerLocalScale.x) : -Mathf.Abs(playerLocalScale.x);

		//Set sprite scale
		player.transform.localScale = new Vector3 (tempX, playerLocalScale.y, playerLocalScale.z);

		//Force camera to reset lock after changing scale
		playerControl.playerCam.GetComponent<CameraFollow>().lockCamera();
	}
}
