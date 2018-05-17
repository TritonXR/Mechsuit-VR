using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisLockZOnly : MonoBehaviour {

	//	public float xlimit;
	//	public float ylimit;
	//	public float zlimit;

	public Transform HeadCam;

	// Use this for initialization
	void Start () {
		HeadCam = HeadCam.GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update () {
		//		Transform headCoords = gameObject.GetComponentInParent<Transform>();
		//		transform.rotation.x = 0;
		//		transform.rotation.y = headCoords.rotation.y;
		//		transform.rotation.z = 0;

		Vector3 eulerAngles = HeadCam.rotation.eulerAngles;
		//Vector3 eulerAngles = HeadCam.GetComponent<Transform>().rotation.eulerAngles;
		eulerAngles = new Vector3(-90, 90, eulerAngles.y);
		transform.rotation = Quaternion. Euler(eulerAngles);
		//transform.position = new Vector3(HeadCam.position.z, HeadCam.position.x, 0);
		//transform.position = new Vector3(HeadCam.position.x, HeadCam.position.z, 0);
		transform.position = new Vector3(HeadCam.position.x, HeadCam.position.y - 2.4f, HeadCam.position.z);
	}
}
