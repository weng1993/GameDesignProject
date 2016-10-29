using UnityEngine;
using System.Collections;

public class Schleem : MonoBehaviour {

	//Tom
	float bulletImpulse = 5f;
	public GameObject projectile_prefab;
	bool schleem = false;
	Vector3 fix = new Vector3 (0,1,.5f);


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		schleem = Input.GetKeyDown(KeyCode.X);
		if (schleem) {
			GameObject projectile = (GameObject)Instantiate (projectile_prefab, transform.position + fix,transform.rotation);
			projectile.GetComponent<Rigidbody>().AddForce(transform.forward*bulletImpulse, ForceMode.Impulse);
		}
	}
}
