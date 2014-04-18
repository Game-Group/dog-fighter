using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System;

public class NetworkControl : MonoBehaviour 
{
    /// <summary>
    /// The amount of times the SyncTimeEvent is called each second.
    /// </summary>
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

	public GameObject ServerControl;
	public GameObject ClientControl;

	public IDictionary<NetworkViewID, Player> Players { get; private set; }
	public Player ThisPlayer
	{
		get
		{
			return this.Players[this.LocalViewID];
		}
	}

    private void Awake()
    {
        this.name = GlobalSettings.NetworkControlName;
    }
	
	// Use this for initialization
	private void Start () 
	{
		this.SyncRate = 20;

		// Find the local IP Address.
		IPAddress localAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
		byte[] bytes = localAddress.GetAddressBytes();

		this.LocalIP = 
			bytes[0].ToString() + "." + bytes[1].ToString() + "." + bytes[2].ToString() + "." + bytes[3].ToString();

		this.Players = new Dictionary<NetworkViewID, Player>(10);

        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            if (GlobalSettings.IsServer)
            {
                Debug.Log("Initializing as server.");
                GameObject.Instantiate(this.ServerControl);
            }
            else
            {
                Debug.Log("Connecting");
                GameObject.Instantiate(this.ClientControl);
            }
        }
	}
	
	// Update is called once per frame
	private void Update () 
	{
		if (!GlobalSettings.HasFocus)
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

	private void LateUpdate()
	{
        //Debug.Log("Late update!");

		if (Network.peerType == NetworkPeerType.Disconnected)
			return;

        //Debug.Log("Past check.");

		this.elapsedTime += Time.deltaTime;

		if (this.elapsedTime > this.timeForOneSync)
		{
            if (this.SyncTimeEvent != null)
            {
                //Debug.Log("Sync moment!");
                this.SyncTimeEvent.Invoke();
            }

			this.elapsedTime = 0;

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
