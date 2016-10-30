using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//make sure player object + terrain have 0 bounciness, unless have 
	//grounded check and different movement controls
	public Collider coll;
	public Rigidbody rb;
	public float startingTime;
	private float timeLeft;

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
	}

	void Update () {

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
			//if flight time left use j to ascend and k to descend
			if (Input.GetKey ("j")) {
				transform.Translate (0, 0.1f, 0);
			} 
			else if (Input.GetKeyUp ("j")) {
				transform.Translate (0, 0, 0);
			}

			if (Input.GetKey ("k")) {
				transform.Translate (0, -0.1f, 0);
			} 
			else if (Input.GetKeyUp ("k")) {
				transform.Translate (0, 0, 0);
			}
		}

	}

	bool isGrounded() {
		//change 3rd var depending on object size
		return Physics.Raycast (transform.position, -Vector3.up, 0.6f);
	}
}
