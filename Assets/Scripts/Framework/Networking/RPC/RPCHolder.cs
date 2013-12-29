using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class RPCHolder : NetworkObject 
{
	public RPCMode RPCMode { get; set; }

	// Use this for initialization
	protected override void Start () {

		base.Start();

		this.RPCMode = RPCMode.All;
	}

	protected void AddToObjectTables(GameObject obj, NetworkViewID playerID, int objectID)
	{
		ObjectSync objSync = obj.GetComponent<ObjectSync>();

		Player player = this.NetworkControl.Players[playerID];

		objSync.AssignID(player, objectID);

		this.ObjectTables.AddPlayerObject(player, objectID, obj);
	}

    [MethodImpl(MethodImplOptions.NoInlining)]
	protected void CheckServer()
	{
        if (Network.peerType != NetworkPeerType.Server)
        {
            string callerName = new StackFrame(1, true).GetMethod().Name;

            throw new UnityException("Only the server may use this function: " + callerName);
        }
	}


}
