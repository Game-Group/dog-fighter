using UnityEngine;
using System.Collections;

public class NetworkInit : MonoBehaviour 
{
	public int Port = 6500;
	public string IP = "127.0.0.1";
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!GameObject.Find("Global").GetComponent<GlobalSettings>().HasFocus)
			return;			
		
		if (Input.GetKeyDown(KeyCode.F1))
		{
			Network.InitializeServer(1, this.Port, false);
			Debug.Log("Initialized as server.");
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			Debug.Log("Connecting");
			Network.Connect(this.IP, this.Port);
			Debug.Log("Connected!");
		}		
	}
	
}
