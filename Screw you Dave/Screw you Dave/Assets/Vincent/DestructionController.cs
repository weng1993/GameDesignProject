using UnityEngine;
using System.Collections;


public class DestructionController : MonoBehaviour {
	public GameObject remains;
	// Use this for initialization

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Claw") {
			//Destroy Claw
			Destroy(col.gameObject);
			//Explode
			Explode();
		}
	}
	void Explode(){
		Instantiate (remains, transform.position, transform.rotation);
		Destroy (gameObject);
//		Debug.Log ("start");
//		StartCoroutine (Delay ());
//		Debug.Log ("end2");
	}
//	IEnumerator Delay() {
//		Debug.Log ("in");
//		yield return new WaitForSeconds (1);
//		Debug.Log ("end");
//		remains.transform.Find("GameObject").GetComponent<SphereCollider>().enabled = false;
//		for (int i = 0; i < 9; i++) {
//			remains.transform.Find ("Rock (" + i + ")").GetComponent<BoxCollider> ().enabled = false;
//		}
//	}
}
