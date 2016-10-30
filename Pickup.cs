using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	
	private Transform pt;
	private Transform ch;
	private float height;

	public bool holding = false;

	void Start () {
		pt = GetComponent<Transform>();
		height = pt.position.y;
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Submit")) {
			if (holding == true) {
				ch = pt.GetChild (pt.childCount - 1);
				ch.parent = null;
				holding = false;
			}
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ( "Pickup"))
		{
			if (Input.GetButton ("Fire1")) {
				ch = other.gameObject.transform;
				ch.SetParent (pt);
				Vector3 atop = new Vector3 (0.0f, height + 0.1f, 0f);
				ch.localPosition = atop;
				holding = true;
			}
		}
	}
}
