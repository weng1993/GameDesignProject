using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HumanAi : MonoBehaviour {

	public Rigidbody rb;

	private float attackRange;
	private float attackTime;
	private float cooldown;
	public Rigidbody bullet;
	public float bulletSpeed = 30;
	Vector3 fix = new Vector3 (0,1.25f,.5f);

	public bool playerSeen = false;

	public Transform player;
	private float speed = 6f;
	private float attackSpeed = 3f;
	private float rotationSpeed = 6f;

	public Transform[] enemyPath = new Transform[12];
	public int pathNum = 0;
	public bool alive = true;

	public GameObject home;

	Animator m_Animator;

	void Start () {

		for (int i = 0; i < 12; i++)
			enemyPath [i] = home.transform.GetChild (i);
		if (alive == false)
			Destroy (transform.gameObject.GetComponent<HumanAi> ().home.GetComponent<EnemyHome> ());

		m_Animator = GetComponent<Animator>();

		//freeze rotation so object will fly straight
		rb = GetComponent<Rigidbody> ();
		rb.freezeRotation = true;

		attackRange = 50f;
		attackTime = 0;
		cooldown = .5f;
	}

	void Update () {
		if (alive) {
			if (playerSeen) {
				attackPlayer ();
			} else {
				idle ();
			}
		} else{
			m_Animator.SetBool("Dead",true);
		}

		fix.z = 0.5f * Mathf.Cos (transform.eulerAngles.y * Mathf.Deg2Rad);
		fix.x = .5f * Mathf.Sin (transform.eulerAngles.y * Mathf.Deg2Rad);
	}

	bool isGrounded() {
		//change 3rd var depending on object size
		return Physics.Raycast (transform.position, -Vector3.up, 0.6f);
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "enemyPath"){
			if (pathNum < 11) {
				pathNum++;
			} else {
				pathNum = 0;
			}

		}
	}

	void attackPlayer(){
//		m_Animator.SetBool ("Fly", false);
//		m_Animator.SetBool ("Walk", true);
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		Quaternion rotation = Quaternion.LookRotation (player.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
		if(Vector3.Distance(transform.position,player.position) > attackRange) {
			transform.Translate (0,0,Time.deltaTime*speed);
		}
		else{
			if (attackTime > 0)
				attackTime -= Time.deltaTime;
			if (attackTime <= 0) {
				attack ();
				attackTime = cooldown;
			}
		}
	}

	void idle(){
//		m_Animator.SetBool ("Fly", true);
//		m_Animator.SetBool ("Walk", false);
		Quaternion rotation = Quaternion.LookRotation (enemyPath[pathNum].position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
		Vector2 pathDirection = enemyPath [pathNum].position - transform.position;
		float speedElement = Vector2.Dot (pathDirection.normalized,transform.forward);
		transform.Translate (0,0,Time.deltaTime*attackSpeed);
	}

	void attack() {
		Debug.DrawRay (transform.position+fix, transform.forward * attackRange, Color.cyan, 3);
		Rigidbody instantiatedBullet = Instantiate(bullet, transform.position+fix, transform.rotation) as Rigidbody;
		instantiatedBullet.velocity = transform.TransformDirection (new Vector3 (0, 0, bulletSpeed));
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Claw") {
			Vector3 dir = transform.forward;
			Destroy(col.gameObject);
			rb.velocity = (new Vector3(0,0,0));
			if (transform.gameObject.GetComponent<Health2> ().health <= 0)
				rb.AddForce (-dir * 5, ForceMode.Impulse);
			else
				rb.AddForce (-dir*15, ForceMode.Impulse);
		}
	}
}