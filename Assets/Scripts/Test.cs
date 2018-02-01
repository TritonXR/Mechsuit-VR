using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
  private SteamVR_TrackedObject viveController;

  // Use this for initialization
  private SteamVR_Controller.Device DeviceInput {
    get { return SteamVR_Controller.Input ((int)viveController.index); }
  }

  void Awake() {
    viveController = GetComponent<SteamVR_TrackedObject> ();
  }


	// Update is called once per frame
	void Update () {
    Debug.Log ( DeviceInput.GetHairTrigger ());
	}
}
