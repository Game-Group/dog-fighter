using UnityEngine;
using System.Collections;

public class RPCChannel : NetworkObject {

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();

		if (Network.isServer)
		{

			base.NetworkControl.LocalViewID = this.networkView.viewID;
			Player server = new Player(base.NetworkControl.LocalViewID, Network.player);
			base.NetworkControl.Players.Add(server.ID, server);
			base.ObjectTables.AddPlayerTable(server);
			Debug.Log("Created server player: " + server.ID);
		}

		this.name = NetworkControl.RPCChannelObject;
	
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

	protected override void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		if (Network.isServer)
			return;
		this.Start();
	}
}
