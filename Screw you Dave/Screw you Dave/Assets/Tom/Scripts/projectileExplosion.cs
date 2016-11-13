using UnityEngine;
using System.Collections;

public class projectileExplosion : MonoBehaviour {

	float lifespan = 0.2f;
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
		pos = new Vector3 (-0.1900001f, 3.922f, -5.51f);
	}

	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
		if (lifespan <= 0 && !collision) {
			Explode ();
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Bird" && col.gameObject.GetComponent<Health2>().health ==0) {
			collision = true; //So object isn't destroyed during function
			//Check what type of animal the player is
			if(Player.gameObject.GetComponent<BearScript>() != null){
				col.gameObject.AddComponent<BirdScript> ();
				col.gameObject.GetComponent<BirdScript>().projectile_prefab = Player.gameObject.GetComponent<BearScript>().projectile_prefab;
				col.gameObject.GetComponent<BirdScript>().claw_prefab = Player.gameObject.GetComponent<BearScript>().claw_prefab;
				col.gameObject.GetComponent<BirdScript>().startingTime = Player.gameObject.GetComponent<BearScript>().startingTime;
				col.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				if (col.gameObject.GetComponent<BirdAi> () != null) {
					Destroy (col.gameObject.GetComponent<BirdAi> ());
				}
				col.gameObject.GetComponent<Health2>().health =  Player.gameObject.GetComponent<Health2>().health;
				Player.gameObject.GetComponent<Health2> ().health = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BearScript> ());
				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bear";
				cam.transform.SetParent (col.transform);
				cam.transform.localRotation = Quaternion.Euler(35.47f, 0, 0);
				cam.transform.localPosition = pos;

				}
			Explode ();
		}
		if (col.gameObject.tag == "Bear"&& col.gameObject.GetComponent<Health2>().health ==0) {
			collision = true; //So object isn't destroyed during function
			//Check what type of animal the player is
			if(Player.gameObject.GetComponent<BirdScript>() != null){
				col.gameObject.AddComponent<BearScript> ();
				col.gameObject.GetComponent<BearScript>().projectile_prefab = Player.gameObject.GetComponent<BirdScript>().projectile_prefab;
				col.gameObject.GetComponent<BearScript>().claw_prefab = Player.gameObject.GetComponent<BirdScript>().claw_prefab;
				col.gameObject.GetComponent<BearScript>().startingTime = Player.gameObject.GetComponent<BirdScript>().startingTime;
				if (col.gameObject.GetComponent<BearAi> () != null) {
					Destroy (col.gameObject.GetComponent<BearAi> ());
				}
				col.gameObject.GetComponent<Health2>().health =  Player.gameObject.GetComponent<Health2>().health;
				Player.gameObject.GetComponent<Health2> ().health = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BirdScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bird";

				cam.transform.SetParent (col.transform);
				cam.transform.localRotation = Quaternion.Euler(35.47f, 0, 0);
				cam.transform.localPosition = pos;
			}
			Explode ();
		}
	
	}

	void Explode(){
		Destroy (gameObject);
	}
}
