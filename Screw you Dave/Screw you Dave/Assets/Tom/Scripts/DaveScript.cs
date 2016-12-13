using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DaveScript : MonoBehaviour {

	Rigidbody rigidbody;
	Vector3 velocity;
	float bulletImpulse = 20f;
	public GameObject projectile_prefab;
	bool schleem = false;
	Vector3 fix = new Vector3 (0,1.5f,0.5f);
	Vector3 fix2 = new Vector3 (.5f,2.5f,0);
	public GameObject claw_prefab;

	public Collider coll;
	public Rigidbody rb;
	public float startingTime;
	private float timeLeft;

	private int meleeDamage;
	private float meleeRange;
	private float attackTime;
	private float cooldown;

	private float specialCD;
	private float CDTime;

	public Slider healthSlider;
	public Slider AtkSlider;
	public Slider CDSlider;

	int layermask = 1 << 8;

	Animator m_Animator;

	private float x;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		timeLeft = startingTime;
		//bounciness 0 (likely included in player already)
		coll = GetComponent<BoxCollider>();
		PhysicMaterial material = new PhysicMaterial();
		material.bounciness = 0;
		coll.material = material;
		m_Animator = GetComponent<Animator>();

		meleeRange = 4;
		meleeDamage = 30;
		attackTime = .5f;
		cooldown = .5f;

		specialCD = 1;
		CDTime = 0;

		//UI
		healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
		AtkSlider = GameObject.Find ("AtkSlider").GetComponent<Slider>();
		CDSlider = GameObject.Find ("CDSlider").GetComponent<Slider> ();
	}


	void Update () {
		//base movement
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 10.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 10.0f;

		if(Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0){
			m_Animator.SetBool ("Walk",true);
		}
		else{
			m_Animator.SetBool ("Walk",false);
		}

		if (isGrounded ())
			//			transform.position = (new Vector3 (transform.position.x + x, 0, transform.position.z + z));
			transform.Translate (x, 0, z);

		//Switch Bodies
		schleem = Input.GetKeyDown("space");
		if (schleem) {
			m_Animator.SetTrigger ("ShleemAttempt");
			GameObject projectile = (GameObject)Instantiate (projectile_prefab, transform.position+fix,Camera.main.transform.rotation);
			projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward*bulletImpulse, ForceMode.Impulse);
		}

		//UI
		healthSlider.value = (this.gameObject.GetComponent<Health2>().health / (float)this.gameObject.GetComponent<Health2>().maxHealth);
		AtkSlider.value = 0;
		CDSlider.value = 0;

		fix.z = 0.5f * Mathf.Cos (Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad);
		fix.x = .5f * Mathf.Sin (Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad);

		if (this.gameObject.GetComponent<Health2>().health <= 0)
			SceneManager.LoadScene (0);
	}

	bool isGrounded() {
		//change 3rd var depending on object size
		return Physics.Raycast (transform.position, -Vector3.up, 0.6f, layermask);
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.CompareTag("Water"))
			transform.gameObject.GetComponent<Health2> ().adjustHealth (-2);
	}
}
