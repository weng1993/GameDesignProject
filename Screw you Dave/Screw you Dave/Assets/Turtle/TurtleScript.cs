﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurtleScript : MonoBehaviour {
	public Collider coll;
	Rigidbody rb;
	Vector3 velocity;

	float bulletImpulse = 20f;
	public GameObject projectile_prefab;
	public GameObject claw_prefab;
	bool schleem = false;
	Vector3 fix = new Vector3 (0,0.5f,.5f);

	private int meleeDamage;
	private float meleeRange;
	private float attackTime;
	private float cooldown;

	/*public Slider healthSlider;
	public Slider AtkSlider;
	public Slider CDSlider;	*/

	public bool underwater = false;
	private float underwaterYOffset;
	private float toAboveY;
	private float abovewaterY;
	public float startingTime;
	private float timeLeft;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		coll = GetComponent<BoxCollider>();
		timeLeft = startingTime;

		underwaterYOffset = 0f - (transform.localScale.y + 0.5f);
		abovewaterY = 150.5f;
		toAboveY = transform.position.y - 1f;

		// freeze rotation so turtle will swim straight
		rb.freezeRotation = true;

		//combat
		meleeDamage = 10;
		meleeRange = 2;
		attackTime = 0;
		cooldown = .5f;

		//UI
		/*healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
		AtkSlider = GameObject.Find ("AtkSlider").GetComponent<Slider>();
		CDSlider = GameObject.Find ("CDSlider").GetComponent<Slider> ();*/
	}
	
	// Update is called once per frame
	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * 10;

		//Switch Bodies
		schleem = Input.GetKeyDown("space");
		if (schleem) {
			GameObject projectile = (GameObject)Instantiate (projectile_prefab, transform.position+fix,Camera.main.transform.rotation);
			projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward*bulletImpulse, ForceMode.Impulse);
		}

		//Makes sure turtle stays on ground at switch
		if(!underwater){
			rb.useGravity = true;
		}

		//update swim time
		/* timeLeft -= Time.deltaTime; */

		//base movement
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 10.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 10.0f;

		transform.Translate (x, 0, z);

		//out of swim time
		/* if (timeLeft <= 0) {
			//re-enable gravity
			rb.useGravity = true;
			// and pop turtle out of water (still need to write this code)
		}else { */
			// if swim time left use j to ascend and k to descend
			// remove gravity when in water
		if (Input.GetMouseButton (1)) {
				rb.useGravity = false;
				underwater = true;
				Vector3 v = rb.velocity;
				v.y = 0;
				rb.velocity = v;
				transform.Translate (0, 0.1f, 0);
			} 
		else if (Input.GetMouseButtonUp (1)) {
				rb.useGravity = true;
			}
		//}

		if (attackTime > 0)
			attackTime -= Time.deltaTime;
		if (attackTime <= 0) {
			if (Input.GetMouseButton (0)) {
				attack ();
				attackTime = cooldown;
			}
		}

		//UI
		/*healthSlider.value = (this.gameObject.GetComponent<Health2>().health / (float)this.gameObject.GetComponent<Health2>().maxHealth);
		AtkSlider.value = 1 - (attackTime / cooldown);
		CDSlider.value = (timeLeft / startingTime);*/

		fix.z = 0.5f * Mathf.Cos (Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad);
		fix.x = .5f * Mathf.Sin (Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad);
	}

	void attack() {
		RaycastHit hit;
		bool collided = false;

		Vector3 dir = Camera.main.transform.TransformDirection (Vector3.forward);
		Vector3 dir2 =Camera.main.transform.TransformDirection (Vector3.forward) + new Vector3 (0, .1f, 0);
		Vector3 dir3 =Camera.main.transform.TransformDirection (Vector3.forward) + new Vector3 (-.1f, 0, 0);
		Vector3 dir4 =Camera.main.transform.TransformDirection (Vector3.forward) + new Vector3 (0, -.1f, 0);
		Vector3 dir5 =Camera.main.transform.TransformDirection (Vector3.forward) + new Vector3 (.1f, 0, 0);
		Vector3[] dirs = { dir, dir2, dir3, dir4, dir5 };


		for (int i = 0; i < dirs.Length; i++) {
			//Debug.DrawRay (transform.position+fix, dirs[i] * meleeRange, Color.cyan, 3);
			if (Physics.Raycast(transform.position+fix, dirs[i], out hit, meleeRange) && (hit.transform.tag == "AIPlayer" || hit.transform.tag == "Bird" || hit.transform.tag == "Bear" || hit.transform.tag == "Turtle" || hit.transform.tag == "Human") && collided == false) {
				collided = true;
				hit.transform.gameObject.GetComponent<Health2>().adjustHealth (-meleeDamage);
				if (hit.transform.gameObject.GetComponent<Health2>().health <= 0){
					if (hit.transform.tag.ToLower() == "bird") {
						if(hit.transform.gameObject.GetComponent<BirdAi> () != null){
							hit.transform.gameObject.GetComponent<BirdAi> ().alive = false;
							if(hit.transform.gameObject.GetComponent<BirdAi> ().home.GetComponent<EnemyHome> () != null){
								Destroy (hit.transform.gameObject.GetComponent<BirdAi> ().home.GetComponent<EnemyHome> ());
							}
						}
					}
					if (hit.transform.tag.ToLower() == "bear") {
						if(hit.transform.gameObject.GetComponent<BearAi> () != null){
							hit.transform.gameObject.GetComponent<BearAi> ().alive = false;
							if (hit.transform.gameObject.GetComponent<BearAi> ().home.GetComponent<EnemyHome> () != null) {
								Destroy (hit.transform.gameObject.GetComponent<BearAi> ().home.GetComponent<EnemyHome> ());
							}
						}
					}
					if (hit.transform.tag.ToLower() == "turtle") {
						if(hit.transform.gameObject.GetComponent<TurtleAi> () != null){
							hit.transform.gameObject.GetComponent<TurtleAi> ().alive = false;
							if (hit.transform.gameObject.GetComponent<TurtleAi> ().home.GetComponent<EnemyHome> () != null) {
								Destroy (hit.transform.gameObject.GetComponent<TurtleAi> ().home.GetComponent<EnemyHome> ());
							}
						}
					}
					if (hit.transform.tag.ToLower() == "human") {
						if(hit.transform.gameObject.GetComponent<HumanAi> () != null){
							hit.transform.gameObject.GetComponent<HumanAi> ().alive = false;
							if(hit.transform.gameObject.GetComponent<HumanAi> ().home.GetComponent<EnemyHome> () != null){
								Destroy (hit.transform.gameObject.GetComponent<HumanAi> ().home.GetComponent<EnemyHome> ());
							}
						}
					}
				}
			}
		}
	} 

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Water")) {
			if (underwater = false) {
				underwater = true;
				rb.useGravity = false;
				transform.Translate (0, underwaterYOffset, 0);
			} else {
				if (transform.position.y < toAboveY) {
					if (transform.position.x < other.gameObject.transform.position.x) {
						transform.Translate (-5f, (abovewaterY - transform.position.y), 0);
					} else {
						transform.Translate (5f, (abovewaterY - transform.position.y), 0);
					}
					underwater = false;
					rb.useGravity = true;
				}
			}
		}
	}
}