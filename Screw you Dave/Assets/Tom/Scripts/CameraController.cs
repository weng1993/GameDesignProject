using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;

	void Start ()
	{
		offset = transform.position - player.transform.position;

	}

	void LateUpdate ()
	{
		transform.position = player.transform.position + offset;
		//Quaternion newview = new Quaternion (0, player.transform.rotation.y, 0, 0);
		//transform.rotation = newview;
	}
}