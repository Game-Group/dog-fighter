using UnityEngine;
using System.Collections;

public class MothershipHealthControl : HealthControl 
{
    public GameObject ExplosionGraphic;
    public AudioClip ExplosionSound;

    public float ExplosionScale;

    private bool dead;
    private int team;

    protected override void Initialize()
    {
        team = TeamHelper.GetTeamNumber(gameObject.layer);
        dead = false;
    }

    public override void Die()
    {
        if (dead)
            return;

        if (Network.peerType != NetworkPeerType.Server)
        {
            GameObject explinst = Instantiate(ExplosionGraphic, gameObject.transform.position, Quaternion.identity) as GameObject;
            explinst.transform.localScale *= ExplosionScale;
            AudioSource.PlayClipAtPoint(ExplosionSound, gameObject.transform.position);
        }

        Destroy(gameObject, 5);
        dead = true;
    }

    public void OnDestroy()
    {
        Application.LoadLevel("MainMenu");
    }

    void OnGUI()
    {
        if (!this.DrawHealthInfo)
            return;

        if (team == 1)
        {
            GUI.Label(new Rect(Screen.width - Screen.width * 0.1f - 200,
                               Screen.height * 0.1f,
                               200,
                               40),
                      new GUIContent("Mothership team " + team + " health: " + health + "\n"
                                   + "Mothership team " + team + " shields: " + shieldStrength));
        }
        else
        {
            GUI.Label(new Rect(Screen.width - Screen.width * 0.1f - 200,
                               Screen.height * 0.1f + 50,
                               200,
                               40),
                      new GUIContent("Mothership team " + team + " health: " + health + "\n"
                                   + "Mothership team " + team + " shields: " + shieldStrength));
        }
    }
}
