using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			if (col.gameObject.GetComponent<DaveScript> () != null) {
				SceneManager.LoadScene (0);
			}
		}
	}

}
