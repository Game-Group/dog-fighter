using UnityEngine;
using System.Collections;

/// <summary>
/// Allows for switching between a list of guns using specified keyboard keys.
/// </summary>
public class GunSwitcher : MonoBehaviour 
{
	public GameObject[] Guns;
	public Transform[] Hardpoints;

	public bool HumanControlledGuns;
	public bool ShowCrosshair;
	public KeyCode NextGun;
	public KeyCode PreviousGun;

	private GameObject[] CurrentGuns;
	private int currentGunIndex;

	void Start()
	{
		currentGunIndex = 0;
		CurrentGuns = new GameObject[Hardpoints.Length];
		assignNewGuns(currentGunIndex);
	}

	void Update () 
	{
		// Check for input and swap accordingly.
		if(Input.GetKeyDown(NextGun))
		{
			currentGunIndex = (currentGunIndex + 1) % Guns.Length;
			assignNewGuns(currentGunIndex);
		}

		if(Input.GetKeyDown(PreviousGun))
		{
			currentGunIndex = Mathf.Max(0, currentGunIndex - 1);
			assignNewGuns(currentGunIndex);
		}
	}

	private void assignNewGuns(int index)
	{
		// Assign all gun slots to the newly selected gun.
		for (int i = 0; i < CurrentGuns.Length; i++)
			assignNewGun(Hardpoints[i], index, i);
	}

	private void assignNewGun(Transform hardpoint, int newGunIndex, int gunToReplaceIndex)
	{
		// Store some properties of the current gun.
		GameObject gunToReplace = CurrentGuns[gunToReplaceIndex];

		Destroy(gunToReplace);

		// Create the new gun, and assign the stored properties to it.
		GameObject newGun = (GameObject)Instantiate(Guns[newGunIndex], hardpoint.position, hardpoint.rotation);
		newGun.transform.parent = hardpoint;
		newGun.layer = hardpoint.gameObject.layer;

		if (!ShowCrosshair)
			newGun.GetComponent<ThirdPersonCrosshair>().enabled = false;

		if (!HumanControlledGuns)
			newGun.GetComponent<Shooter>().HumanControlled = false;

		CurrentGuns[gunToReplaceIndex] = newGun;
	}
}
