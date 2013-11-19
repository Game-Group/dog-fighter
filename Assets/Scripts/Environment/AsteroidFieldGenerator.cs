using UnityEngine;
using System.Collections;

/* Generates a certain amount of asteroids within a bounded area.
 * The position, rotation and size of each asteroid is randomized.
 */
public class AsteroidFieldGenerator : MonoBehaviour {
	//Amount of asteroids generated
	public int amount = 20;
	//Determine the max distance from the center that an astoid can be
	public float maxOffset = 30;
	//The range of possible sizes, relative to the template asteroid
	public float minAsteroidSize = 2;
	public float maxAsteroidSize = 8;
	//The template asteroid that is copied each time
	public GameObject templateAsteroid;

	//Keep a count of the asteroid, used to name them
	private int asteroidCount = 0;

	// Use this for initialization
	void Start () {
		for (int numAsteroids = 0; numAsteroids < amount; numAsteroids ++) {
			CreateRandomAsteroid();
		}
	}

	//Creates one randomized asteroid within the bounds
	void CreateRandomAsteroid () {
		Vector3 position = GetRandomPosition();
		//Make sure that everything scales well
		position.Scale(transform.localScale);

		//Instantiate new Asteroid
		GameObject asteroid = (GameObject) GameObject.Instantiate(templateAsteroid, position, Random.rotation);
		//Scale the asteroid randomly
		asteroid.transform.localScale *= Random.Range(minAsteroidSize, maxAsteroidSize);

		//Make sure it is active, in case the template is hidden/inactive
		asteroid.SetActive(true);
		asteroid.transform.parent = templateAsteroid.transform.parent;
		asteroid.transform.name = "asteroid" + asteroidCount;
		asteroidCount ++;
	}

	//Returns a random vector3 position within the bounds
	public virtual Vector3 GetRandomPosition() {
		return new Vector3(Random.Range(transform.position.x - maxOffset, transform.position.x + maxOffset),
		                                     Random.Range(transform.position.y - maxOffset, transform.position.y + maxOffset),
		                                     Random.Range(transform.position.z - maxOffset, transform.position.z + maxOffset));
	}

}
//""