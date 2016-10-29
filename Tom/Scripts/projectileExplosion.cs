using UnityEngine;
using System.Collections;

public class projectileExplosion : MonoBehaviour {

	float lifespan = 2.0f;
	bool collision = false;
	GameObject Player;
	GameObject Controller;
	 
	// Use this for initialization
	void Start () {
		//Player = GameObject.FindGameObjectWithTag ("Schleemer");
		Player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
		if (lifespan <= 0 && !collision) {
			Explode ();
		}
	
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Enemy") {
			collision = true; //So object isn't destroyed during function
			//Switch Players
			col.gameObject.AddComponent<Schleem> ();
			col.gameObject.GetComponent<Schleem>().projectile_prefab = Player.gameObject.GetComponent<Schleem>().projectile_prefab;
			Destroy (Player.gameObject.GetComponent<Schleem> ());

			col.gameObject.AddComponent<Player>();
			Destroy (Player.gameObject.GetComponent<Player> ());
			//Player.gameObject.GetComponent<ThirdPersonUserControl>().enabled = false;

			col.gameObject.tag = "Player";
			Player.gameObject.tag = "Enemy";

			Camera cam = Camera.main;
			cam.gameObject.GetComponent<CameraController>().player = col.gameObject;

			//Make old guy AI
			//Make Enemny controllable
			//Change Camera

			Explode ();
		}
	}

	void Explode(){
		Destroy (gameObject);
	}
}
