using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BearScript : MonoBehaviour {

	Rigidbody rigidbody;
	Vector3 velocity;
	float bulletImpulse = 20f;
	public GameObject projectile_prefab;
	bool schleem = false;
	Vector3 fix = new Vector3 (0,0.5f,0);
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

	Animator m_Animator;

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

		transform.Translate (x, 0, z);

		//Switch Bodies
		schleem = Input.GetKeyDown("space");
		if (schleem) {
			GameObject projectile = (GameObject)Instantiate (projectile_prefab, transform.position + fix,transform.rotation);
			projectile.GetComponent<Rigidbody>().AddForce(transform.forward*bulletImpulse, ForceMode.Impulse);
		}

		if (CDTime > 0)
			CDTime -= Time.deltaTime;
		if (CDTime <= 0) {
			if (Input.GetMouseButton (1)) {
				GameObject claw = (GameObject)Instantiate (claw_prefab, transform.position + fix2, transform.rotation);
				claw.GetComponent<Rigidbody> ().AddForce (transform.forward * bulletImpulse, ForceMode.Impulse);
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
	}

	void attack() {
		RaycastHit hit;
		m_Animator.SetTrigger ("Attack");
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		if (Physics.Raycast(transform.position, fwd, out hit, meleeRange) && (hit.transform.tag == "AIPlayer" || hit.transform.tag == "Bird" || hit.transform.tag == "Bear" || hit.transform.tag == "Turtle")) {
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
			}
		}
	}
}