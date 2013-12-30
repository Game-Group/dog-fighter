using UnityEngine;
using System.Collections;

public class MatchControl : MonoBehaviour 
{
    public void ObjectDestroyed(GameObject obj)
    {
        ObjectSync objSync = obj.GetComponent<ObjectSync>();

        if (objSync.Type == ObjectSyncType.Mothership)
        {

        }
    }

	// Use this for initialization
	protected virtual void Start () 
    {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () 
    {
	
	}
}
