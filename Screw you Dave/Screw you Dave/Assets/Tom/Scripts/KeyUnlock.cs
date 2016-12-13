using UnityEngine;
using System.Collections;

public class KeyUnlock : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Door") {
			Destroy(col.gameObject);
		}
	}
}
