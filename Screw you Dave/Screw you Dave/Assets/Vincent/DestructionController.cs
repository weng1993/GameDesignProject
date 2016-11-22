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
		//remains.transform.Find("GameObject").GetComponent<SphereCollider>().enabled = false;
	}
}
