using UnityEngine;
using System.Collections;

public class ObjectRPC : RPCHolder 
{
	public GameObject PlayerSpawnPoint;

    #region Calls

    public static void SetObjectTag(Player objectOwner, int objectID, string tag)
    {
        channel.CheckServer();

        channel.networkView.RPC("SetObjectTagRPC", RPCMode.All, objectOwner.ID, objectID, tag);
    }
    public static void SetObjectLayer(Player objectOwner, int objectID, Layers layer)
    {
        channel.CheckServer();

        channel.networkView.RPC("SetObjectLayerRPC", RPCMode.All, objectOwner.ID, objectID, (int)layer);
    }
    public static void SetObjectLayer(NetworkPlayer target, Player objectOwner, int objectID, Layers layer)
    {
        channel.CheckServer();

        channel.networkView.RPC("SetObjectLayerRPC", target, objectOwner.ID, objectID, (int)layer);
    }

    public static void LoadLevel(int levelID)
	{
		channel.CheckServer();

		channel.networkView.RPC("LoadLevelRPC", RPCMode.All, levelID);
	}

	public static void CreatePlayerSpawnpoint(Player owner, int objectID, Vector3 position)
	{
		channel.CheckServer();

		channel.networkView.RPC("CreatePlayerSpawnpointRPC", RPCMode.All, owner.ID, objectID, position);
	}
	public static void CreatePlayerSpawnpoint(NetworkPlayer target, Player owner, int objectID, Vector3 position)
	{
		channel.CheckServer();		
		
		channel.networkView.RPC("CreatePlayerSpawnpointRPC", target, owner.ID, objectID, position);
	}

	public static void SetObjectHealth(Player objectOwner, int objectID, float health, float shields)
	{
		channel.CheckServer();

        Debug.Log("Sending SetObjectHealthRPC");

		channel.networkView.RPC("SetObjectHealthRPC", RPCMode.Others, objectOwner.ID, objectID, health, shields);
	}

    #endregion

    #region RPCs
    [RPC]
    private void SetObjectTagRPC(NetworkViewID owner, int objectID, string tag)
    {
        GameObject obj = base.GetObject(owner, objectID);
        obj.tag = tag;
    }
    [RPC]
    private void SetObjectLayerRPC(NetworkViewID owner, int objectID, int layer)
    {
        Debug.Log("SetObjectLayerRPC received.");

        GameObject obj = base.GetObject(owner, objectID);
        obj.name = "Player_" + layer;
        obj.layer = layer;
        TeamHelper.IterativeLayerAssignment(obj.transform, layer);
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

		if (Network.peerType == NetworkPeerType.Server)
		{
			GameObject.Find("ServerControl").GetComponent<ServerControl>().ChangeLevel(creator);
		}
		else if (Network.peerType == NetworkPeerType.Client)
		{
			GameObject.Find("ClientControl").GetComponent<ClientControl>().ChangeLevel(creator);
		}
	}

	[RPC]
	private void CreatePlayerSpawnpointRPC(NetworkViewID owner, int objectID, Vector3 position, NetworkMessageInfo info)
	{		
//		Debug.Log("Create player spawn point RPC received.");
		base.ObjectTables.PlayerObjects[base.Players[owner]].PlayerSpawnPointID = objectID;

		Object spawnPoint = GameObject.Instantiate(this.PlayerSpawnPoint, position, Quaternion.identity);
		base.AddToObjectTables((GameObject)spawnPoint, owner, objectID);
	}

	[RPC]
	private void SetObjectHealthRPC(NetworkViewID objectOwner, int objectID, float health, float shields)
	{
        Debug.Log("SetObjectHealthRPC received.");

		GameObject obj = base.GetObject(objectOwner, objectID);
		HealthControl healthControl = obj.GetComponent<HealthControl>();

		healthControl.CurrentHealth = health;
		healthControl.CurrentShields = shields;
	}
    #endregion

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
