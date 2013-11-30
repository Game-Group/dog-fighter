using UnityEngine;
using System.Collections;

public class RPCHolder : MonoBehaviour 
{
	public RPCMode RPCMode { get; set; }

	// Use this for initialization
	protected virtual void Start () {

		this.ObjectTables = GameObject.Find("PlayerObjectTable").GetComponent<PlayerObjectTable>();
		this.NetworkControl = GameObject.Find("NetworkControl").GetComponent<NetworkControl>();		
		this.GUIDGenerator = this.NetworkControl.GetComponent<GUIDGenerator>();

		this.RPCMode = RPCMode.All;
	}

	protected PlayerObjectTable ObjectTables { get; private set; }
	protected NetworkControl NetworkControl { get; private set; }
	protected GUIDGenerator GUIDGenerator { get; private set; }

	protected void AddToObjectTables(GameObject obj, NetworkViewID playerID, int objectID)
	{
		ObjectSync objSync = obj.GetComponent<ObjectSync>();

		Player player = this.NetworkControl.Players[playerID];		

		objSync.AssignID(player, objectID);


		this.ObjectTables.AddPlayerObject(player, objectID, obj);
	}

	protected virtual void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		Debug.Log ("On Network Intantiate!");

		this.Start();
	}
}
