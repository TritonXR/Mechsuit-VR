using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamRotation : MonoBehaviour {
	// Use this for initialization
	void Start () {
    string deviceName = UnityEngine.XR.XRDevice.model;
    Debug.Log(deviceName);
    if (deviceName.Contains("Rift")) {
      gameObject.transform.localEulerAngles = new Vector3(36, 0, 0);
    }
	}
}
