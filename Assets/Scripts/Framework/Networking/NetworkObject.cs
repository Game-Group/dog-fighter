using UnityEngine;
using System.Collections.Generic;

public class NetworkObject : MonoBehaviour {

	public virtual void Dispose()
	{
		this.NetworkControl = null;
		
	}

	protected PlayerObjectTable ObjectTables { get; private set; }
	protected NetworkControl NetworkControl { get; private set; }
	protected GUIDGenerator GUIDGenerator { get; private set; }
	protected GameObject RPCChannel { get; private set; }

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

	// Use this for initialization
	protected virtual void Start () {

		if (!GlobalSettings.SinglePlayer)
		{
			this.ObjectTables = GameObject.Find("PlayerObjectTable").GetComponent<PlayerObjectTable>();
			this.NetworkControl = GameObject.Find("NetworkControl").GetComponent<NetworkControl>();		
			this.GUIDGenerator = this.NetworkControl.GetComponent<GUIDGenerator>();
			this.RPCChannel = (GameObject)GameObject.Find(NetworkControl.RPCChannelObject);
		}
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnNetworkInstantiate(NetworkMessageInfo info)
	{
//		Debug.Log ("On Network Intantiate!");
		
		this.Start();
	}
}
