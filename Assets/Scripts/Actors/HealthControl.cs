using UnityEngine;
using System.Collections;

public class HealthControl : MonoBehaviour 
{
	public Respawner RespawnPoint;
	public float RespawnDelay;

	public float MaxHealth;
	public float MaxShields;
	public float HealthPerSecond;
	public float ShieldsPerSecond;

	private float health;
	private float shieldStrength;

	void Start()
	{
		health = MaxHealth;
		shieldStrength = MaxShields;
		RespawnPoint = FindObjectOfType<Respawner>();
	}

	public void OnEnable()
	{
		health = MaxHealth;
		shieldStrength = MaxShields;
	}

	void Update()
	{
		Heal(HealthPerSecond * Time.deltaTime, ShieldsPerSecond * Time.deltaTime);
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
			Die();
	}

	public void Die()
	{
		RespawnPoint.DisableAndRespawn(gameObject, RespawnDelay);
	}

	public void Heal(float hullHealing, float shieldHealing)
	{
		health = Mathf.Max(health + hullHealing, MaxHealth);
		shieldStrength = Mathf.Max(shieldStrength + shieldHealing, MaxShields);
	}
}
