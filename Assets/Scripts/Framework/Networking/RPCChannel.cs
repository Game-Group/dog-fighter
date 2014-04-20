using UnityEngine;
using System.Collections;

public class RPCChannel : NetworkObject 
{
    protected override void Awake()
    {
        this.name = GlobalSettings.RPCChannelName;

        base.Awake();
    }

    protected void OnDestroy()
    {
        UnityEngine.Debug.Log("RPCChannel destroyed.");

        GameObject.Find(GlobalSettings.NetworkControlName).GetComponent<NetworkControl>().Shutdown();

        MatchControl matchControl = GameObject.Find(GlobalSettings.MatchControlName).GetComponent<MatchControl>();
        matchControl.ReturnToMenu();
    }

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();

		if (Network.isServer)
		{
			base.NetworkControl.LocalViewID = this.networkView.viewID;
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}
}
