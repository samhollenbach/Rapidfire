using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	//Player this camera is following
	public GameObject player;

	//How far away the camera is from the screen (cannot be 0)
	public int depth = -10;
	public int w = Screen.width/2;
	public int h = Screen.height/2;

	// Update is called once per frame
	void Update()
	{
		lockCamera ();
	}

	public void lockCamera(){
		//Gets the current player's transform object and position
		Transform playerTransform = player.transform;

		if(playerTransform != null)
		{
			float x = transform.position.x;
			float y = transform.position.y;

			//Keeps the camera within the bounds of the map (will use map size variables soon)
			if (playerTransform.position.x >= 90) {
				x = 90;
			} else if (playerTransform.position.x <= -90) {
				x = -90;
			} else {
				x = playerTransform.position.x;
			}
			if (playerTransform.position.y <= -37) {
				y = -37;
			} else if (playerTransform.position.y >= 47) {
				y = 47;
			} else {
				y = playerTransform.position.y;
			}
			//Sets the camera transform to these new coordinates
			transform.position = new Vector3(x,y,depth);		
		}
	}

	//Sets the player which this camera should follow
	public void setTarget(GameObject target){
		this.player = target;
	}
		
}