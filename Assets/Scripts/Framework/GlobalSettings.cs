using UnityEngine;
using System.Collections;

public class GlobalSettings : MonoBehaviour 
{
	public const bool SinglePlayer = true;
	
	public static bool HasFocus { get; private set;}
	
	// Use this for initialization
	void Start () {
	}

	
	void OnApplicationFocus(bool focus) 
	{
    	HasFocus = focus;    
		//Debug.Log("Focus = " + this.HasFocus);
	}
	
	// Update is called once per frame
	void Update () {

	}


}
