using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisLock : MonoBehaviour {

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
		eulerAngles = new Vector3(0, eulerAngles.y, eulerAngles.z);
		transform.rotation = Quaternion. Euler(eulerAngles);
	}
}
