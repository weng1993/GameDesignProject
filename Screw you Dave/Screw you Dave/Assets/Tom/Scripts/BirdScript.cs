using UnityEngine;
using System.Collections;

public class BirdScript : MonoBehaviour {
	Rigidbody rigidbody;
	Vector3 velocity;
	float bulletImpulse = 20f;
	public GameObject projectile_prefab;
	bool schleem = false;
	Vector3 fix = new Vector3 (0,0.5f,0);

	public Collider coll;
	public Rigidbody rb;
	public float startingTime;
	private float timeLeft;


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

	}

	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * 10;
		//Switch Bodies
		schleem = Input.GetKeyDown(KeyCode.X);
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
			if (Input.GetKey ("j")) {
				rb.useGravity = false;
				flight = true;
				transform.Translate (0, 0.1f, 0);
			} 
			else if (Input.GetKeyUp ("j")) {
				transform.Translate (0, 0, 0);
			}

			if (Input.GetKey ("k")) {
				rb.useGravity = false;
				flight = true;
				transform.Translate (0, -0.1f, 0);
			} 
			else if (Input.GetKeyUp ("k")) {
				transform.Translate (0, 0, 0);
			}
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
			if (Input.GetButton ("Fire1")) {
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