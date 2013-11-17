using UnityEngine;
using System.Collections;

/// <summary>
/// Allows for switching between a list of guns using specified keyboard keys.
/// </summary>
public class GunSwitcher : MonoBehaviour 
{
	public GameObject[] Guns;
	public GameObject[] CurrentGuns;
	public KeyCode NextGun;
	public KeyCode PreviousGun;

	private int currentGunIndex;

	void Start()
	{
		currentGunIndex = 0;
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
			assignNewGun(index, i);
	}

	private void assignNewGun(int newGunIndex, int gunToReplaceIndex)
	{
		// Store some properties of the current gun.
		GameObject gunToReplace = CurrentGuns[gunToReplaceIndex];

		Transform parent = gunToReplace.transform.parent;
		Transform t = gunToReplace.transform;
		Collider[] ignoredCollisions = gunToReplace.GetComponent<Shooter>().IgnoredCollisions;
		Destroy(gunToReplace);

		// Create the new gun, and assign the stored properties to it.
		GameObject newGun = (GameObject)Instantiate(Guns[newGunIndex], t.position, t.rotation);
		newGun.transform.parent = parent;
		newGun.GetComponent<Shooter>().IgnoredCollisions = ignoredCollisions;

		CurrentGuns[gunToReplaceIndex] = newGun;
	}
}
