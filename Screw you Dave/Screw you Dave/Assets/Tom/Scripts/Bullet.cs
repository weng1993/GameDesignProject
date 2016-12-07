using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour {

	public int damage = 30;
	private float lifespan = 1.5f;
	private bool collision = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
		if (lifespan <= 0 && !collision)
			Destroy (gameObject);
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Bird" || col.gameObject.tag == "Bear" || col.gameObject.tag == "Turtle") {
			collision = true;
			col.gameObject.GetComponent<Health2> ().adjustHealth (-damage);
			Destroy (gameObject);
			if (col.gameObject.GetComponent<Health2> ().health <= 0) {
				if (col.tag.ToLower () == "bird") {
					if (col.gameObject.GetComponent<BirdAi> () != null) {
						col.gameObject.GetComponent<BirdAi> ().alive = false;
						if (col.gameObject.GetComponent<BirdAi> ().home.GetComponent<EnemyHome> () != null) {
							Destroy (col.gameObject.GetComponent<BirdAi> ().home.GetComponent<EnemyHome> ());
						}
					}
				}
				if (col.tag.ToLower () == "bear") {
					if (col.gameObject.GetComponent<BearAi> () != null) {
						col.gameObject.GetComponent<BearAi> ().alive = false;
						if (col.gameObject.GetComponent<BearAi> ().home.GetComponent<EnemyHome> () != null) {
							Destroy (col.gameObject.GetComponent<BearAi> ().home.GetComponent<EnemyHome> ());
						}
					}
				}
				if (col.tag.ToLower () == "turtle") {
					if (col.gameObject.GetComponent<TurtleAi> () != null) {
						col.gameObject.GetComponent<TurtleAi> ().alive = false;
						if (col.gameObject.GetComponent<TurtleAi> ().home.GetComponent<EnemyHome> () != null) {
							Destroy (col.gameObject.GetComponent<TurtleAi> ().home.GetComponent<EnemyHome> ());
						}
					}
				}
				if (col.tag.ToLower() == "player")
						SceneManager.LoadScene (0);
			} 

		}
	}
}
