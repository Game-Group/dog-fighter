using UnityEngine;
using System.Collections;

public class MatchControl : MonoBehaviour 
{
    public float GameEndDelay
    {
        get { return this.gameEndDelay; }
        set
        {
            if (value < 0)
                this.gameEndDelay = 0;
            else
                this.gameEndDelay = value;
        }
    }

    public virtual void EndMatch(MatchResult result)
    {
        Debug.Log("Ending match.");

        this.countDown = true;
        this.result = result;
    }

    public void ObjectDestroyed(GameObject obj)
    {
        ObjectSync objSync = obj.GetComponent<ObjectSync>();

        if (objSync == null)
            throw new UnityException("Given GameObject must contain an ObjectSync component");

        if (objSync.Type == ObjectSyncType.Mothership)
        {
            MatchResult result = TeamHelper.GetTeamNumber(obj.layer) == 1 ? MatchResult.Team2Win : MatchResult.Team1Win;
            this.EndMatch(result);
        }
    }

    protected virtual void Awake()
    {
        this.name = GlobalSettings.MatchControlName;
        this.GameEndDelay = 5;
        this.result = MatchResult.Undefined;
    }

	// Use this for initialization
	protected virtual void Start () 
    {
	}
	
	// Update is called once per frame
	protected virtual void Update () 
    {
        if (this.countDown)
        {
            this.gameEndDelay += Time.deltaTime;

            if (this.waitTime >= this.gameEndDelay)
                this.MatchFinished();
        }
	}

    protected void MatchFinished()
    {
        Application.LoadLevel("MainMenu");
    }

    protected virtual void OnGuid()
    {
        if (this.result != MatchResult.Undefined)
        {
            if (this.result != MatchResult.Disconnected)
            {
                GUI.Label(new Rect(10, 40, 100, 100), "Team " + this.result + " has won!");
            }
            else
                GUI.Label(new Rect(10, 40, 100, 100), "Disconnected from server!");

            GUI.Label(new Rect(10, 70, 100, 100), "" + (int)this.waitTime);

        }
    }

    private bool countDown;
    private float gameEndDelay;
    private float waitTime;
    private MatchResult result;
}
