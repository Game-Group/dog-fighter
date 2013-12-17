using UnityEngine;
using System.Collections;

public class ServerControl : NetworkObject 
{
	public Object RPCChannelPrefab;

	public LevelCreator StartingLevel { get; set; }

	// Use this for initialization
	protected override void Start ()
	{
		base.Start();

		this.StartingLevel = new NetworkPrototypeLevel();

		Network.InitializeServer(10, base.NetworkControl.ServerPort, false);
	}

	private void OnServerInitialized()
	{
		// Network Initialize the RPCChannel to agree on a NetworkView with all clients.
		Network.Instantiate(this.RPCChannelPrefab, Vector3.zero, Quaternion.identity, 0);

		Player server = new Player(base.NetworkControl.LocalViewID, Network.player);
		base.NetworkControl.Players.Add(server.ID, server);
		base.ObjectTables.AddPlayerTable(server);

		ObjectRPC.LoadLevel(0);
	}

	private void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
		Debug.Log("A new player has joined.");
		Debug.Log("Current number of players: " + base.Players.Count);

		foreach (Player p in base.Players.Values)
		{
			PlayerRPC.SingleNewPlayerJoined(networkPlayer, p.NetworkPlayerInfo, p.ID);

			if (p.ID != base.NetworkControl.LocalViewID)
			{
				GameObject playerShip = base.ObjectTables.PlayerShips[p];
				ObjectSync objsync = playerShip.GetComponent<ObjectSync>();
				PlayerShipRPC.SinglCreatePlayerShip(networkPlayer, p, objsync.GlobalID);
			}
		}

		// Generate a viewID for the new player.
		NetworkViewID viewID = Network.AllocateViewID();

		// Notice everyone that the new player has joined.
		PlayerRPC.NewPlayerJoined(networkPlayer, viewID);
		PlayerShipRPC.CreatePlayerShip(base.Players[viewID], base.GUIDGenerator.GenerateID());		
	}
}
