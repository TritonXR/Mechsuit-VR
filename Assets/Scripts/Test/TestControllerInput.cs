using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControllerInput : MonoBehaviour {
  public SteamVR_TrackedObject viveController;
  private Dictionary<string, Valve.VR.EVRButtonId> buttonDictionary;

  // Use this for initialization
  private SteamVR_Controller.Device DeviceInput {
    get { return SteamVR_Controller.Input((int)viveController.index); }
  }

  void Awake() {
    viveController = GetComponent<SteamVR_TrackedObject>();
  }

  void Start() {
    /*
    * k_EButton_System = 0,
    * k_EButton_ApplicationMenu = 1,
    * k_EButton_Grip = 2,
    * k_EButton_DPad_Left = 3,
    * k_EButton_DPad_Up = 4,
    * k_EButton_DPad_Right = 5,
    * k_EButton_DPad_Down = 6,
    * k_EButton_A = 7,
    * k_EButton_ProximitySensor = 31,
    * k_EButton_Axis0 = 32,
    * k_EButton_Axis1 = 33,
    * k_EButton_Axis2 = 34,
    * k_EButton_Axis3 = 35,
    * k_EButton_Axis4 = 36,
    * k_EButton_SteamVR_Touchpad = 32,
    * k_EButton_SteamVR_Trigger = 33,
    * k_EButton_Dashboard_Back = 2,
    * k_EButton_Max = 64,
    */
    buttonDictionary = new Dictionary<string, Valve.VR.EVRButtonId> {
      { "System", Valve.VR.EVRButtonId.k_EButton_System},
      { "ApplicationMenu", Valve.VR.EVRButtonId.k_EButton_ApplicationMenu},
      { "Grip", Valve.VR.EVRButtonId.k_EButton_Grip},
      { "Left", Valve.VR.EVRButtonId.k_EButton_DPad_Left},
      { "Up", Valve.VR.EVRButtonId.k_EButton_DPad_Up},
      { "Right", Valve.VR.EVRButtonId.k_EButton_DPad_Right},
      { "Down", Valve.VR.EVRButtonId.k_EButton_DPad_Down},
      { "A", Valve.VR.EVRButtonId.k_EButton_A},
      { "Proximity", Valve.VR.EVRButtonId.k_EButton_ProximitySensor},
      { "0", Valve.VR.EVRButtonId.k_EButton_Axis0},
      { "1", Valve.VR.EVRButtonId.k_EButton_Axis1},
      { "2", Valve.VR.EVRButtonId.k_EButton_Axis2},
      { "3", Valve.VR.EVRButtonId.k_EButton_Axis3},
      { "4", Valve.VR.EVRButtonId.k_EButton_Axis4},
      { "Touchpad", Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad},
      { "Trigger",  Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger},
      { "Back", Valve.VR.EVRButtonId.k_EButton_Dashboard_Back},
      { "Max", Valve.VR.EVRButtonId.k_EButton_Max}
    };
  }

  // Update is called once per frame
  void Update() {
    foreach (KeyValuePair<string, Valve.VR.EVRButtonId> pair in buttonDictionary) {
      Debug.Log(pair.Key + ": " + DeviceInput.GetAxis(pair.Value));
    }
  }
}
