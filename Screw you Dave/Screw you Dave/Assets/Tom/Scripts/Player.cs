using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Rigidbody rigidbody;
	Vector3 velocity;
	float bulletImpulse = 5f;
	public GameObject projectile_prefab;
	bool schleem = false;
	Vector3 fix = new Vector3 (0,1,.5f);

	public Collider coll;
	public Rigidbody rb;
	public float startingTime;
	private float timeLeft;


	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		timeLeft = startingTime;
		//bounciness 0 (likely included in player already)
		coll = GetComponent<BoxCollider>();
		PhysicMaterial material = new PhysicMaterial();
		material.bounciness = 0;
		coll.material = material;
	}
		

	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * 10;
		//Switch Bodies
		schleem = Input.GetKeyDown(KeyCode.X);
		if (schleem) {
			GameObject projectile = (GameObject)Instantiate (projectile_prefab, transform.position + fix,transform.rotation);
			projectile.GetComponent<Rigidbody>().AddForce(transform.forward*bulletImpulse, ForceMode.Impulse);
		}
	}

	void FixedUpdate() {
		rigidbody.MovePosition (rigidbody.position + velocity * Time.fixedDeltaTime);
	}
}