using UnityEngine;
using System.Collections;

public class GlobalSettings : MonoBehaviour {
	
	public bool HasFocus { get; private set;}
	
	// Use this for initialization
	void Start () {
	
	}
	
	void OnApplicationFocus() 
	{
    	this.HasFocus = !this.HasFocus;    
		//Debug.Log("Focus = " + this.HasFocus);
	}
	
	// Update is called once per frame
	void Update () {

	}


}
