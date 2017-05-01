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



	//change all these to vectors




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

			Transform wallLeft = GameObject.Find("WallLeft").transform;
			Transform wallRight = GameObject.Find("WallRight").transform;
			Transform floor = GameObject.Find("Floor").transform;
			Transform ceiling = GameObject.Find("Ceiling").transform;

			Camera playerCam = player.GetComponent<PlayerControl> ().playerCam;

			 
			Vector3 playerPos = new Vector3 (playerTransform.position.x, playerTransform.position.y,0);
			Vector3 lWPos = new Vector3 (wallLeft.position.x, wallLeft.position.y,0);
			Vector3 rWPos = new Vector3 (wallRight.position.x, wallRight.position.y,0);
			Vector3 fPos = new Vector3 (floor.position.x, floor.position.y,0);
			Vector3 cPos = new Vector3 (ceiling.position.x, ceiling.position.y,0);
//			Vector3 playerScreen = playerCam.WorldToScreenPoint (playerPos);
//			Vector3 leftWallPos = playerCam.WorldToScreenPoint (lWPos);
//			Vector3 rightWallPos = playerCam.WorldToScreenPoint (rWPos);
//			Vector3 floorPos = playerCam.WorldToScreenPoint (fPos);
//			Vector3 ceilPos = playerCam.WorldToScreenPoint (cPos);

			Vector3 playerScreen = playerCam.WorldToViewportPoint (playerPos);
			Vector3 leftWallPos = playerCam.WorldToViewportPoint (lWPos);
			Vector3 rightWallPos = playerCam.WorldToViewportPoint (rWPos);
			Vector3 floorPos = playerCam.WorldToViewportPoint (fPos);
			Vector3 ceilPos = playerCam.WorldToViewportPoint (cPos);

			float xMax = 0;
			float xMin = 0;
			float yMax = 0;
			float yMin = 0;


//			print (w);
//			print (h);
//			print (lWPos);
//			print (rWPos);
//			print (fPos);
//			print (cPos);
//			print (leftWallPos);
//			print (rightWallPos);
//			print (floorPos);
//			print (ceilPos);
//
//			float viewportX = .5f;
//			float viewportY = .5f;
//
//			if (leftWallPos.x < 0) {
//				viewportX = 0f;
//			} else if (rightWallPos.x > 1) {
//				viewportX = 1f;
//			} else {
//				viewportX = .5f;
//			}
//			if (ceilPos.y < 0) {
//				viewportY = 0f;
//			} else if (floorPos.x > 1) {
//				viewportY = 1f;
//			} else {
//				viewportY = .5f;
//			}
//			Vector3 camPos = new Vector3 (x,y,-10);
//			Vector3 worldCam = playerCam.ViewportToWorldPoint(camPos);
//
//			transform.position = new Vector3(worldCam.x,worldCam.y,depth);
//			transform.position = worldCam;


			//Keeps the camera within the bounds of the map (will use map size variables soon)
			if (playerTransform.position.x >= 80) {
				x = 82;
			} else if (playerTransform.position.x <= -80) {
				x = -82;
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