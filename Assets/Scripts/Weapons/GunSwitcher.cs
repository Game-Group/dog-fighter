using UnityEngine;
using System.Collections;

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
		for (int i = 0; i < CurrentGuns.Length; i++)
			assignNewGun(index, i);
	}

	private void assignNewGun(int newGunIndex, int gunToReplaceIndex)
	{
		GameObject gunToReplace = CurrentGuns[gunToReplaceIndex];

		Transform parent = gunToReplace.transform.parent;
		Transform t = gunToReplace.transform;
		Destroy(gunToReplace);
		gunToReplace = (GameObject)Instantiate(Guns[newGunIndex], t.position, t.rotation);
		gunToReplace.transform.parent = parent;

		CurrentGuns[gunToReplaceIndex] = gunToReplace;
	}
}
