using UnityEngine;
using System.Collections;

public class HealthControl : MonoBehaviour 
{
	public PlayerRespawner RespawnPoint;
	public float RespawnDelay;

	public GameObject ExplosionGraphic;
	public AudioClip ExplosionSound;

	public float MaxHealth;
	public float MaxShields;
	public float HealthPerSecond;
	public float ShieldsPerSecond;

	public GameObject ShieldImpactPrefab;

	private float health;
	private float shieldStrength;

	void Start()
	{
		health = MaxHealth;
		shieldStrength = MaxShields;
		PlayerRespawner[] respawnpoints = FindObjectsOfType<PlayerRespawner>();
		foreach (PlayerRespawner spawner in respawnpoints)
			if (spawner.gameObject.layer == gameObject.layer)
			{
				RespawnPoint = spawner;
				break;
			}

		this.objSync = this.GetComponent<DestroyableObjectSync>();
	}

	public void OnEnable()
	{
		health = MaxHealth;
		shieldStrength = MaxShields;
	}

	void Update()
	{
		Heal(HealthPerSecond * Time.deltaTime, ShieldsPerSecond * Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.F7))
			TakeDamage(MaxHealth, MaxShields);
	}

	public float CurrentHealth 
	{
		get { return health; }
		set { this.health = value; }
	}
	public float CurrentShields 
	{
		get { return shieldStrength; }
		set { this.shieldStrength = value; }
	}

	#region TakeDamageMethods

	public void TakeDamage(float damage)
	{
		TakeDamage(Mathf.Max(0, damage - shieldStrength), damage);
	}

	public void TakeDamage(float damage, Vector3 impactPoint)
	{
		TakeDamage(Mathf.Max(0, damage - shieldStrength), damage, impactPoint);
	}
	
	public void TakeDamage(float hullDamage, float shieldDamage)
	{
		shieldStrength = Mathf.Max(0, shieldStrength - shieldDamage);

		health = Mathf.Max(0, health - hullDamage);;

		if (health <= 0)
			Die();
	}

	public void TakeDamage(float hullDamage, float shieldDamage, Vector3 impactPoint)
	{
		if (shieldStrength > 0)
		{
			GameObject shieldPrefabInstance = (GameObject)Instantiate(ShieldImpactPrefab, transform.position, transform.rotation);
			shieldPrefabInstance.transform.LookAt(impactPoint);
			shieldPrefabInstance.transform.parent = transform;
		}
		shieldStrength = Mathf.Max(0, shieldStrength - shieldDamage);
		
		health = Mathf.Max(0, health - hullDamage);;

		this.objSync.RequestHealthSync();
		
		if (health <= 0)
			Die();
	}

	#endregion

	public void Die()
	{
		GameObject explinst = Instantiate(ExplosionGraphic, gameObject.transform.position, Quaternion.identity) as GameObject;
		AudioSource.PlayClipAtPoint(ExplosionSound, gameObject.transform.position);

		RespawnPoint.DisableAndWaitForSpawn(RespawnDelay);
	}

	public void Heal(float hullHealing, float shieldHealing)
	{
		health = Mathf.Min(health + hullHealing, MaxHealth);
		shieldStrength = Mathf.Min(shieldStrength + shieldHealing, MaxShields);
	}

	public void OnGUI()
	{
		GUI.Label(new Rect(0, 0, 100, 100)
		          , "Health: " + health + "/" + MaxHealth + "\n"
		          + "Shields: " + shieldStrength + "/" + MaxShields);
	}

	private DestroyableObjectSync objSync;
}
