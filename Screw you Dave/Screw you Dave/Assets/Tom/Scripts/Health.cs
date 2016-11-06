using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int maxHealth = 100;
	public int currHealth = 100;
	private Texture2D healthBar;

	private float healthBarLength;
	public GameObject target;

	// Use this for initialization
	void Start () {
		healthBarLength = Screen.width / 10;
		healthBar = new Texture2D (650, 10);
		healthBarColor ();
	}
	
	// Update is called once per frame
	void Update () {
		adjustHealth (0);

//		Vector3 Pos = Camera.main.WorldToScreenPoint (target.transform.position);
//		transform.position = Pos;
	}

	void healthBarColor() {
		Color barColor = Color.green;
		Color[] barArray = healthBar.GetPixels ();

		for (int i = 0; i < barArray.Length; i++) {
			barArray [i] = barColor;
		}

		healthBar.SetPixels (barArray);
		healthBar.Apply ();
	}

	void OnGUI() {
		GUI.Box (new Rect (10, 10, healthBarLength, 10), healthBar);
	}

	public void adjustHealth(int change) {
		currHealth += change;

		if (currHealth < 0)
			currHealth = 0;
		healthBarLength = (Screen.width / 10) * (currHealth / (float)maxHealth);
	}
}
