using UnityEngine;
using System.Collections;

public class PlayerRPC : MonoBehaviour {

	// Use this for initialization
	void Start () {

		this.table = GameObject.Find("PlayerObjectTable").GetComponent<PlayerObjectTable>();
		this.networkControl = GameObject.Find("NetworkControl").GetComponent<NetworkControl>();
	
	}

	[RPC]
	public void NewPlayerJoined(NetworkPlayer networkPlayer, NetworkViewID id, NetworkMessageInfo info)
	{
		Debug.Log("New player joined RPC received!");

		if (Network.isClient)
		{
			if (networkPlayer.ipAddress == this.networkControl.LocalIP)
			{
				// Set own ID assigned by server.
//				this.networkControl.networkView.viewID = id;
				this.networkControl.LocalViewID = id;
			}

			Player player = new Player(id, networkPlayer);
			this.networkControl.Players.Add(id, player);
		}
	}

	private PlayerObjectTable table;
	private NetworkControl networkControl;
	
}
