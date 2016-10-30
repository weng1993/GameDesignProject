using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	
	private float x;
	private float y;
	private float z;

	private Vector3 offset;

	void Start ()
	{
		offset = transform.position - player.transform.position;
		x = transform.rotation.x;
		z = transform.rotation.z;
	}

	void LateUpdate ()
	{
		transform.position = player.transform.position + offset;
		transform.rotation = Vector3(x, player.transform.rotation.y, z);
	}
}
