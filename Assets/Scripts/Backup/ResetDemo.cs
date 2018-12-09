using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetDemo : MonoBehaviour {

	public Text resetText;

	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	//public SteamVR_TrackedObject trackedObj;

	//public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.GetKeyDown(KeyCode.R) || controller.GetPressDown (triggerButton)) {
		//	resetText.text = "RESETTING LEVEL...";
		//}
		//if(Input.GetKeyUp(KeyCode.R)) {
		//	SceneManager.LoadScene ("Main", LoadSceneMode.Single);
		//}
	}
}
