using UnityEngine;
using System.Collections;

public class DestroyAndReloadOnCollideEnter : MonoBehaviour 
{
	void OnCollisionEnter(Collision info)
	{
		if (info.collider.tag != "PlayerProjectile")
		{
			Destroy (gameObject);
			Application.LoadLevel(Application.loadedLevelName);
		}
	}
}

