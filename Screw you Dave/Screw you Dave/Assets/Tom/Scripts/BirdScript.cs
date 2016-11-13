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

	public bool flight = false;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		timeLeft = startingTime;
		//bounciness 0 (likely included in player already)
		coll = GetComponent<BoxCollider>();
		PhysicMaterial material = new PhysicMaterial();
		material.bounciness = 0;
		coll.material = material;

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

		//update flight time
		timeLeft -= Time.deltaTime;

		//base movement
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 10.0f;

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

		//regain flight time when touching ground
		//might not need this
		if (isGrounded ()) {
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
				rb.useGravity = false;
				flight = true;
				Vector3 v = rb.velocity;
				v.y = 0;
				rb.velocity = v;
				transform.Translate (0, 0.1f, 0);
			} 
			else if (Input.GetMouseButtonUp (1)) {
			//	transform.Translate (0, 0, 0);
				rb.useGravity = true;
			}

			//if (Input.GetKey ("k")) {
			//	rb.useGravity = false;
			//	flight = true;
			//	transform.Translate (0, -0.1f, 0);
			//} 
			//else if (Input.GetKeyUp ("k")) {
			//	transform.Translate (0, 0, 0);
			//}
		}


		if (holding == true) {
			ch = pt.GetChild (pt.childCount - 1);
			if (Input.GetButtonDown ("Submit")) {
				ch.parent = null;
				holding = false;
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
			}
		}
	}
		
	bool isGrounded() {
		//change 3rd var depending on object size
		return Physics.Raycast (transform.position, -Vector3.up, 0.6f);
	}

	//Bird Stuff
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ( "Pickup"))
		{
			if (Input.GetMouseButton (2)) {
				if(!holding){
					ch = other.gameObject.transform;
					ch.SetParent (pt);
					ch.localPosition = offset;
					holding = true;
				}
			}
		}
	}
}