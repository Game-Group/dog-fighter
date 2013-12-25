using UnityEngine;
using System.Collections;

public class HealthControl : MonoBehaviour 
{
    public bool DrawHealthInfo { get; set; }

	public PlayerRespawner RespawnPoint;
	public float RespawnDelay;

	public GameObject ExplosionGraphic;
	public AudioClip ExplosionSound;

	public float MaxHealth;
	public float MaxShields;
	public float HealthPerSecond;
	public float ShieldsPerSecond;
    public float ShieldRechargeDelay;

	public GameObject ShieldImpactPrefab;

    public float StatusTexture_Left;
    public float StatusTexture_Top;
    public float StatusTexture_Width;
    public float StatusTexture_Height;
    public Texture2D HullTexture;
    public Texture2D ShieldTexture;

	private float health;
	private float shieldStrength;
    private float currentShieldDelay;

    private void Awake()
    {
        this.DrawHealthInfo = true;
    }

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
        currentShieldDelay = Mathf.Max(currentShieldDelay - Time.deltaTime, 0);

		//Heal(HealthPerSecond * Time.deltaTime, ShieldsPerSecond * Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.F7))
			TakeDamage(MaxHealth, MaxShields);
	}

    void OnGUI()
    {
        if (!this.DrawHealthInfo)
            return;

        Color hullColor = Color.Lerp(Color.red, Color.green, health / MaxHealth);

        GUI.color = hullColor;
        GUI.Label(
            new Rect(StatusTexture_Left, StatusTexture_Top, StatusTexture_Width, StatusTexture_Height),
            new GUIContent(HullTexture));

        Color shieldColor = Color.white;
        shieldColor.a = shieldStrength / MaxShields;

        GUI.color = shieldColor;
        GUI.Label(
             new Rect(StatusTexture_Left, StatusTexture_Top, StatusTexture_Width, StatusTexture_Height),
             new GUIContent(ShieldTexture));

        GUI.color = Color.white;
        GUI.Label(
                new Rect(StatusTexture_Left + StatusTexture_Width + 10, StatusTexture_Top, StatusTexture_Width, StatusTexture_Height),
                new GUIContent("Hull: " + health + "/" + MaxHealth));
        GUI.Label(
                new Rect(StatusTexture_Left + StatusTexture_Width + 10, StatusTexture_Top + 20, StatusTexture_Width, StatusTexture_Height),
                new GUIContent("Shields: " + shieldStrength + "/" + MaxShields));

    }

	public float CurrentHealth 
	{
		get { return health; }
        set
        {
            this.health = value;
            //if (!this.CheckIfAlive())
            //    this.Die();
        }
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
        if (Network.peerType != NetworkPeerType.Server)
            return;

        currentShieldDelay = ShieldRechargeDelay;
		shieldStrength = Mathf.Max(0, shieldStrength - shieldDamage);

		health = Mathf.Max(0, health - hullDamage);

        this.objSync.RequestHealthSync();

		if (!this.CheckIfAlive())
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

        TakeDamage(hullDamage, shieldDamage);
	}

	#endregion

    public bool CheckIfAlive()
    {
        return this.health > 0;
    }

	public void Die()
	{
        // Notify others that the object should start 'dying'.
        ObjectRPC.KillObject(this.objSync.Owner, this.objSync.GlobalID);

		GameObject explinst = Instantiate(ExplosionGraphic, gameObject.transform.position, Quaternion.identity) as GameObject;
		AudioSource.PlayClipAtPoint(ExplosionSound, gameObject.transform.position);

		RespawnPoint.DisableAndWaitForSpawn(RespawnDelay);
	}

    public void Heal(float hullHealing, float shieldHealing, bool ignoreShieldDelay = true)
    {
        health = Mathf.Min(health + hullHealing, MaxHealth);
        if (ignoreShieldDelay || currentShieldDelay <= 0)
            shieldStrength = Mathf.Min(shieldStrength + shieldHealing, MaxShields);
    }

	private DestroyableObjectSync objSync;
}
