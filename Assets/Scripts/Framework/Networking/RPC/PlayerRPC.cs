using UnityEngine;
using System.Collections;

public class PlayerRPC : RPCHolder {

	public static void NewPlayerJoined(NetworkPlayer networkPlayer, NetworkViewID id)
	{
		Debug.Log("Sending new player joined.");

		channel.networkView.RPC("NewPlayerJoinedRPC", channel.RPCMode, networkPlayer, id);
	}

	public static void NewPlayerJoined(NetworkPlayer target, NetworkPlayer networkPlayer, NetworkViewID id)
	{
		Debug.Log("Sending new player joined.");
		
		channel.networkView.RPC("NewPlayerJoinedRPC", target, networkPlayer, id);
	}

	[RPC]
	private void NewPlayerJoinedRPC(NetworkPlayer networkPlayer, NetworkViewID id, NetworkMessageInfo info)
	{
		Debug.Log("New player joined RPC received!");
		
		Player player = new Player(id, networkPlayer);
		base.NetworkControl.Players.Add(id, player);
		base.ObjectTables.AddPlayerTable(player);

		if (Network.isServer)
		{
//			PlayerShipRPC.CreatePlayerShip(player, channel.GUIDGenerator.GenerateID()); 
		}
		else if (Network.isClient)
		{
			if (networkPlayer.ipAddress == base.NetworkControl.LocalIP)
			{
//				Debug.Log ("It's me!");
				// Set own ID assigned by server.
//				this.networkControl.networkView.viewID = id;
				base.NetworkControl.LocalViewID = id;
			}

		}
	}

	private static PlayerRPC channel
	{
		get
		{
			if (channel_ == null)
				channel_ = GameObject.Find(NetworkControl.RPCChannelObject).GetComponent<PlayerRPC>();
			return channel_;
		}
	}
	
	private static PlayerRPC channel_;
}
