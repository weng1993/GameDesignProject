using UnityEngine;
using System.Collections;

public class WaterInteraction : MonoBehaviour {
	public Vector3 playerPos;
	void Start () {
		playerPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		playerPos = gameObject.transform.position;
	}

	void onTriggerEnter (Collider other){
		if (other.gameObject.CompareTag ("Water")) {
			//if (gameObject.CompareTag ("Player")) {
				playerPos = new Vector3 (playerPos.x - 2.5f, 300.5f, playerPos.z);
				gameObject.transform.position = playerPos;
			//}
		}
	}
}
