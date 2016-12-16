using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
	public GameObject Target;
	public Transform t;
	public Transform mesh;
	public float RotateSpeed = 10,
	FollowDistance = 4,
	FollowHeight = 1;
	float RotateSpeedPerTime,
	DesiredRotationAngle,
	DesiredHeight,
	CurrentRotationAngle,
	CurrentHeight,
	Yaw,
	Pitch,
	xRot;
	Quaternion CurrentRotation;
	Quaternion turtleRotation;

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
		t.rotation = Quaternion.Euler (0, CurrentRotationAngle, 0);
		if (Target.GetComponent<TurtleScript> () != null) {
			turtleRotation = transform.rotation * Quaternion.Euler (0, -90, 0);
			mesh.rotation = turtleRotation;
		}
		else
			mesh.rotation = transform.rotation;
		//		t.Rotate (Vector3.left * 23.757f);
		transform.Translate(new Vector3(Yaw, -Pitch, 0));
		//		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		Vector3 pos = Target.transform.position + new Vector3 (0, 2, 0);
		transform.LookAt (pos);
	}
}