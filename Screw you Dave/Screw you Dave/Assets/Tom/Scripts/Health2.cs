using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health2 : MonoBehaviour {

	public int health;
	public int maxHealth;
	private Image healthBar;


	// Use this for initialization
	void Start () {
		maxHealth = 100;
		health = maxHealth;
		healthBar = transform.FindChild ("HealthCanvas").FindChild ("HealthBG").FindChild ("Health").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void adjustHealth(int change) {
		health += change;

		if (health < 0)
			health = 0;
//		if (health = 0)
//			call whatever function for dead enemy

		healthBar.fillAmount = ((float)health / (float)maxHealth);
	}
}