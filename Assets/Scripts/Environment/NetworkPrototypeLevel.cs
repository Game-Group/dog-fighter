using UnityEngine;
using System.Collections;

public class NetworkPrototypeLevel : LevelCreator {

	public override void CreateLevel ()
	{
		if (Network.peerType == NetworkPeerType.Server)
			this.createServerSideObjects();
	}

	private void createServerSideObjects()
	{
		Player server = base.NetworkControl.ThisPlayer;

		int spawnPointID = base.GUIDGenerator.GenerateID();
		Vector3 spawnPointPos = Vector3.zero;
		ObjectRPC.CreatePlayerSpawnpoint(server, spawnPointID, spawnPointPos);
	}
}
