using UnityEngine;
using System.Collections;

public class HealthControl : MonoBehaviour 
{
	public Respawner RespawnPoint;
	public float RespawnDelay;

	public GameObject ExplosionGraphic;
	public AudioClip ExplosionSound;

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
		TakeDamage(Mathf.Max(0, damage - shieldStrength), damage);
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
		GameObject explinst = Instantiate(ExplosionGraphic, gameObject.transform.position, Quaternion.identity) as GameObject;
		AudioSource.PlayClipAtPoint(ExplosionSound, gameObject.transform.position);

		RespawnPoint.DisableAndRespawn(gameObject, RespawnDelay);
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
}
