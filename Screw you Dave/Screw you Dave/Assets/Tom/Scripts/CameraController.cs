using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;
	public float speed = 3f;

	void Start ()
	{
		offset = transform.position - player.transform.position;
	}

	void LateUpdate ()
	{
		//offset = transform.position - player.transform.position;
		transform.position = player.transform.position + offset;
		transform.Translate(-Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);
		//transform.rotation = Quaternion.LookRotation(player.transform.position-transform.position);
	}
}