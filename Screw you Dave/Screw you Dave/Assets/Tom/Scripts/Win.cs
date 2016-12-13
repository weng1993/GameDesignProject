using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			if (col.gameObject.GetComponent<DaveScript> () != null) {
				//Win Game
			}
		}
	}

}
