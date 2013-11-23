using UnityEngine;
using System.Collections;

public class ServerControl : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		this.networkControl = GameObject.Find("NetworkControl").GetComponent<NetworkControl>();
		this.playerObjectTable = GameObject.Find("PlayerObjectTable").GetComponent<PlayerObjectTable>();

//		Player player = new Player(this.networkControl.LocalViewID, Network.player);
//		this.networkControl.Players.Add(this.networkControl.LocalViewID, player);
//		this.playerObjectTable.AddPlayerTable(player);

		Network.InitializeServer(10, this.networkControl.ServerPort, false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private NetworkControl networkControl;
	private PlayerObjectTable playerObjectTable;

	private void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
		Debug.Log("Someone joined!");

		// Generate a viewID for the new player.
		NetworkViewID viewID = Network.AllocateViewID();

		// Notice everyone that the new player has joined and create that player's ship.
		// Buffer the RPC calls.
		this.networkControl.networkView.RPC("NewPlayerJoined", RPCMode.AllBuffered, networkPlayer, viewID);
		this.networkControl.networkView.RPC("CreatePlayerShip", RPCMode.AllBuffered, networkPlayer, viewID);
	}
}
