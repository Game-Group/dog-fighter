using UnityEngine;
using System.Collections;

public class PlayerShipSync : ObjectSync {

	protected override void Start ()
	{
		base.Start ();

		this.objectTransform = this.GetComponent<ObjectTransform>();
	}

	protected override void SyncFunction ()
	{
		if (Network.isServer)
		{
			Vector3 pos = this.transform.position;
			Vector3 orientation = this.transform.eulerAngles;

			PlayerShipRPC.PlayerShipPosition(base.Owner, base.GlobalID, pos, orientation);
		}
		else if (Network.isClient)
		{
			PlayerShipRPC.PlayerShipVelocity(base.Owner, base.GlobalID, this.objectTransform.Translation, this.objectTransform.Rotation);
		}

	}

	private ObjectTransform objectTransform;

}
