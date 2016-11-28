using UnityEngine;
using System.Collections;

public class EnemyHome : MonoBehaviour {

	//public BoxCollider territory;
	GameObject player;
	bool playerInTerritory;
	string EnemyTag = "";
	public GameObject enemy;

	// Use this for initialization
	void Start ()
	{
		//player = GameObject.FindGameObjectWithTag ("Player");
		playerInTerritory = false;
		EnemyTag = enemy.gameObject.tag.ToLower();
	}

	// Update is called once per frame
	void Update ()
	{
		if (playerInTerritory == true)
		{
			switch (EnemyTag) {
			case "bird":
				enemy.GetComponent<BirdAi> ().playerSeen = true;
				break;

			case "bear":
				enemy.GetComponent<BearAi> ().playerSeen = true;
				break;

			case "turtle":
				enemy.GetComponent<TurtleAi> ().playerSeen = true;
				break;

			case "human":
				break;

			case "dave":
				break;
			}
		}

		if (playerInTerritory == false)
		{
			switch (EnemyTag) {
			case "bird":
				enemy.GetComponent<BirdAi> ().playerSeen = false;
				break;

			case "bear":
				enemy.GetComponent<BearAi> ().playerSeen = false;
				break;

			case "turtle":
				enemy.GetComponent<TurtleAi> ().playerSeen = false;
				break;

			case "human":
				break;

			case "dave":
				break;
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerInTerritory = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			playerInTerritory = false;
		}
	}
}