using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour {

	private SteamVR_TrackedObject viveController;

	// Use this for initialization
	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input ((int)viveController.index); }
	}

	void Awake() {
		viveController = GetComponent<SteamVR_TrackedObject> ();
	}

	// Update is called once per frame
	void Update () {
		if (Controller == null) {
			Debug.Log ("Null controller value, attempting to fetch");
			viveController = GetComponent<SteamVR_TrackedObject> ();
			if (Controller == null) {
				Debug.Log ("Fetch failed");
				return;
			}
		}
		if (Controller.GetHairTriggerDown ()) {
			Debug.Log ("Trigger pulled");
		}
	}
}
