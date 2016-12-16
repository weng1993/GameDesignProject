using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BearScript : MonoBehaviour {

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
		attackTime = 0;
		cooldown = .5f;

		specialCD = 1;
		CDTime = 0;

		//UI
		healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
		AtkSlider = GameObject.Find ("AtkSlider").GetComponent<Slider>();
		CDSlider = GameObject.Find ("CDSlider").GetComponent<Slider> ();

		Cursor.lockState = CursorLockMode.Locked;
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

		if (CDTime > 0)
			CDTime -= Time.deltaTime;
		if (CDTime <= 0) {
			if (Input.GetMouseButton (1)) {
				GameObject claw = (GameObject)Instantiate (claw_prefab, transform.position + fix, Camera.main.transform.rotation);
				claw.GetComponent<Rigidbody> ().AddForce (claw.transform.forward * bulletImpulse, ForceMode.Impulse);
				CDTime = specialCD;
				m_Animator.SetTrigger ("Claw");
			}
		}


		if (attackTime > 0)
			attackTime -= Time.deltaTime;
		if (attackTime <= 0) {
			if (Input.GetMouseButton (0)) {
				attack ();
				attackTime = cooldown;
			}
		}

		//UI
		healthSlider.value = (this.gameObject.GetComponent<Health2>().health / (float)this.gameObject.GetComponent<Health2>().maxHealth);
		AtkSlider.value = 1 - (attackTime / cooldown);
		CDSlider.value = 1 - (CDTime / specialCD);

		fix.z = 0.5f * Mathf.Cos (Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad);
		fix.x = .5f * Mathf.Sin (Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad);

		if (this.gameObject.GetComponent<Health2>().health <= 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		if (Input.GetMouseButtonDown (0)) {
			if (Cursor.lockState == CursorLockMode.None)
				Cursor.lockState = CursorLockMode.Locked;
		}
	}
		
	void attack() {
		RaycastHit hit;
		bool collided = false;
		m_Animator.SetTrigger ("Attack");

		Vector3 dir = Camera.main.transform.TransformDirection (Vector3.forward);
		Vector3 dir2 =Camera.main.transform.TransformDirection (Vector3.forward) + new Vector3 (0, .1f, 0);
		Vector3 dir3 =Camera.main.transform.TransformDirection (Vector3.forward) + new Vector3 (-.1f, 0, 0);
		Vector3 dir4 =Camera.main.transform.TransformDirection (Vector3.forward) + new Vector3 (0, -.1f, 0);
		Vector3 dir5 =Camera.main.transform.TransformDirection (Vector3.forward) + new Vector3 (.1f, 0, 0);
		Vector3[] dirs = { dir, dir2, dir3, dir4, dir5 };


		for (int i = 0; i < dirs.Length; i++) {
			//Debug.DrawRay (transform.position+fix, dirs[i] * meleeRange, Color.cyan, 3);
			if (Physics.Raycast(transform.position+fix, dirs[i], out hit, meleeRange) && (hit.transform.tag == "AIPlayer" || hit.transform.tag == "Bird" || hit.transform.tag == "Bear" || hit.transform.tag == "Turtle" || hit.transform.tag == "Human") && collided == false) {
				collided = true;
				hit.transform.gameObject.GetComponent<Health2>().adjustHealth (-meleeDamage);
				if (hit.transform.gameObject.GetComponent<Health2>().health <= 0){
					if (hit.transform.tag.ToLower() == "bird") {
						if(hit.transform.gameObject.GetComponent<BirdAi> () != null){
							hit.transform.gameObject.GetComponent<BirdAi> ().alive = false;
							if(hit.transform.gameObject.GetComponent<BirdAi> ().home.GetComponent<EnemyHome> () != null){
								Destroy (hit.transform.gameObject.GetComponent<BirdAi> ().home.GetComponent<EnemyHome> ());
							}
						}
					}
					if (hit.transform.tag.ToLower() == "bear") {
						if(hit.transform.gameObject.GetComponent<BearAi> () != null){
							hit.transform.gameObject.GetComponent<BearAi> ().alive = false;
							if (hit.transform.gameObject.GetComponent<BearAi> ().home.GetComponent<EnemyHome> () != null) {
								Destroy (hit.transform.gameObject.GetComponent<BearAi> ().home.GetComponent<EnemyHome> ());
							}
						}
					}
					if (hit.transform.tag.ToLower() == "turtle") {
						if(hit.transform.gameObject.GetComponent<TurtleAi> () != null){
							hit.transform.gameObject.GetComponent<TurtleAi> ().alive = false;
							if (hit.transform.gameObject.GetComponent<TurtleAi> ().home.GetComponent<EnemyHome> () != null) {
								Destroy (hit.transform.gameObject.GetComponent<TurtleAi> ().home.GetComponent<EnemyHome> ());
							}
						}
					}
					if (hit.transform.tag.ToLower() == "human") {
						if(hit.transform.gameObject.GetComponent<HumanAi> () != null){
							hit.transform.gameObject.GetComponent<HumanAi> ().alive = false;
							if(hit.transform.gameObject.GetComponent<HumanAi> ().home.GetComponent<EnemyHome> () != null){
								Destroy (hit.transform.gameObject.GetComponent<HumanAi> ().home.GetComponent<EnemyHome> ());
							}
						}
					}
				}
			}
		}
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