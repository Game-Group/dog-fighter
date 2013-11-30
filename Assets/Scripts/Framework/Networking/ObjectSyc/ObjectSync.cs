using UnityEngine;
using System.Collections;
using System;

public abstract class ObjectSync : MonoBehaviour 
{
	public Player Owner { get; private set; }
	public int GlobalID { get; private set; }
	public bool IsIDAssigned { get; private set; }

	public void AssignID(Player owner, int globalID)
	{
		if (!this.IsIDAssigned)
		{
			this.Owner = owner;
			this.GlobalID = globalID;
			this.IsIDAssigned = true;
		}
		else
			throw new UnityException("IDs cannot be reassigned.");
	}

	public virtual void NetworkDestroy()
	{
		this.ObjectTable.RemovePlayerObject(this.Owner, this.GlobalID);
	}

	public void Dispose()
	{
		this.NetworkControl.SyncTimeEvent -= this.SyncFunction;
		this.NetworkControl = null;

		if (this.IsIDAssigned)
		{
			this.Owner = null;

			if (Network.isServer)
			{
				GUIDGenerator g = this.NetworkControl.GetComponent<GUIDGenerator>();
				g.RecycleID(this.GlobalID);
			}
		}
	}

	protected NetworkControl NetworkControl { get; private set; }
	protected GameObject RPCChannel { get; private set; }
	protected PlayerObjectTable ObjectTable { get; private set; }

	protected abstract void SyncFunction();

	// Use this for initialization
	protected virtual void Start () {
		this.NetworkControl = GameObject.Find("NetworkControl").GetComponent<NetworkControl>();
		this.NetworkControl.SyncTimeEvent += this.SyncFunction;
		this.RPCChannel = (GameObject)GameObject.Find("RPCChannel");
		this.ObjectTable = GameObject.Find("PlayerObjectTable").GetComponent<PlayerObjectTable>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
