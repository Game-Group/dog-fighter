using UnityEngine;
using System.Collections;

public class NetworkSync : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Debug.Log("Writing.");
			
			Vector3 pos = this.transform.position;
			Vector3 orientation = this.transform.eulerAngles;
			
			stream.Serialize(ref pos);
			stream.Serialize(ref orientation);
		}
		else
		{
			Debug.Log("Receiving.");
			
			Vector3 pos = Vector3.zero;
			Vector3 orientation = Vector3.zero;
			
			stream.Serialize(ref pos);
			stream.Serialize(ref orientation);
			
			this.transform.position = pos;
			this.transform.eulerAngles = orientation;
		}
	}
}
