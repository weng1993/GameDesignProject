﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurtleAi : MonoBehaviour {

	public Collider coll;
	public Rigidbody rb;
	public float startingTime;
	private float timeLeft;

	public int meleeDamage = 15;
	private float meleeRange;
	private float attackTime;
	private float cooldown;

	private float specialCD;
	private float CDTime;

	public Slider healthSlider;
	public Slider AtkSlider;
	public Slider CDSlider;

	public bool playerSeen = false;
	public bool flight = false;

	public Transform player;
	private float speed = 2f;
	private float attackSpeed = 10f;
	private float rotationSpeed = 6f;

	public Transform[] enemyPath = new Transform[12];
	public int pathNum = 0;
	public bool alive = true;

	public GameObject home;

	Animator m_Animator;

	void Start () {
		for (int i = 0; i < 12; i++)
			enemyPath [i] = home.transform.GetChild (i);
		
		timeLeft = startingTime;
		//bounciness 0 (likely included in player already)
		coll = GetComponent<BoxCollider>();
		PhysicMaterial material = new PhysicMaterial();
		material.bounciness = 0;
		coll.material = material;
		m_Animator = gameObject.GetComponent<Animator> ();

		//freeze rotation so object will fly straight
		rb = GetComponent<Rigidbody> ();
		rb.freezeRotation = true;

		meleeRange = 3;
		attackTime = 0;
		cooldown = .5f;

		specialCD = 1;
		CDTime = 0;
		//UI
		healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
		AtkSlider = GameObject.Find ("AtkSlider").GetComponent<Slider>();
		CDSlider = GameObject.Find ("CDSlider").GetComponent<Slider> ();

		player = GameObject.FindGameObjectWithTag ("Player").transform;
		m_Animator.SetBool ("Walk", true);
	}

	void Update () {
		if (alive) {
			if (playerSeen) {
				attackPlayer ();
			} else {
				idle ();
			}
		} else {
			m_Animator.SetBool ("Dead", true);
			m_Animator.SetBool ("Walk", false);
		}
	}

	bool isGrounded() {
		//change 3rd var depending on object size
		return Physics.Raycast (transform.position, -Vector3.up, 0.6f);
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "enemyPath"){
			if (pathNum < 11) {
				pathNum++;
			} else {
				pathNum = 0;
			}

		}
	}

	void attackPlayer(){
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		Quaternion rotation = Quaternion.LookRotation (player.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
		if(Vector3.Distance(transform.position,player.position) > meleeRange) {
			transform.Translate (0,0,Time.deltaTime*speed);
		}
		else{
			if (attackTime > 0)
				attackTime -= Time.deltaTime;
			if (attackTime <= 0) {
				attack ();
				attackTime = cooldown;
			}
		}
	}

	void idle(){
		Quaternion rotation = Quaternion.LookRotation (enemyPath[pathNum].position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
		Vector2 pathDirection = enemyPath [pathNum].position - transform.position;
		float speedElement = Vector2.Dot (pathDirection.normalized,transform.forward);
		transform.Translate (0,0,Time.deltaTime*attackSpeed);
	}

	void attack() {
		RaycastHit hit;
<<<<<<< HEAD
		m_Animator.SetTrigger ("Attack");
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		if (Physics.Raycast(transform.position, fwd, out hit, meleeRange) && (hit.transform.tag == "Player" || hit.transform.tag == "Bird" || hit.transform.tag == "Bear" || hit.transform.tag == "Turtle")) {
			hit.transform.gameObject.GetComponent<Health2>().adjustHealth (-meleeDamage);
			if ((hit.transform.gameObject.GetComponent<Health2>().health <= 0) && (hit.transform.gameObject.tag == "Player")){
				//SceneManager.LoadScene (2);
				SceneManager.LoadScene (0);
=======

		Vector3 dir = (player.position - transform.position) / (player.position - transform.position).magnitude;
		Vector3 dir2 = (player.position - (transform.position + new Vector3(0,.1f,0))) / (player.position - transform.position).magnitude;
		Vector3 dir3 = (player.position - (transform.position + new Vector3(-.1f,0,0))) / (player.position - transform.position).magnitude;
		Vector3 dir4 = (player.position - (transform.position + new Vector3(0,-.1f,0))) / (player.position - transform.position).magnitude;
		Vector3 dir5 = (player.position - (transform.position + new Vector3(.1f,0,0))) / (player.position - transform.position).magnitude;
		Vector3[] dirs = { dir, dir2, dir3, dir4, dir5 };

		bool collided = false;
		for (int i=0; i<dirs.Length; i++) {
			if (Physics.Raycast (transform.position, dirs [i], out hit, meleeRange + .2f) && (hit.transform.tag == "Player" || hit.transform.tag == "Bird" || hit.transform.tag == "Bear" || hit.transform.tag == "Turtle") && (collided == false)) {
				collided = true;
				hit.transform.gameObject.GetComponent<Health2> ().adjustHealth (-meleeDamage);
				if ((hit.transform.gameObject.GetComponent<Health2> ().health <= 0) && (hit.transform.gameObject.tag == "Player")) {
					//SceneManager.LoadScene (2);
					SceneManager.LoadScene (0);
				}
>>>>>>> 090000f461798c363e50b7014bdc7fb31ab8d54b
			}
			i++;
		}


		//		Debug.DrawRay (transform.position+fix, dir * (meleeRange+.2f), Color.cyan, 3);
		//		Debug.DrawRay (transform.position+fix, dir2 * (meleeRange+.2f), Color.cyan, 3);
		//		Debug.DrawRay (transform.position+fix, dir3 * (meleeRange+.2f), Color.cyan, 3);
		//		Debug.DrawRay (transform.position+fix, dir4 * (meleeRange+.2f), Color.cyan, 3);
		//		Debug.DrawRay (transform.position+fix, dir5 * (meleeRange+.2f), Color.cyan, 3);

		//		if (Physics.Raycast(transform.position, dir, out hit, meleeRange) && (hit.transform.tag == "Player" || hit.transform.tag == "Bird" || hit.transform.tag == "Bear" || hit.transform.tag == "Turtle")) {
		//			hit.transform.gameObject.GetComponent<Health2>().adjustHealth (-meleeDamage);
		//			if ((hit.transform.gameObject.GetComponent<Health2>().health <= 0) && (hit.transform.gameObject.tag == "Player")){
		//				//SceneManager.LoadScene (2);
		//				SceneManager.LoadScene (0);
		//			}
		//		}
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Claw") {
			Vector3 dir = transform.forward;
			Destroy(col.gameObject);
			rb.velocity = (new Vector3(0,0,0));
			if (transform.gameObject.GetComponent<Health2> ().health <= 0)
				rb.AddForce (-dir * 5, ForceMode.Impulse);
			else
				rb.AddForce (-dir*15, ForceMode.Impulse);
		}
	}
}
