using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitFence : MonoBehaviour {
	
	public int fencePoints;
	// Use this for initialization
	void Start () {
		fencePoints = 3;
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Pickup"){
			GameObject.FindWithTag ("Player").GetComponent<BirdScript>().holding = false;
			if (fencePoints > 0) {
				fencePoints--;
				col.gameObject.SetActive (false);
			}
			if(fencePoints <= 0){
				SceneManager.LoadScene (0);
			}
		}
	}
}
