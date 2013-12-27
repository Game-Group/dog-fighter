using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour 
{
    public Camera Camera;

    public PlayerHealthControl HealthControl;

    public float StatusTexture_Left;
    public float StatusTexture_Top;
    public float StatusTexture_Width;
    public float StatusTexture_Height;
    public Texture2D HullTexture;
    public Texture2D ShieldTexture;

    public Texture2D HostileNpcTexture;

    private int team;

    void Start()
    {
        team = TeamHelper.GetTeamNumber(gameObject.layer);
    }

    void OnGUI()
    {
        // Draw health info
        if (HealthControl.DrawHealthInfo)
        {
            Color hullColor = Color.Lerp(Color.red, Color.green, HealthControl.CurrentHealth / HealthControl.MaxHealth);

            GUI.color = hullColor;
            GUI.Label(
                new Rect(StatusTexture_Left, StatusTexture_Top, StatusTexture_Width, StatusTexture_Height),
                new GUIContent(HullTexture));

            Color shieldColor = Color.white;
            shieldColor.a = HealthControl.CurrentShields / HealthControl.MaxShields;

            GUI.color = shieldColor;
            GUI.Label(
                 new Rect(StatusTexture_Left, StatusTexture_Top, StatusTexture_Width, StatusTexture_Height),
                 new GUIContent(ShieldTexture));

            GUI.color = Color.white;
            GUI.Label(
                    new Rect(StatusTexture_Left + StatusTexture_Width + 10, StatusTexture_Top, StatusTexture_Width, StatusTexture_Height),
                    new GUIContent("Hull: " + HealthControl.CurrentHealth + "/" + HealthControl.MaxHealth));
            GUI.Label(
                    new Rect(StatusTexture_Left + StatusTexture_Width + 10, StatusTexture_Top + 20, StatusTexture_Width, StatusTexture_Height),
                    new GUIContent("Shields: " + HealthControl.CurrentShields + "/" + HealthControl.MaxShields));
        }

        // Draw npc markers
        IList<GameObject> npcs;

        // Find the hostile npcs
        if (team == 1)
            npcs = GlobalSettings.Team2Npcs;
        else npcs = GlobalSettings.Team1Npcs;

        for (int i = 0; i < npcs.Count; i++)
        {
            Vector3 screenPosition = Camera.WorldToScreenPoint(npcs[i].transform.position);

            screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
            screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

            if (screenPosition.z < 0)
            {
                if (screenPosition.x > Screen.width / 2)
                    screenPosition.x = 0;
                else screenPosition.x = Screen.width;
            }
            else
                screenPosition.y = Screen.height - screenPosition.y;

            GUI.Label(new Rect(screenPosition.x - HostileNpcTexture.width / 2f,
                               screenPosition.y - HostileNpcTexture.height / 2f,
                               HostileNpcTexture.width,
                               HostileNpcTexture.height),
                      new GUIContent(HostileNpcTexture));
        }
    }
}
