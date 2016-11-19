using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Rigidbody rigidbody;
	Vector3 velocity;
	public GameObject projectile_prefab;
	float bulletImpulse = 20f;
	bool schleem = false;
	Vector3 fix = new Vector3 (0,0.5f,0);

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
	}
		

	void Update () {
		//base movement
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 10.0f;
		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

		//Switch Bodies
		schleem = Input.GetKeyDown("space");
		if (schleem) {
			GameObject projectile = (GameObject)Instantiate (projectile_prefab, transform.position + fix,transform.rotation);
			projectile.GetComponent<Rigidbody>().AddForce(transform.forward*bulletImpulse, ForceMode.Impulse);
		}
	}
}