using UnityEngine;
using System.Collections;

public class ShipCollision : MonoBehaviour {

	public GameObject destroyObject;
	public GameObject explosion;
	public AudioClip explosionSound;

	void OnCollisionEnter( Collision collision ){
		if( collision.gameObject.tag != "PlayerProjectile" ){
			GameObject explinst = Instantiate( explosion, destroyObject.transform.position, Quaternion.identity ) as GameObject;
			AudioSource.PlayClipAtPoint( explosionSound, destroyObject.transform.position );
			Destroy(destroyObject);
			//Destroy(gameObject);
			//Destroy( explinst, 3 );
		}
	}

}
