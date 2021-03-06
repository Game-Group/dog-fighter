﻿using UnityEngine;
using System.Collections;

public class ShieldImpact : MonoBehaviour 
{
	public float MaxTimeToLive;
	public MeshRenderer Renderer;

	private float timeToLive;

	void Start () 
	{
		timeToLive = MaxTimeToLive;
	}
	
	void Update () 
	{
		timeToLive -= Time.deltaTime;

		float alpha = timeToLive / MaxTimeToLive;
		Color c = renderer.materials[0].color;
		c.a = alpha;
		renderer.materials[0].color = c;
	}
}
