using UnityEngine;
using System.Collections;

public class Animalnteraction : MonoBehaviour {
	private float xPos;

	void Start (){
		xPos = gameObject.transform.position.x;
	}

	void OnTriggerEnter (Collider other){
		if (other.gameObject.layer != 9){
			if (other.gameObject.transform.position.x < xPos) {
				other.gameObject.transform.position =
					new Vector3 (other.gameObject.transform.position.x - 5f,
					other.gameObject.transform.position.y,
					other.gameObject.transform.position.z);
			} else {
				other.gameObject.transform.position =
					new Vector3 (other.gameObject.transform.position.x + 5f,
					other.gameObject.transform.position.y,
					other.gameObject.transform.position.z);
			}
		}
	}
}