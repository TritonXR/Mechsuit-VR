using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAxisLock : MonoBehaviour {

	[SerializeField]
	private float xRotLimit;
	[SerializeField]
	private float zRotLimit;

//	public float xlimit;
//	public float ylimit;
//	public float zlimit;

	public Transform HeadCam;

	// Use this for initialization
	void Start () {
		HeadCam = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
//		Transform headCoords = gameObject.GetComponentInParent<Transform>();
//		transform.rotation.x = 0;
//		transform.rotation.y = headCoords.rotation.y;
//		transform.rotation.z = 0;

		Vector3 eulerAngles = HeadCam.rotation.eulerAngles;
		//if (eulerAngles.x <= xRotLimit && eulerAngles.x >= -xRotLimit && eulerAngles.z <= zRotLimit && eulerAngles.z >= -zRotLimit) {
		//	eulerAngles = new Vector3 (eulerAngles.x, eulerAngles.y, eulerAngles.z);
		//}
		//transform.rotation = Quaternion.Euler (eulerAngles);
		//return;

//		else if () {
//
//		}
//
		//else {
		//	transform.rotation = Quaternion.Euler (transform.eulerAngles);
		//}

		float newX;
		float newY;
		float newZ;

		if (eulerAngles.x <= xRotLimit && eulerAngles.x >= -xRotLimit) {
			newX = eulerAngles.x;
			Debug.Log ("X set to intermediate value "+newX);
		}
		else if (eulerAngles.x > xRotLimit) {
			newX = xRotLimit;
			Debug.Log ("X set to max positive value "+newX);
		}
		else {
			newX = -xRotLimit;
			Debug.Log ("X set to max negative value "+newX);
		}

		newY = eulerAngles.y;

		if (eulerAngles.z <= zRotLimit && eulerAngles.z >= -zRotLimit) {
			newZ = eulerAngles.z;
			Debug.Log ("Z set to intermediate value "+newZ);
		}
		else if (eulerAngles.z > zRotLimit) {
			newZ = zRotLimit;
			Debug.Log ("Z set to max positive value "+newZ);
		}
		else {
			newZ = -zRotLimit;
			Debug.Log ("Z set to max negative value "+newZ);
		}

		Quaternion newRotation = Quaternion.Euler (newX, newY, newZ);
		transform.rotation = newRotation;
	}
}