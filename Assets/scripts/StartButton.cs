using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class StartButton : MonoBehaviour {
	
	public void StartGameButton(int sceneIndex){
		SceneManager.LoadScene (sceneIndex);
		if (NetworkLobbyManager.singleton != null) {
			if (sceneIndex == 1) {
				NetworkLobbyManager.singleton.GetComponent<NetworkManagerHUD> ().enabled = true;
			} else {
				NetworkLobbyManager.singleton.GetComponent<NetworkManagerHUD> ().enabled = false;
			}
		}
	}

}
