using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BirdScript : MonoBehaviour {
	Rigidbody rigidbody;
	Vector3 velocity;
	float bulletImpulse = 20f;
	public GameObject projectile_prefab;
	public GameObject claw_prefab;
	bool schleem = false;
	Vector3 fix = new Vector3 (0,0.5f,0);

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

	private Transform pt;
	private Transform ch;
	private float height;
	public bool holding = false;
	private Vector3 offset; 
	private Vector3 placeOffset;

	int layermask = 1 << 8;

	public bool flight = false;

	private Vector3 prevPos;

	Animator m_Animator;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		startingTime = 5;
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

		//Picking Up Script
		pt = GetComponent<Transform>();
		height = pt.position.y;
		offset = new Vector3 (0.0f,0.1f, -1.0f);

		//combat
		meleeDamage = 10;
		meleeRange = 2;
		attackTime = 0;
		cooldown = .5f;

		//UI
		healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
		AtkSlider = GameObject.Find ("AtkSlider").GetComponent<Slider>();
		CDSlider = GameObject.Find ("CDSlider").GetComponent<Slider> ();

		prevPos = transform.position;

		Cursor.lockState = CursorLockMode.Locked;
	}

	void FixedUpdate(){
		//base movement
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 10.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 10.0f;

		//Set Walk Animation
		if(Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0){
			if(isGrounded()){
				m_Animator.SetBool ("Walk",true);
			}
		}
		else{
			m_Animator.SetBool ("Walk",false);
		}

		transform.Translate (x, 0, z);

		//update flight time
		timeLeft -= Time.deltaTime;

		//regain flight time when touching ground
		//might not need this
		if (isGrounded ()) {
			m_Animator.SetBool ("Fly", false);
			timeLeft = startingTime;
		}

		//out of flight time
		if (timeLeft <= 0) {
			//re-enable gravity to make object fall
			rb.useGravity = true;
			flight = true;
		}
		else {
			//if flight time left use j to ascend and k to descend
			//remove gravity when in flight
			if (Input.GetMouseButton (1)) {
				m_Animator.SetBool ("Fly", true);
				rb.useGravity = false;
				flight = true;
				Vector3 v = rb.velocity;
				v.y = 0;
				rb.velocity = v;
				transform.Translate (0, 0.1f, 0);
			} 
			else if (!Input.GetMouseButton (1)) {
				//	transform.Translate (0, 0, 0);
				rb.useGravity = true;
			}
		}
		float yDif = transform.position.y - prevPos.y;
		Camera.main.transform.Translate (0, yDif, 0);
		prevPos = transform.position;
	}

	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * 10;
		//Switch Bodies
		schleem = Input.GetKeyDown("space");
		if (schleem) {
			GameObject projectile = (GameObject)Instantiate (projectile_prefab, transform.position+fix,transform.rotation);
			projectile.GetComponent<Rigidbody>().AddForce(transform.forward*bulletImpulse, ForceMode.Impulse);
		}

		//Makes sure bird stays on ground at switch
		if(!flight){
			rb.useGravity = true;
		}


		if (holding == true) {
			ch = pt.GetChild (pt.childCount - 1);
			if (Input.GetKeyDown ("e")) {
				ch.parent = null;
				holding = false;
				m_Animator.SetTrigger ("PutDown");
			} else {
				ch.localPosition = offset;
			}
		}

		if (attackTime > 0)
			attackTime -= Time.deltaTime;
		if (attackTime <= 0) {
			if (Input.GetMouseButton (0)) {
				attack ();
				attackTime = cooldown;
			}
		}

		//UI
		healthSlider.value = (this.gameObject.GetComponent<Health2>().health / (float)this.gameObject.GetComponent<Health2>().maxHealth);
		AtkSlider.value = 1 - (attackTime / cooldown);
		CDSlider.value = (timeLeft / startingTime);

		if (Input.GetMouseButtonDown (0)) {
			if (Cursor.lockState == CursorLockMode.None)
				Cursor.lockState = CursorLockMode.Locked;
		}
	}

	void attack() {
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		if (Physics.Raycast(transform.position, fwd, out hit, meleeRange) && (hit.transform.tag == "AIPlayer" || hit.transform.tag == "Bird" || hit.transform.tag == "Bear" || hit.transform.tag == "Turtle")) {
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
			}
		}
	}
		
	bool isGrounded() {
		//change 3rd var depending on object size
		return Physics.Raycast (transform.position, -Vector3.up, 0.6f, layermask);
	}

	//Bird Stuff
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ( "Pickup"))
		{
			if (Input.GetKey ("e")) {
				if(!holding){
					m_Animator.SetTrigger ("PickUp");
					ch = other.gameObject.transform;
					ch.SetParent (pt);
					ch.localPosition = offset;
					holding = true;
				}
			}
		}
	}
}