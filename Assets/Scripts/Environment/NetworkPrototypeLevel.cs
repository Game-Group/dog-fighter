using UnityEngine;
using System.Collections;

public class NetworkPrototypeLevel : LevelCreator {

	public override void CreateLevel ()
	{
		if (Network.peerType == NetworkPeerType.Server)
			this.createServerSideObjects();
	}

	public override void SyncNewPlayer(Player newPlayer)
	{
		base.SyncNewPlayer(newPlayer);

//		Debug.Log("Syncing new player.");
//		Debug.Log(base.Players.Values.Count);

		foreach (Player p in base.Players.Values)
		{
//			Debug.Log("Found new player with ID: " + p.ID);

			if (!(p.ID == base.NetworkControl.LocalViewID || p.ID == newPlayer.ID))
			{
//				Debug.Log("Syncing this player.");

				PlayerObjects playerObjects = base.ObjectTables.PlayerObjects[p];			

				GameObject playerSpawnPoint = base.ObjectTables.GetPlayerObject(p, playerObjects.PlayerSpawnPointID);
				
//				Debug.Log("Got spawn point.");	

				ObjectRPC.CreatePlayerSpawnpoint(
					newPlayer.NetworkPlayerInfo, p, playerObjects.PlayerSpawnPointID, playerSpawnPoint.transform.position
					); 				

				PlayerShipRPC.CreatePlayerShip(newPlayer.NetworkPlayerInfo, p, playerObjects.PlayerShipID);
			}
			else
			{
//				Debug.Log("Skipped server player.");				
			}
		}

//		Debug.Log("Done syncing old players.");

		int spawnPointID = base.GUIDGenerator.GenerateID();
		int playerShipID = base.GUIDGenerator.GenerateID();
		Vector3 spawnPointPos = Vector3.zero;
		ObjectRPC.CreatePlayerSpawnpoint(newPlayer, spawnPointID, spawnPointPos);
		PlayerShipRPC.CreatePlayerShip(newPlayer, playerShipID);		
		PlayerShipRPC.SpawnPlayerShip(newPlayer, spawnPointID, playerShipID);	                              

	}

	private void createServerSideObjects()
	{
//		Player server = base.NetworkControl.ThisPlayer;
//
//		int spawnPointID = base.GUIDGenerator.GenerateID();
//		Vector3 spawnPointPos = Vector3.zero;
//		ObjectRPC.CreatePlayerSpawnpoint(server, spawnPointID, spawnPointPos);
	}
}
