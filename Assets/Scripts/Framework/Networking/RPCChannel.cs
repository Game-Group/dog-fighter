using UnityEngine;
using System.Collections;

public class RPCChannel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.name = NetworkControl.RPCChannelObject;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		this.Start ();
	}
}
