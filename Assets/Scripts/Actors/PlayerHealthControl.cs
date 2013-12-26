using UnityEngine;
using System.Collections;

public class PlayerHealthControl : HealthControl 
{
    public bool DrawHealthInfo { get; set; }

	public PlayerRespawner RespawnPoint;
	public float RespawnDelay;

	public GameObject ExplosionGraphic;
	public AudioClip ExplosionSound;

    private void Awake()
    {
        this.DrawHealthInfo = true;
    }

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
        // Notify others that the object should start 'dying'.
        if (!GlobalSettings.SinglePlayer)
            ObjectRPC.KillObject(this.objSync.Owner, this.objSync.GlobalID);

		GameObject explinst = Instantiate(ExplosionGraphic, gameObject.transform.position, Quaternion.identity) as GameObject;
		AudioSource.PlayClipAtPoint(ExplosionSound, gameObject.transform.position);

		RespawnPoint.DisableAndWaitForSpawn(RespawnDelay);
	}
}
