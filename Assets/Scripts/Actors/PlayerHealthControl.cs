using UnityEngine;
using System.Collections;

public class PlayerHealthControl : HealthControl 
{
    public bool DrawHealthInfo { get; set; }

	public PlayerRespawner RespawnPoint;
	public float RespawnDelay;

	public GameObject ExplosionGraphic;
	public AudioClip ExplosionSound;

    public float StatusTexture_Left;
    public float StatusTexture_Top;
    public float StatusTexture_Width;
    public float StatusTexture_Height;
    public Texture2D HullTexture;
    public Texture2D ShieldTexture;

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
