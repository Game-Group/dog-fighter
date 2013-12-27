using UnityEngine;
using System.Collections;

public class ObjectRPC : RPCHolder 
{
	public GameObject PlayerSpawnPointPrefab;
    public GameObject MothershipPrefab;

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

    public static void ObjectPosition(Player player, int objectID, Vector3 position, Vector3 orientation)
    {
        Debug.Log ("Sending player ship position.");

        channel.networkView.RPC("ObjectPositionRPC", RPCMode.Others, player.ID, objectID, position, orientation);
    }
    public static void ObjectVelocity(Player player, int objectID, Vector3 transform, Vector3 rotation)
    {
        Debug.Log("Sending player ship velocity.");

        channel.networkView.RPC("ObjectVelocityRPC", RPCMode.Server, player.ID, objectID, transform, rotation);
    }

    public static void CreateMothership(NetworkPlayer target, Player owner, int objectID, int layer)
    {
        channel.CheckServer();

        channel.networkView.RPC("CreateMothershipRPC", target, owner.ID, objectID, layer);
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
    public static void KillObject(Player objectOwner, int objectID)
    {
        channel.CheckServer();

        Debug.Log("Sending KillObjectRPC.");

        channel.networkView.RPC("KillObjectRPC", RPCMode.Others, objectOwner.ID, objectID);
    }
    /// <summary>
    /// Send an RPC to all clients to respawn a certain object. May only be used by the server.
    /// This function currently only works for player ships.
    /// </summary>
    /// <param name="spawnPointOwner">The spawnpoint that will respawn the attatched player.</param>
    /// <param name="spawnPointID">The ID of the spawnpoint.</param>
    public static void RespawnObject(Player spawnPointOwner, int spawnPointID)
    {
        channel.CheckServer();

        Debug.Log("Sending RespawnObjectRPC.");

        channel.networkView.RPC("RespawnObjectRPC", RPCMode.Others, spawnPointOwner.ID, spawnPointID);
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
        //Debug.Log("SetObjectLayerRPC received.");

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
                    NetworkPrototypeLevel npl = new NetworkPrototypeLevel();
                    npl.MothershipPrefab = this.MothershipPrefab;
                    creator = npl;
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
    private void ObjectPositionRPC(NetworkViewID objectOwner, int objectID, Vector3 position, Vector3 orientation)
    {
        //		Debug.Log("Player ship position received.");

        GameObject obj = base.GetObject(objectOwner, objectID);
        obj.transform.position = position;
        obj.transform.eulerAngles = orientation;
    }

    [RPC]
    private void ObjectVelocityRPC(NetworkViewID objectOwner, int objectID, Vector3 translation, Vector3 rotation)
    {
        //		Debug.Log("Player ship velocity received.");		

        GameObject obj = base.GetObject(objectOwner, objectID);
        ObjectTransformer objectTransform = obj.GetComponent<ObjectTransformer>();

        objectTransform.Translation = translation;
        objectTransform.Rotation = rotation;
    }

	[RPC]
	private void CreatePlayerSpawnpointRPC(NetworkViewID owner, int objectID, Vector3 position, NetworkMessageInfo info)
	{		
//		Debug.Log("Create player spawn point RPC received.");

		GameObject spawnPoint = (GameObject)GameObject.Instantiate(this.PlayerSpawnPointPrefab, position, Quaternion.identity);
        ObjectSync spawnPointSync = spawnPoint.GetComponent<ObjectSync>();
        spawnPointSync.Type = ObjectSyncType.PlayerSpawnPoint;

        base.ObjectTables.PlayerObjects[base.Players[owner]].PlayerSpawnPointID = objectID;
		base.AddToObjectTables(spawnPoint, owner, objectID);
	}

    [RPC]
    private void CreateMothershipRPC(NetworkViewID owner, int objectID, int layer)
    {
        Debug.Log("Received CreateMothershipRPC");

        GameObject motherShip = (GameObject)GameObject.Instantiate(this.MothershipPrefab);
        motherShip.GetComponent<DroneSpawn>().enabled = false;

        base.AddToObjectTables(motherShip, owner, objectID);
        TeamHelper.IterativeLayerAssignment(motherShip.transform, layer);
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

    [RPC]
    private void KillObjectRPC(NetworkViewID objectOwner, int objectID)
    {
        Debug.Log("KillObjectRPC received.");

        GameObject obj = base.GetObject(objectOwner, objectID);
        HealthControl healthControl = obj.GetComponent<HealthControl>();
        healthControl.Die();
    }

    /// <summary>
    /// RPC to respawn a certain object. This function currently only works for player ships.
    /// </summary>
    /// <param name="spawnPointOwner">The spawnpoint that will respawn the attatched player.</param>
    /// <param name="spawnPointID">The ID of the spawnpoint.</param>
    [RPC]
    private void RespawnObjectRPC(NetworkViewID spawnPointOwner, int spawnPointID)
    {
        Debug.Log("RespawnObjectRPC received.");

        GameObject spawnPoint = base.GetObject(spawnPointOwner, spawnPointID);
        PlayerRespawner respawner = spawnPoint.GetComponent<PlayerRespawner>();

        respawner.Respawn();
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
