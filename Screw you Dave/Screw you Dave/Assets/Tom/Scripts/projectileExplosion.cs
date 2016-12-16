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
	private AudioSource sound;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		cam = Camera.main;
		offset = cam.transform.position - Player.transform.position;
		rotOffset = cam.transform.rotation;// - Player.transform.rotation;
		//pos = new Vector3 (-0.1900001f, 3.922f, -5.51f);
		pos = new Vector3 (-0.1900001f, 3f, -6.51f);
		sound = GameObject.Find ("schleem(blorp)").GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
		if (lifespan <= 0 && !collision) {
			Explode ();
		}
	}

	void OnTriggerEnter(Collider col){
		sound.Play();
		if (col.gameObject.tag == "Bird" && col.gameObject.GetComponent<Health2>().health ==0) {
			Player.gameObject.GetComponent<Animator> ().SetTrigger ("ShleemSuc");
			col.gameObject.GetComponent<Animator> ().SetBool ("Dead", false);
			Player.gameObject.GetComponent<Animator> ().SetBool ("Dead", true);
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
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				//col.gameObject.GetComponent<Health2>().health =  System.Math.Min(Player.gameObject.GetComponent<Health2>().health, col.gameObject.GetComponent<Health2>().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BearScript> ());
				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bear";
				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;

				}
			if(Player.gameObject.GetComponent<BirdScript>() != null){
				col.gameObject.AddComponent<BirdScript> ();
				col.gameObject.GetComponent<BirdScript>().projectile_prefab = Player.gameObject.GetComponent<BirdScript>().projectile_prefab;
				col.gameObject.GetComponent<BirdScript>().claw_prefab = Player.gameObject.GetComponent<BirdScript>().claw_prefab;
				col.gameObject.GetComponent<BirdScript>().startingTime = Player.gameObject.GetComponent<BirdScript>().startingTime;
				col.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				if (col.gameObject.GetComponent<BirdAi> () != null) {
					Destroy (col.gameObject.GetComponent<BirdAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BirdScript> ());
				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bird";
				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;

			}
			if(Player.gameObject.GetComponent<TurtleScript>() != null){
				col.gameObject.AddComponent<BirdScript> ();
				col.gameObject.GetComponent<BirdScript>().projectile_prefab = Player.gameObject.GetComponent<TurtleScript>().projectile_prefab;
				col.gameObject.GetComponent<BirdScript>().claw_prefab = Player.gameObject.GetComponent<TurtleScript>().claw_prefab;
				col.gameObject.GetComponent<BirdScript>().startingTime = Player.gameObject.GetComponent<TurtleScript>().startingTime;
				col.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				if (col.gameObject.GetComponent<BirdAi> () != null) {
					Destroy (col.gameObject.GetComponent<BirdAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<TurtleScript> ());
				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Turtle";
				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;

			}
			Explode ();
		}
		if (col.gameObject.tag == "Bear"&& col.gameObject.GetComponent<Health2>().health ==0) {
			collision = true; //So object isn't destroyed during function
			col.gameObject.GetComponent<Animator> ().SetBool ("Dead", false);
			Player.gameObject.GetComponent<Animator> ().SetBool ("Dead", true);
			//Check what type of animal the player is
			if(Player.gameObject.GetComponent<BirdScript>() != null){
				col.gameObject.AddComponent<BearScript> ();
				col.gameObject.GetComponent<BearScript>().projectile_prefab = Player.gameObject.GetComponent<BirdScript>().projectile_prefab;
				col.gameObject.GetComponent<BearScript>().claw_prefab = Player.gameObject.GetComponent<BirdScript>().claw_prefab;
				col.gameObject.GetComponent<BearScript>().startingTime = Player.gameObject.GetComponent<BirdScript>().startingTime;
				if (col.gameObject.GetComponent<BearAi> () != null) {
					Destroy (col.gameObject.GetComponent<BearAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BirdScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bird";

//				cam.transform.SetParent (col.transform);
				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;
			}
			if(Player.gameObject.GetComponent<BearScript>() != null){
				col.gameObject.AddComponent<BearScript> ();
				col.gameObject.GetComponent<BearScript>().projectile_prefab = Player.gameObject.GetComponent<BearScript>().projectile_prefab;
				col.gameObject.GetComponent<BearScript>().claw_prefab = Player.gameObject.GetComponent<BearScript>().claw_prefab;
				col.gameObject.GetComponent<BearScript>().startingTime = Player.gameObject.GetComponent<BearScript>().startingTime;
				if (col.gameObject.GetComponent<BearAi> () != null) {
					Destroy (col.gameObject.GetComponent<BearAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BearScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bear";

				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;
			}
			if(Player.gameObject.GetComponent<TurtleScript>() != null){
				col.gameObject.AddComponent<BearScript> ();
				col.gameObject.GetComponent<BearScript>().projectile_prefab = Player.gameObject.GetComponent<TurtleScript>().projectile_prefab;
				col.gameObject.GetComponent<BearScript>().claw_prefab = Player.gameObject.GetComponent<TurtleScript>().claw_prefab;
				col.gameObject.GetComponent<BearScript>().startingTime = Player.gameObject.GetComponent<TurtleScript>().startingTime;
				if (col.gameObject.GetComponent<BearAi> () != null) {
					Destroy (col.gameObject.GetComponent<BearAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<TurtleScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Turtle";

				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;
			}
			Explode ();
		}
		if (col.gameObject.tag == "Turtle"&& col.gameObject.GetComponent<Health2>().health ==0) {
			collision = true; //So object isn't destroyed during function
			col.gameObject.GetComponent<Animator> ().SetBool ("Dead", false);
			Player.gameObject.GetComponent<Animator> ().SetBool ("Dead", true);
			//Check what type of animal the player is
			if(Player.gameObject.GetComponent<BirdScript>() != null){
				col.gameObject.AddComponent<TurtleScript> ();
				col.gameObject.GetComponent<TurtleScript>().projectile_prefab = Player.gameObject.GetComponent<BirdScript>().projectile_prefab;
				col.gameObject.GetComponent<TurtleScript>().claw_prefab = Player.gameObject.GetComponent<BirdScript>().claw_prefab;
				col.gameObject.GetComponent<TurtleScript>().startingTime = Player.gameObject.GetComponent<BirdScript>().startingTime;
				if (col.gameObject.GetComponent<TurtleAi> () != null) {
					Destroy (col.gameObject.GetComponent<TurtleAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BirdScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bird";

				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;
			}
			if(Player.gameObject.GetComponent<BearScript>() != null){
				col.gameObject.AddComponent<TurtleScript> ();
				col.gameObject.GetComponent<TurtleScript>().projectile_prefab = Player.gameObject.GetComponent<BearScript>().projectile_prefab;
				col.gameObject.GetComponent<TurtleScript>().claw_prefab = Player.gameObject.GetComponent<BearScript>().claw_prefab;
				col.gameObject.GetComponent<TurtleScript>().startingTime = Player.gameObject.GetComponent<BearScript>().startingTime;
				if (col.gameObject.GetComponent<TurtleAi> () != null) {
					Destroy (col.gameObject.GetComponent<TurtleAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BearScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bear";

				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;
			}
			if(Player.gameObject.GetComponent<TurtleScript>() != null){
				col.gameObject.AddComponent<TurtleScript> ();
				col.gameObject.GetComponent<TurtleScript>().projectile_prefab = Player.gameObject.GetComponent<TurtleScript>().projectile_prefab;
				col.gameObject.GetComponent<TurtleScript>().claw_prefab = Player.gameObject.GetComponent<TurtleScript>().claw_prefab;
				col.gameObject.GetComponent<TurtleScript>().startingTime = Player.gameObject.GetComponent<TurtleScript>().startingTime;
				if (col.gameObject.GetComponent<TurtleAi> () != null) {
					Destroy (col.gameObject.GetComponent<TurtleAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<TurtleScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Turtle";

				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;
			}
			Explode ();
		}
		if (col.gameObject.tag == "Human" && col.gameObject.GetComponent<Health2> ().health == 0) {
			collision = true; //So object isn't destroyed during function
			col.gameObject.GetComponent<Animator> ().SetBool ("Dead", false);
			Player.gameObject.GetComponent<Animator> ().SetBool ("Dead", true);
			//Check what type of animal the player is
			if(Player.gameObject.GetComponent<BirdScript>() != null){
				col.gameObject.AddComponent<DaveScript> ();
				col.gameObject.GetComponent<DaveScript>().projectile_prefab = Player.gameObject.GetComponent<BirdScript>().projectile_prefab;
				col.gameObject.GetComponent<DaveScript>().claw_prefab = Player.gameObject.GetComponent<BirdScript>().claw_prefab;
				col.gameObject.GetComponent<DaveScript>().startingTime = Player.gameObject.GetComponent<BirdScript>().startingTime;
				if (col.gameObject.GetComponent<HumanAi> () != null) {
					Destroy (col.gameObject.GetComponent<HumanAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BirdScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bird";

				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				Vector3 viewFix = new Vector3 (0,0,100f);
				cam.transform.localPosition = viewFix;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;
			}
			if(Player.gameObject.GetComponent<BearScript>() != null){
				col.gameObject.AddComponent<DaveScript> ();
				col.gameObject.GetComponent<DaveScript>().projectile_prefab = Player.gameObject.GetComponent<BearScript>().projectile_prefab;
				col.gameObject.GetComponent<DaveScript>().claw_prefab = Player.gameObject.GetComponent<BearScript>().claw_prefab;
				col.gameObject.GetComponent<DaveScript>().startingTime = Player.gameObject.GetComponent<BearScript>().startingTime;
				if (col.gameObject.GetComponent<HumanAi> () != null) {
					Destroy (col.gameObject.GetComponent<HumanAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<BearScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Bear";

				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;
			}
			if(Player.gameObject.GetComponent<TurtleScript>() != null){
				col.gameObject.AddComponent<DaveScript> ();
				col.gameObject.GetComponent<DaveScript>().projectile_prefab = Player.gameObject.GetComponent<TurtleScript>().projectile_prefab;
				col.gameObject.GetComponent<DaveScript>().claw_prefab = Player.gameObject.GetComponent<TurtleScript>().claw_prefab;
				col.gameObject.GetComponent<DaveScript>().startingTime = Player.gameObject.GetComponent<TurtleScript>().startingTime;
				if (col.gameObject.GetComponent<HumanAi> () != null) {
					Destroy (col.gameObject.GetComponent<HumanAi> ());
				}
				col.gameObject.GetComponent<Health2>().maxHealth = Player.gameObject.GetComponent<Health2> ().maxHealth;
				col.gameObject.GetComponent<Health2>().health = Player.gameObject.GetComponent<Health2> ().health;
				//col.gameObject.GetComponent<Health2> ().health = (int)(((float)Player.gameObject.GetComponent<Health2> ().health / (float)Player.gameObject.GetComponent<Health2> ().maxHealth) * col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().health = 0;
				col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
				Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
				Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
				Destroy (Player.gameObject.GetComponent<TurtleScript> ());

				col.gameObject.tag = "Player";
				Player.gameObject.tag = "Turtle";

				Vector3 forward = col.gameObject.transform.forward;
				forward.y = 0;
				Quaternion headingAngle = Quaternion.LookRotation (forward);
				cam.transform.localRotation = headingAngle;
				cam.transform.localPosition = pos + col.transform.position;
				cam.GetComponent<Orbit>().Target = col.gameObject;
				cam.GetComponent<Orbit> ().t = col.gameObject.transform;
				cam.GetComponent<Orbit> ().mesh = col.gameObject.transform.FindChild ("Body").transform;
			}
			Explode ();
		}
	}

/*
	void OnTriggerEnter(Collider col){
		if (col.gameObject.GetComponent<Health2> ().health == 0) {
			swapScripts (col);
			Explode ();
		}
	}
	void swapScripts(Collider col) {
		string colTag;
		string playerTag;
		if (Player.gameObject.GetComponent<BearScript> () != null) {
			playerTag = "Bear";
			var playerScript = Player.gameObject.GetComponent<BearScript> ();
		} else if (Player.gameObject.GetComponent<BirdScript> () != null) {
			playerTag = "Bird";
			var playerScript = Player.gameObject.GetComponent<BirdScript> ();
		} else if (Player.gameObject.GetComponent<TurtleScript> () != null) {
			playerTag = "Turtle";
			var playerScript = Player.gameObject.GetComponent<TurtleScript> ();
		}
		if (col.gameObject.tag == "Bear") {
			colTag = "Bear";
			col.gameObject.AddComponent<BearScript> ();
			var colScript = col.gameObject.GetComponent<BearScript> ();
			if (col.gameObject.GetComponent<BearAi> () != null) {
				var colAi = col.gameObject.GetComponent<BearAi> ();
			}
		}
		if (col.gameObject.tag == "Bird") {
			colTag = "Bird";
			col.gameObject.AddComponent<BirdScript> ();
			var colScript = col.gameObject.GetComponent<BirdScript> ();
			if (col.gameObject.GetComponent<BirdAi> () != null) {
				var colAi = col.gameObject.GetComponent<BirdAi> ();
			}
		}
		if (col.gameObject.tag == "Turtle") {
			colTag = "Turtle";
			col.gameObject.AddComponent<TurtleScript> ();
			var colScript = col.gameObject.GetComponent<TurtleScript> ();
			if (col.gameObject.GetComponent<TurtleAi> () != null) {
				var colAi = col.gameObject.GetComponent<TurtleAi> ();
			}
		}
		col.gameObject.AddComponent<colScript> ();
		col.gameObject.GetComponent<colScript> ().projectile_prefab = Player.gameObject.GetComponent<playerScript> ().projectile_prefab;
		col.gameObject.GetComponent<colScript> ().claw_prefab = Player.gameObject.GetComponent<playerScript> ().claw_prefab;
		col.gameObject.GetComponent<colScript> ().startingTime = Player.gameObject.GetComponent<playerScript> ().startingTime;
		col.gameObject.GetComponent<Rigidbody> ().useGravity = true;
		if (col.gameObject.GetComponent<colAi> () != null) {
			Destroy (col.gameObject.GetComponent<colAi> ());
		}
		col.gameObject.GetComponent<Health2> ().health = System.Math.Min (Player.gameObject.GetComponent<Health2> ().health, col.gameObject.GetComponent<Health2> ().maxHealth);
		Player.gameObject.GetComponent<Health2> ().health = 0;
		col.gameObject.GetComponent<Health2> ().healthBar.fillAmount = ((float)col.gameObject.GetComponent<Health2> ().health / (float)col.gameObject.GetComponent<Health2> ().maxHealth);
		Player.gameObject.GetComponent<Health2> ().healthBar.fillAmount = 0;
		Player.gameObject.GetComponent<Rigidbody> ().useGravity = true;
		Destroy (Player.gameObject.GetComponent<playerScript> ());
		col.gameObject.tag = "Player";
		Player.gameObject.tag = playerTag;
		cam.transform.SetParent (col.transform);
		cam.transform.localRotation = Quaternion.Euler (19.35f, 0, 0);
		cam.transform.localPosition = pos;
	}
*/
	void Explode(){
		Destroy (gameObject);
	}
}
