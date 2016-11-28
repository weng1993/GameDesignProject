/*using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public float turnSpeed = 4.0f;
	public Transform player;
	private Vector3 offsetX;
	private Vector3 offsetY;

	// Use this for initialization
	void Start () {
		offsetX = new Vector3 (0, 1f, -4f);
		offsetY = new Vector3 (0, 0, -4f);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		offsetX = Quaternion.AngleAxis (Input.GetAxis ("Mouse X") * turnSpeed, Vector3.up) * offsetX;
		offsetY = Quaternion.AngleAxis (Input.GetAxis ("Mouse Y") * turnSpeed, Vector3.left) * offsetY;
		transform.position = player.position + offsetX + offsetY;

		player.transform.Rotate(0, Input.GetAxis("Mouse X") * turnSpeed, 0);
		Vector3 pos = player.position + new Vector3 (0, 1, 0);
		transform.LookAt (pos);
	}
}
*/
using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
	public GameObject Target;
	public Transform t;
	public float RotateSpeed = 10,
	FollowDistance = 4,
	FollowHeight = 1;
	float RotateSpeedPerTime,
	DesiredRotationAngle,
	DesiredHeight,
	CurrentRotationAngle,
	CurrentHeight,
	Yaw,
	Pitch;
	Quaternion CurrentRotation;

	void LateUpdate()
	{
		RotateSpeedPerTime = RotateSpeed * Time.deltaTime;

		DesiredRotationAngle = Target.transform.eulerAngles.y;
		DesiredHeight = Target.transform.position.y + FollowHeight;
		CurrentRotationAngle = transform.eulerAngles.y;
		CurrentHeight = transform.position.y;

		CurrentRotationAngle = Mathf.LerpAngle(CurrentRotationAngle, DesiredRotationAngle, 0);
		CurrentHeight = Mathf.Lerp(CurrentHeight, DesiredHeight, 0);

		CurrentRotation = Quaternion.Euler(0, CurrentRotationAngle, 0);
		transform.position = Target.transform.position;
		transform.position -= CurrentRotation * Vector3.forward * FollowDistance;
		transform.position = new Vector3(transform.position.x, CurrentHeight, transform.position.z);

		Yaw = -Input.GetAxis("Mouse X") * RotateSpeedPerTime;
		Pitch = Input.GetAxis("Mouse Y") * RotateSpeedPerTime;
		//t.Rotate (0, -(Yaw*14.05f) , 0);
		t.Rotate(0, Input.GetAxis("Mouse X") * RotateSpeedPerTime * 14.05f, 0);
		transform.Translate(new Vector3(Yaw, -Pitch, 0));
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		Vector3 pos = Target.transform.position + new Vector3 (0, 1, 0);
		transform.LookAt (pos);
	}
}