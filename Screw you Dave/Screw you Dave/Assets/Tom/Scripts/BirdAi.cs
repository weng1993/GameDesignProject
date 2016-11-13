using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BirdAi : MonoBehaviour {

	public Collider coll;
	public Rigidbody rb;
	public float startingTime;
	private float timeLeft;

	private int meleeDamage;
	private float meleeRange;
	private float attackTime;
	private float cooldown;

	public Slider healthSlider;
	public Slider AtkSlider;
	public Slider CDSlider;

	public bool playerSeen = false;

	public Transform player;
	private float speed = 3f;
	private float attackSpeed = 6f;
	private float rotationSpeed = 6f;

	public Transform[] enemyPath = new Transform[12];
	public int pathNum = 0;
	public bool alive = true;
	Vector3 flight = new Vector3(0,.1f,0);
	int flightCount = 0;
	float flightTime = 0;

	public GameObject home;

	void Start () {
		timeLeft = startingTime;
		//bounciness 0 (likely included in player already)
		coll = GetComponent<BoxCollider>();
		PhysicMaterial material = new PhysicMaterial();
		material.bounciness = 0;
		coll.material = material;

		//freeze rotation so object will fly straight
		rb = GetComponent<Rigidbody> ();
		rb.freezeRotation = true;

		//combat
		meleeDamage = 10;
		meleeRange = 2;
		attackTime = 0;
		cooldown = .5f;

		//UI
		healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
		AtkSlider = GameObject.Find ("AtkSlider").GetComponent<Slider>();
		CDSlider = GameObject.Find ("CDSlider").GetComponent<Slider> ();

		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update () {
		if (alive) {
			if (playerSeen) {
				attackPlayer ();
			} else {
				idle ();
			}
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
		Quaternion rotation = Quaternion.LookRotation (player.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
		if(Vector3.Distance(transform.position,player.position) > meleeRange+.2) {
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
		Quaternion rotation = Quaternion.LookRotation (enemyPath[pathNum].position - transform.position +(flight*25));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
		Vector2 pathDirection = enemyPath [pathNum].position - transform.position +(flight*25);
		float speedElement = Vector2.Dot (pathDirection.normalized,transform.forward);
		transform.Translate (0,0,Time.deltaTime*attackSpeed);

		if (isGrounded ()) {
			timeLeft = startingTime;
		}
		if (flightTime > 0)
			flightTime -= Time.deltaTime;
		if (flightTime <= 0) {
			transform.Translate (flight);
			flightCount++;
			flightTime = .05f;
		}
		if (timeLeft <= 0) {
			flightCount = 0;
		}
	}

	void attack() {
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		if (Physics.Raycast(transform.position, fwd, out hit, meleeRange) && (hit.transform.tag == "Player" || hit.transform.tag == "Bird" || hit.transform.tag == "Bear" || hit.transform.tag == "Turtle")) {
			hit.transform.gameObject.GetComponent<Health2>().adjustHealth (-meleeDamage);
			if (hit.transform.gameObject.GetComponent<Health2>().health <= 0){
				//Game over
			}
		}
	}
}