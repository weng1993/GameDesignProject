using UnityEngine;
using System.Collections;

public class GunShot : MonoBehaviour {

	public Rigidbody bullet;
	public float speed = 1000;
	Vector3 fix = new Vector3 (0,1.25f,1f);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		fix.z = 1f * Mathf.Cos (transform.eulerAngles.y * Mathf.Deg2Rad);
		fix.x = 1f * Mathf.Sin (transform.eulerAngles.y * Mathf.Deg2Rad);

		if (Input.GetMouseButtonDown(0)) {
			Rigidbody instantiatedBullet = Instantiate(bullet, transform.position+fix, Camera.main.transform.rotation) as Rigidbody;
			instantiatedBullet.velocity = Camera.main.transform.TransformDirection (new Vector3 (0, 0, speed));
		}
	
	}
}
