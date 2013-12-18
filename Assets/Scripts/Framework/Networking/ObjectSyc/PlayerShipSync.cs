using UnityEngine;
using System.Collections;

public class PlayerShipSync : DestroyableObjectSync 
{
	protected override void Start ()
	{
		base.Start ();

		this.objectTransformer = this.GetComponent<ObjectTransformer>();
	}

	protected override void SyncFunction ()
	{
		if (Network.isServer)
		{
			Vector3 pos = this.transform.position;
			Vector3 orientation = this.transform.eulerAngles;

			if (pos != this.previousPos || orientation != this.previousOrientation)
			{
				PlayerShipRPC.PlayerShipPosition(base.Owner, pos, orientation);
				this.previousPos = pos;
				this.previousOrientation = orientation;
			}
		}
		else if (Network.isClient)
		{
			Vector3 translation = this.objectTransformer.Translation;
			Vector3 rotation = this.objectTransformer.Rotation;

			if (translation != this.previousTranslation || rotation != this.previousRotation)
			{
				PlayerShipRPC.PlayerShipVelocity(base.Owner, base.GlobalID, translation, rotation);

				this.previousTranslation = translation;
				this.previousRotation = rotation;
			}
		}

	}

	private Vector3 previousPos;
	private Vector3 previousOrientation;
	private Vector3 previousTranslation;
	private Vector3 previousRotation;

	private ObjectTransformer objectTransformer;

}
