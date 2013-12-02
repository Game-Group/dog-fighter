using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System;

public class NetworkControl : MonoBehaviour 
{
	public const string RPCChannelObject = "RPCChannel";

	public int ServerPort = 6500;
	public string ServerIP = "127.0.0.1";

	public int SyncRate
	{
		get { return this.syncRate; }
		set
		{
			if (value <= 0)
				throw new UnityException("Invalid value for sync rate. Must be larger than zero.");
			else
			{
				this.syncRate = value;
				this.timeForOneSync = 1 / (float)this.syncRate;
			}
		}
	}
	public event Action SyncTimeEvent;

	public string LocalIP { get; set; }
	public NetworkViewID LocalViewID { get; set; }

	//public IList<NetworkView> NetworkViews { get; private set; }

	public GameObject ServerControl;
	public GameObject ClientControl;

	public IDictionary<NetworkViewID, Player> Players { get; private set; }
	public Player ThisPlayer
	{
		get
		{
			return this.Players[this.networkView.viewID];
		}
	}
	
	// Use this for initialization
	public void Start () 
	{
		this.SyncRate = 15;

		// Find the local IP Address.
		IPAddress localAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
		byte[] bytes = localAddress.GetAddressBytes();

		this.LocalIP = 
			bytes[0].ToString() + "." + bytes[1].ToString() + "." + bytes[2].ToString() + "." + bytes[3].ToString();

//		this.NetworkViews = new List<NetworkView>(10);
//
//		this.AddNewNetworkView(Network.AllocateViewID());
//		this.AddNewNetworkView(Network.AllocateViewID());

		this.Players = new Dictionary<NetworkViewID, Player>(10);
	}
	
	// Update is called once per frame
	public void Update () 
	{
		if (!GameObject.Find("Global").GetComponent<GlobalSettings>().HasFocus)
			return;

		if (Network.peerType == NetworkPeerType.Disconnected)
		{		
			if (Input.GetKeyDown(KeyCode.F1))
			{
				Debug.Log ("Initializing as server.");
				GameObject.Instantiate(this.ServerControl);
			}
			else if (Input.GetKeyDown(KeyCode.F2))
			{
				Debug.Log("Connecting");
				GameObject.Instantiate(this.ClientControl);
			}
		}
	}

	public void LateUpdate()
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
			return;

//		Debug.Log("Late update!");

		this.elapsedTime += Time.deltaTime;

		if (this.elapsedTime > this.timeForOneSync)
		{
			if (this.SyncTimeEvent != null)
				this.SyncTimeEvent.Invoke();

			this.elapsedTime = 0;

//			Debug.Log("Sync moment!");

			// Make sure the elapsed time is lower than the timeForOneSync.
//			while (this.elapsedTime > this.timeForOneSync)
//				this.elapsedTime -= this.timeForOneSync;
		}

//		Debug.Log("Late update exit.");
	}

	private float elapsedTime;
	private int syncRate;
	private float timeForOneSync;

}
