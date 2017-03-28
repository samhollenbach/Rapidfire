using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour
{
	[SyncVar]
	public GameObject player;
	public int depth = -10;




	// Update is called once per frame
	void Update()
	{
		Transform playerTransform = player.transform;
		float x = transform.position.x;
		float y = transform.position.y;

		if (playerTransform.position.x >= 80) {
			x = 82;
		} else if (playerTransform.position.x <= -80) {
			x = -82;
		} else {
			x = playerTransform.position.x;
		}
		if (playerTransform.position.y <= -37) {
			y = -37;
		} else if (playerTransform.position.y >= 37) {
			y = 37;
		} else {
			y = playerTransform.position.y;
		}
		if(playerTransform != null)
		{
			transform.position = new Vector3 (x, y, depth);
		}
	}

	[Command]
	public void CmdSetTarget(GameObject target)
	{
		player = target;
	}
}