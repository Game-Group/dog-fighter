using UnityEngine;
using System.Collections;

public class NetworkInit : MonoBehaviour 
{
	public const int Port = 6500;
	public const string IP = "127.0.0.1";

	public GameObject PlayerPrefab;
	
	// Use this for initialization
	public void Start () 
	{
		//this.playerPrefab = Resources.Load("Prefabs/Actors/PlayerSpaceShip 1");
		//Debug.Log(this.playerPrefab);
	}
	
	// Update is called once per frame
	public void Update () 
	{
		if (!GameObject.Find("Global").GetComponent<GlobalSettings>().HasFocus)
			return;			
		
		if (Input.GetKeyDown(KeyCode.F1))
		{
			Debug.Log ("Initializing as server.");
			Network.InitializeServer(1, Port, false);
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			Debug.Log("Connecting");
			Network.Connect(IP, Port);
		}		
	}

	private void OnServerInitialized()
	{
		this.createPlayer();
	}

	private void OnConnectedToServer()
	{
		this.createPlayer();
	}

	private void OnPlayerConnected()
	{
		Debug.Log("New player connected.");
	}

	private void createPlayer()
	{
		Network.Instantiate(this.PlayerPrefab, new Vector3(0, 10, 0), Quaternion.identity, 0);
	}	

}
