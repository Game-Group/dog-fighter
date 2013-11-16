using UnityEngine;
using System.Collections;

/* Generates a certain amount of astroids within a bounded area.
 * The position, rotation and size of each astroid is randomized.
 */
public class AstroidGenerator : MonoBehaviour {
	//Amount of astroids generated
	public int amount = 20;
	//Determine the max distance from the center that an astoid can be
	public float maxOffset = 30;
	//The range of possible sizes, relative to the template astroid
	public float minAstroidSize = 2;
	public float maxAstroidSize = 8;
	//The template astroid that is copied each time
	public GameObject templateAstroid;

	//Keep a count of the astroid, used to name them
	private int astroidCount = 0;

	// Use this for initialization
	void Start () {
		for (int numAstroids = 0; numAstroids < amount; numAstroids ++) {
			CreateRandomAstroid();
		}
	}

	//Creates one randomized astroid within the bounds
	void CreateRandomAstroid () {
		Vector3 randomPosition = new Vector3(Random.Range(transform.position.x - maxOffset, transform.position.x + maxOffset),
		                                     Random.Range(transform.position.y - maxOffset, transform.position.y + maxOffset),
		                                     Random.Range(transform.position.z - maxOffset, transform.position.z + maxOffset));
		//Make sure that everything scales well
		randomPosition.Scale(transform.localScale);

		//Instantiate new astroid
		GameObject astroid = (GameObject) GameObject.Instantiate(templateAstroid, randomPosition, Random.rotation);
		//Scale the astroid randomly
		astroid.transform.localScale *= Random.Range(minAstroidSize, maxAstroidSize);

		//Make sure it is active, in case the template is hidden/inactive
		astroid.SetActive(true);
		astroid.transform.parent = templateAstroid.transform.parent;
		astroid.transform.name = "Astroid" + astroidCount;
		astroidCount ++;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
//""