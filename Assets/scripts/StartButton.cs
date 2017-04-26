using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class StartButton : MonoBehaviour {
	
	public void StartGameButton(int sceneIndex){
		SceneManager.LoadScene (sceneIndex);
		NetworkLobbyManager.singleton.GetComponent<NetworkManagerHUD> ().enabled = true;
	}

}
