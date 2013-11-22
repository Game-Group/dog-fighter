using UnityEngine;
using System.Collections;

public class ObjectRPC : MonoBehaviour {

	public GameObject PlayerPrefab;

	// Use this for initialization
	void Start () {
		this.table = GameObject.Find("PlayerObjectTable").GetComponent<PlayerObjectTable>();
		this.networkControl = GameObject.Find("NetworkControl").GetComponent<NetworkControl>();		
	}

	[RPC]
	public void CreatePlayerShip(NetworkPlayer player, NetworkViewID id, NetworkMessageInfo info)
	{
		Debug.Log("Create player ship RPC received!");

		GameObject obj = (GameObject)GameObject.Instantiate(this.PlayerPrefab);

		if (this.networkControl.LocalViewID == id)
		{
			Debug.Log("Yes!");
			obj.GetComponentInChildren<Camera>().enabled = true;
		}
		else
		{
			obj.GetComponentInChildren<Camera>().enabled = false;
			obj.GetComponentInChildren<AudioListener>().enabled = false;
			obj.GetComponentInChildren<ShipOrientation>().enabled = false;			
			obj.GetComponent<ShipControl>().enabled = false;
		}
	}

	private PlayerObjectTable table;
	private NetworkControl networkControl;
	
}
