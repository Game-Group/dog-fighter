using UnityEngine;
using System.Collections;
using System;

public class ObjectSync : NetworkObject 
{
    public ObjectSyncType Type { get; set; }

	public Player Owner { get; private set; }
	public int GlobalID { get; private set; }
	public bool IsIDAssigned { get; private set; }

    public bool IsOwner
    {
        get
        {
            if (GlobalSettings.SinglePlayer)
                return true;

            return base.NetworkControl.ThisPlayer.ID == this.Owner.ID;
        }
    }

	public void AssignID(Player owner, int globalID)
	{
		if (!this.IsIDAssigned)
		{
			this.Owner = owner;
			this.GlobalID = globalID;
			this.IsIDAssigned = true;
		}
		else
			throw new UnityException("IDs have already been assigned.");
	}

	public virtual void NetworkDestroy()
	{
		base.ObjectTables.RemovePlayerObject(this.Owner, this.GlobalID);
	}

	public override void Dispose()
	{
		base.NetworkControl.SyncTimeEvent -= this.SyncFunction;

		if (this.IsIDAssigned)
		{
			this.Owner = null;

			if (Network.isServer)
				base.GUIDGenerator.RecycleID(this.GlobalID);
		}

		base.Dispose();
	}

	protected virtual void SyncFunction()
	{
	}

	// Use this for initialization
	protected override void Start () {

		base.Start();

		if (Network.peerType != NetworkPeerType.Disconnected)		
			base.NetworkControl.SyncTimeEvent += this.SyncFunction;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}
}
