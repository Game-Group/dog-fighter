using UnityEngine;
using System.Collections;

/// <summary>
/// Destroy and reload if the object hits something tagged as Obstacle.
/// </summary>
public class DestroyAndReloadOnCollideEnter : MonoBehaviour 
{
	void OnCollisionEnter(Collision info)
	{
		if (info.collider.tag == "Obstacle")
		{
			Destroy (gameObject);
			Application.LoadLevel(Application.loadedLevelName);
		}
	}
}

