using UnityEngine;
using System.Collections;

public class ClientControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Connecting");

		this.networkControl = GameObject.Find("NetworkControl").GetComponent<NetworkControl>();
		this.playerObjectTable = GameObject.Find("PlayerObjectTable").GetComponent<PlayerObjectTable>();

		Network.Connect(this.networkControl.ServerIP, this.networkControl.ServerPort);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private NetworkControl networkControl;
	private PlayerObjectTable playerObjectTable;

	private void OnConnectedToServer()
	{
		Debug.Log("Connected to server.");
	}

	private void OnFailedToConnect(NetworkConnectionError error) 
	{
		GameObject.Destroy(this);
	}
}
