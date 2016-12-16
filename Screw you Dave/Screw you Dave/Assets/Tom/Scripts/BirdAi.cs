using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BirdAi : MonoBehaviour {

	public Collider coll;
	public Rigidbody rb;
	public float startingTime;
	private float timeLeft;

	public int meleeDamage = 8;
	private float meleeRange;
	private float attackTime;
	private float cooldown;
	Vector3 fix = new Vector3 (0,1.25f,.5f);

	public Slider healthSlider;
	public Slider AtkSlider;
	public Slider CDSlider;

	public bool playerSeen = false;

	public Transform player;
	private float speed = 6f;
	private float attackSpeed = 3f;
	private float rotationSpeed = 6f;

	public Transform[] enemyPath = new Transform[12];
	public int pathNum = 0;
	public bool alive = true;
	Vector3 flight = new Vector3(0,.5f,0);
	int flightCount = 0;
	float flightTime = 0;
	public GameObject home;

	Animator m_Animator;

	void Start () {

		for (int i = 0; i < 12; i++)
			enemyPath [i] = home.transform.GetChild (i);
		if (alive == false)
			Destroy (transform.gameObject.GetComponent<BearAi> ().home.GetComponent<EnemyHome> ());

		timeLeft = startingTime;
		//bounciness 0 (likely included in player already)
		coll = GetComponent<BoxCollider>();
		PhysicMaterial material = new PhysicMaterial();
		material.bounciness = 0;
		coll.material = material;
		m_Animator = GetComponent<Animator>();

		//freeze rotation so object will fly straight
		rb = GetComponent<Rigidbody> ();
		rb.freezeRotation = true;

		//combat
		meleeRange = 2;
		attackTime = 0;
		cooldown = .5f;

		//UI
		healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
		AtkSlider = GameObject.Find ("AtkSlider").GetComponent<Slider>();
		CDSlider = GameObject.Find ("CDSlider").GetComponent<Slider> ();

		//player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update () {
		if (alive) {
			if (playerSeen) {
				attackPlayer ();
			} else {
				idle ();
			}
		} else{
			m_Animator.SetBool("Dead",true);
		}

		fix.z = 0.5f * Mathf.Cos (transform.eulerAngles.y * Mathf.Deg2Rad);
		fix.x = .5f * Mathf.Sin (transform.eulerAngles.y * Mathf.Deg2Rad);
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
		m_Animator.SetBool ("Fly", false);
		m_Animator.SetBool ("Walk", true);
		player = GameObject.FindGameObjectWithTag ("Player").transform;
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
		m_Animator.SetBool ("Fly", true);
		m_Animator.SetBool ("Walk", false);
		Quaternion rotation = Quaternion.LookRotation (enemyPath[pathNum].position - transform.position +(flight));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
		Vector2 pathDirection = enemyPath [pathNum].position - transform.position +(flight);
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