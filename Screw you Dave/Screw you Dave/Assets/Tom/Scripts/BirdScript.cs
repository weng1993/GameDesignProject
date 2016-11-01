using UnityEngine;
using System.Collections;

public class BirdScript : MonoBehaviour {
	Rigidbody rigidbody;
	Vector3 velocity;
	float bulletImpulse = 5f;
	public GameObject projectile_prefab;
	bool schleem = false;
	Vector3 fix = new Vector3 (0,1,.5f);

	public Collider coll;
	public Rigidbody rb;
	public float startingTime;
        public float maxHeight;
        //make layer for all terrain in scene
        public int terrainLayer;
	private float timeLeft;


	private Transform pt;
	private Transform ch;
	private float height;
	public bool holding = false;
	private Vector3 offset; 

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
		offset = new Vector3 (0.0f, height + 0.1f, 0f);
	}

	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * 10;
		//Switch Bodies
		schleem = Input.GetKeyDown(KeyCode.X);
		if (schleem) {
			GameObject projectile = (GameObject)Instantiate (projectile_prefab, transform.position + fix,transform.rotation);
			projectile.GetComponent<Rigidbody>().AddForce(transform.forward*bulletImpulse, ForceMode.Impulse);
		}



		//update flight time
		timeLeft -= Time.deltaTime;

		//base movement
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 5.0f;

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

		//regain flight time when touching ground
		if (isGrounded ()) {
			timeLeft = startingTime;
			rb.useGravity = false;
		}

		//out of flight time
		if (timeLeft <= 0) {
			//re-enable gravity to make object fall
			rb.useGravity = true;
		}
		else {
                        //if within max height allow ascension
			if (Physics.Raycast (transform.position, Vector3.down, maxHeight, 1 << terrainLayer)) {
				//if flight time left use j to ascend and k to descend
				if (Input.GetKey ("j")) {
					transform.Translate (0, 0.1f, 0);
				}
			} 
			if (Input.GetKeyUp ("j")) {
				transform.Translate (0, 0, 0);
			}

			if (Input.GetKey ("k")) {
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
			}
			ch.position = pt.position + offset;
		}

	}

	void FixedUpdate() {
		rigidbody.MovePosition (rigidbody.position + velocity * Time.fixedDeltaTime);
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
				ch = other.gameObject.transform;
				ch.SetParent (pt);
				ch.localPosition = offset;
				holding = true;
			}
		}
	}

}