using UnityEngine;
using System.Collections;

public class PlayerHealthControl : HealthControl 
{

	public PlayerRespawner RespawnPoint;
	public float RespawnDelay;

	public GameObject ExplosionGraphic;
	public AudioClip ExplosionSound;

    protected override void Initialize()
    {
        PlayerRespawner[] respawnpoints = FindObjectsOfType<PlayerRespawner>();
        foreach (PlayerRespawner spawner in respawnpoints)
            if (spawner.gameObject.layer == gameObject.layer)
            {
                RespawnPoint = spawner;
                break;
            }
    }

	public override void Die()
	{
        if (Network.peerType != NetworkPeerType.Server)
        {
            Instantiate(ExplosionGraphic, gameObject.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(ExplosionSound, gameObject.transform.position);
        }

		RespawnPoint.DisableAndWaitForSpawn(RespawnDelay);
	}
}
