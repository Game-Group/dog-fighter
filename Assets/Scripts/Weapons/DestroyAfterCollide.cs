using UnityEngine;
using System.Collections;

public class DestroyAfterCollide : MonoBehaviour 
{
	void OnCollisionExit(Collision info) 
	{
		if (info.collider.tag != "Player")
			Destroy(gameObject);
	}
}
