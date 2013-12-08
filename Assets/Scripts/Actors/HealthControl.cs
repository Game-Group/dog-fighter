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
		shieldStrength = Mathf.Max(0, shieldStrength - shieldDamage);

		health = Mathf.Max(0, health - hullDamage);;

		if (health <= 0)
			Die();
	}

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
}
