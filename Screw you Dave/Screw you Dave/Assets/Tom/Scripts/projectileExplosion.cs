using UnityEngine;
using System.Collections;

public class projectileExplosion : MonoBehaviour {

	float lifespan = 2.0f;
	bool collision = false;
	GameObject Player;
	GameObject Controller;
	Camera cam;
	private Vector3 offset;
	private Quaternion rotOffset;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		cam = Camera.main;
		offset = cam.transform.position - Player.transform.position;
		rotOffset = cam.transform.rotation;// - Player.transform.rotation;
	}

	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
		if (lifespan <= 0 && !collision) {
			Explode ();
		}
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Bird") {
			collision = true; //So object isn't destroyed during function
			//Check what type of animal the player is
			if(Player.gameObject.GetComponent<Player>() != null){
				col.gameObject.AddComponent<BirdScript> ();
				col.gameObject.GetComponent<BirdScript>().projectile_prefab = Player.gameObject.GetComponent<Player>().projectile_prefab;
				col.gameObject.GetComponent<BirdScript>().startingTime = Player.gameObject.GetComponent<Player>().startingTime;
				Destroy (Player.gameObject.GetComponent<Player> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "AIPlayer";

				cam.GetComponent<CameraController> ().player = col.gameObject;
				//cam.transform.position = col.transform.position + offset;
				//cam.transform.SetParent (col.transform);
				}
			Explode ();
		}
		if (col.gameObject.tag == "AIPlayer") {
			collision = true; //So object isn't destroyed during function
			//Check what type of animal the player is
			if(Player.gameObject.GetComponent<BirdScript>() != null){
				col.gameObject.AddComponent<Player> ();
				col.gameObject.GetComponent<Player>().projectile_prefab = Player.gameObject.GetComponent<BirdScript>().projectile_prefab;
				col.gameObject.GetComponent<Player>().startingTime = Player.gameObject.GetComponent<BirdScript>().startingTime;
				Destroy (Player.gameObject.GetComponent<BirdScript> ());

				col.gameObject.tag = "Bird";
				Player.gameObject.tag = "Player";

				cam.GetComponent<CameraController> ().player = col.gameObject;
				//cam.transform.position = col.transform.position + offset;
				//cam.transform.SetParent (col.transform);
			}
			Explode ();
		}
	
	}

	void Explode(){
		Destroy (gameObject);
	}
}
