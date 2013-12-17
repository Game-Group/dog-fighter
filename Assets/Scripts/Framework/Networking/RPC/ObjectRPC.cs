using UnityEngine;
using System.Collections;

public class ObjectRPC : RPCHolder 
{
	public GameObject PlayerSpawnPoint;

	public static void LoadLevel(int levelID)
	{
		channel.networkView.RPC("LoadLevelRPC", RPCMode.All, levelID);
	}

	public static void CreatePlayerSpawnpoint(Player owner, int objectID, Vector3 position)
	{
		if (Network.peerType != NetworkPeerType.Server)
			throw new UnityException("Only the server may use this function.");

		channel.networkView.RPC("CreatePlayerSpawnpointRPC", RPCMode.All, owner.ID, objectID, position);
	}

	[RPC]
	private void LoadLevelRPC(int levelID, NetworkMessageInfo info)
	{
		LevelCreator creator = null;
		
		switch (levelID)
		{
		case 0:
			creator = new NetworkPrototypeLevel();
			break;
		}
		
		creator.CreateLevel();
	}

	[RPC]
	private void CreatePlayerSpawnpointRPC(NetworkViewID owner, int objectID, Vector3 position, NetworkMessageInfo info)
	{		
		Object spawnPoint = GameObject.Instantiate(this.PlayerSpawnPoint, position, Quaternion.identity);
		base.AddToObjectTables((GameObject)spawnPoint, owner, objectID);
	}
	
	private static ObjectRPC channel
	{
		get
		{
			if (channel_ == null)
				channel_ = GameObject.Find(NetworkControl.RPCChannelObject).GetComponent<ObjectRPC>();
			return channel_;
		}
	}
		
	private static ObjectRPC channel_;
}
