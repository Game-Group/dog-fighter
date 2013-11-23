using UnityEngine;
using System.Collections;

public class HealthAndRespawnControl : MonoBehaviour 
{
	public GameObject RespawningObjectPrefab;
	public Transform RespawnPoint;
	public float RespawnDelay;

	public float MaxHealth;
	public float MaxShields;
	public float HealthPerSecond;
	public float ShieldsPerSecond;

	private float health;
	private float shieldStrength;
	private GameObject CurrentObjectIntance;
	private bool waitingForRespawn;
	private float respawnTimer;

	void Start()
	{
		Respawn();
	}

	void Update()
	{
		if (waitingForRespawn)
		{
			respawnTimer -= Time.deltaTime;
			if (respawnTimer <= 0)
			{
				Respawn();
			}
		}
		else
		{
			Heal(HealthPerSecond * Time.deltaTime, ShieldsPerSecond * Time.deltaTime);
		}
	}

	/// <summary>
	/// Changing health goes through the TakeDamage and Heal methods.
	/// </summary>
	/// <value>The current health of this object.</value>
	public float CurrentHealth 
	{
		get { return health; }
	}

	/// <summary>
	/// Changing shields goes through the TakeDamage and Heal methods.
	/// </summary>
	/// <value>The current shields of this object.</value>
	public float CurrentShields 
	{
		get { return shieldStrength; }
	}

	public void TakeDamage(float damage)
	{
		float shieldOverkill = damage - shieldStrength;

		TakeDamage(Mathf.Min(0, shieldOverkill), damage);
	}
	
	public void TakeDamage(float hullDamage, float shieldDamage)
	{
		shieldStrength = Mathf.Min(0, shieldDamage);

		health -= hullDamage;
		if (health <= 0)
			InitializeDeath();
	}

	public void InitializeDeath()
	{
		Destroy(CurrentObjectIntance);
		waitingForRespawn = true;
		respawnTimer = RespawnDelay;
	}

	private void Respawn()
	{
		CurrentObjectIntance = (GameObject)Instantiate(RespawningObjectPrefab, RespawnPoint.position, RespawnPoint.rotation);

		health = MaxHealth;
		shieldStrength = MaxShields;

		waitingForRespawn = false;
	}

	public void Heal(float hullHealing, float shieldHealing)
	{
		health = Mathf.Max(health + hullHealing, MaxHealth);
		shieldStrength = Mathf.Max(shieldStrength + shieldHealing, MaxShields);
	}
}
