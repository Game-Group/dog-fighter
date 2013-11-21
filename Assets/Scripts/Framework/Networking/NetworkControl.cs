using UnityEngine;
using System.Collections;

public class NetworkControl : MonoBehaviour 
{
	public const int Port = 6500;
	public const string IP = "127.0.0.1";

	public NetworkView UnreliableNetworkView { get; private set; }
	public NetworkView RPCNetworkView { get; private set;}

	public GameObject PlayerPrefab;
	
	// Use this for initialization
	public void Start () 
	{
		NetworkView unreliableNetworkView = this.gameObject.AddComponent<NetworkView>();
		unreliableNetworkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
		unreliableNetworkView.observed = this;
		this.UnreliableNetworkView = unreliableNetworkView;

		NetworkView rpcNetworkView = this.gameObject.AddComponent<NetworkView>();
		rpcNetworkView.stateSynchronization = NetworkStateSynchronization.Off;
		rpcNetworkView.observed = this;
		this.RPCNetworkView = rpcNetworkView;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		if (!GameObject.Find("Global").GetComponent<GlobalSettings>().HasFocus)
		{
			Debug.Log ("whut");
			return;		
		}
		
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
