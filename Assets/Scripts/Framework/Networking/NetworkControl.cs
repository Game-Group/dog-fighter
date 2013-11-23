using UnityEngine;
using System.Collections.Generic;
using System.Net;

public class NetworkControl : MonoBehaviour 
{
	public int ServerPort = 6500;
	public string ServerIP = "127.0.0.1";

	public string LocalIP { get; set; }
	public NetworkViewID LocalViewID { get; set; }

	public GameObject ServerControl;
	public GameObject ClientControl;

	public IDictionary<NetworkViewID, Player> Players { get; private set; }
	public Player ThisPlayer
	{
		get
		{
			return this.Players[this.networkView.viewID];
		}
	}
	
	// Use this for initialization
	public void Start () 
	{
		// Find the local IP Address.
		IPAddress localAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
		byte[] bytes = localAddress.GetAddressBytes();

		this.LocalIP = 
			bytes[0].ToString() + "." + bytes[1].ToString() + "." + bytes[2].ToString() + "." + bytes[3].ToString();

		this.Players = new Dictionary<NetworkViewID, Player>(10);
	}
	
	// Update is called once per frame
	public void Update () 
	{
		if (!GameObject.Find("Global").GetComponent<GlobalSettings>().HasFocus)
			return;

		if (Network.peerType == NetworkPeerType.Disconnected)
		{		
			if (Input.GetKeyDown(KeyCode.F1))
			{
				Debug.Log ("Initializing as server.");
				GameObject.Instantiate(this.ServerControl);
			}
			else if (Input.GetKeyDown(KeyCode.F2))
			{
				Debug.Log("Connecting");
				GameObject.Instantiate(this.ClientControl);
			}
		}
	}

//	private void OnServerInitialized()
//	{
//		this.createPlayer();
//	}
//
//	private void OnConnectedToServer()
//	{
//		this.createPlayer();
//	}
//
//	private void OnPlayerConnected(NetworkPlayer player)
//	{
//		//this.Players.Add(player);
//		//Debug.Log("New player connected.");
//	}
//
//	private void OnPlayerDisconnected(NetworkPlayer player)
//	{
//		//this.Players.Remove(player);
//
//		//Network.RemoveRPCs(player);
//		//Network.DestroyPlayerObjects(player);
//	}
//
//	private void OnDisconnectedFromServer(NetworkDisconnection info)
//	{
////		Network.RemoveRPCs(Network.player);
////		Network.DestroyPlayerObjects(Network.player);
////
////		Application.LoadLevel(Application.loadedLevel);
//	}
//
//	private void createPlayer()
//	{
//		//Network.Instantiate(this.PlayerPrefab, new Vector3(0, 10, 0), Quaternion.identity, 0);
//	}	

}
