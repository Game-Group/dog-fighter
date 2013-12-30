using UnityEngine;
using System.Collections.Generic;

public class NetworkObject : MonoBehaviour 
{
	public virtual void Dispose()
	{	
	}

    protected PlayerObjectTable ObjectTables
    {
        get
        {
            if (this.objectTable == null)
                this.objectTable = GameObject.Find(GlobalSettings.PlayerObjectTableName).GetComponent<PlayerObjectTable>();
            return this.objectTable;
        }
    }
    protected NetworkControl NetworkControl
    {
        get
        {
            if (this.networkControl == null)
                this.networkControl = GameObject.Find(GlobalSettings.NetworkControlName).GetComponent<NetworkControl>();
            return this.networkControl;
        }
    }
    protected GUIDGenerator GUIDGenerator
    {
        get
        {
            if (this.guidGenerator == null)
                this.guidGenerator = this.NetworkControl.GetComponent<GUIDGenerator>();
            return this.guidGenerator;
        }
    }
    protected GameObject RPCChannel
    {
        get
        {
            if (this.rpcChannel == null)
                this.rpcChannel = (GameObject)GameObject.Find(GlobalSettings.RPCChannelName);
            return this.rpcChannel;
        }
    }

	protected IDictionary<NetworkViewID, Player> Players 
	{
		get
		{
			return this.NetworkControl.Players;
		}
	}

	protected GameObject GetObject(NetworkViewID owner, int objectID)
	{
		Player player = this.Players[owner];
        return this.GetObject(player, objectID);
	}
    protected GameObject GetObject(Player player, int objectID)
    {
        return this.ObjectTables.GetPlayerObject(player, objectID);
    }
    protected GameObject GetPlayerShip(NetworkViewID playerID)
    {
        Player player = this.NetworkControl.Players[playerID];

        int playerShipID = this.ObjectTables.PlayerObjects[player].PlayerShipID;

        return this.ObjectTables.GetPlayerObject(player, playerShipID);
    }

    protected virtual void Awake()
    {
        
    }

	// Use this for initialization
	protected virtual void Start () 
    {
        //if (!GlobalSettings.SinglePlayer)
        //{
        //    Debug.Log("Finding important objects in NetworkObject");
        //    this.ObjectTables = GameObject.Find(GlobalSettings.PlayerObjectTableName).GetComponent<PlayerObjectTable>();
        //    this.NetworkControl = GameObject.Find(GlobalSettings.NetworkControlName).GetComponent<NetworkControl>();
        //    this.GUIDGenerator = this.NetworkControl.GetComponent<GUIDGenerator>();
        //    this.RPCChannel = (GameObject)GameObject.Find(GlobalSettings.RPCChannelName);

        //    if (this.ObjectTables == null || this.NetworkControl == null || this.GUIDGenerator == null || this.RPCChannel == null)
        //        throw new UnityException("Some object was not found");
        //}	
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnNetworkInstantiate(NetworkMessageInfo info)
	{
        //Debug.Log("On Network Intantiate!");

        this.Awake();
		//this.Start();
	}

    private PlayerObjectTable objectTable;
    private NetworkControl networkControl;
    private GUIDGenerator guidGenerator;
    private GameObject rpcChannel;
}
