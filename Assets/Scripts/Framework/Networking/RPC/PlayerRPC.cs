using UnityEngine;
using System.Collections;

public class PlayerRPC : RPCHolder {

	public static void NewPlayerJoined(NetworkPlayer networkPlayer, NetworkViewID id)
	{
		Debug.Log("Sending new player joined.");

		Channel.networkView.RPC("NewPlayerJoinedRPC", RPCMode.All, networkPlayer, id);
	}

	public static void NewPlayerJoined(NetworkPlayer target, NetworkPlayer networkPlayer, NetworkViewID id)
	{
		Debug.Log("Sending new player joined.");
		
		Channel.networkView.RPC("NewPlayerJoinedRPC", target, networkPlayer, id);
	}

	[RPC]
	private void NewPlayerJoinedRPC(NetworkPlayer networkPlayer, NetworkViewID id, NetworkMessageInfo info)
	{
		Debug.Log("New player joined RPC received!");
		
		Player player = new Player(id, networkPlayer);
		base.NetworkControl.Players.Add(id, player);
		base.ObjectTables.AddPlayerTable(player);

        if (Network.isClient)
		{
			if (networkPlayer.ipAddress == base.NetworkControl.LocalIP)
			{
				// Set own ID assigned by server.
				base.NetworkControl.LocalViewID = id;
			}

		}
	}
}
