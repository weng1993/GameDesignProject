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
	private Vector3 pos;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		cam = Camera.main;
		offset = cam.transform.position - Player.transform.position;
		rotOffset = cam.transform.rotation;// - Player.transform.rotation;
		pos = new Vector3 (-0.1900001f, 3.922f, -4.03f);
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
				col.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<Player> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "AIPlayer";

				cam.transform.SetParent (col.transform);
				cam.transform.localRotation = Quaternion.Euler(50.634f, 0, 0);
				cam.transform.localPosition = pos;

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
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BirdScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bird";

				cam.transform.SetParent (col.transform);
				cam.transform.localRotation = Quaternion.Euler(50.634f, 0, 0);
				cam.transform.localPosition = pos;
			}
			Explode ();
		}
	
	}

	void Explode(){
		Destroy (gameObject);
	}
}
