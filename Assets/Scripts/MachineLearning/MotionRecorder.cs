using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using System.IO;

public class MotionRecorder : MonoBehaviour {

  public Transform LeftController { get { return transform.GetChild(0); } }
  public Transform RightController { get { return transform.GetChild(1); } }

  public List<Vector3> leftControllerData;
  public List<Vector3> rightControllerData;

  public int controllerIndex;

  public StreamWriter file;

  // Use this for initialization
  void Start() {
    leftControllerData = new List<Vector3>();
    rightControllerData = new List<Vector3>();
    file = new System.IO.StreamWriter(@"D:\MechSuit-VR\NNData\test.txt");
  }

  // Update is called once per frame
  void Update() {
    switch (controllerIndex) {
      case 0:
        //Record left controller data to txt file.
        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand)) {
          file.WriteLine();
          foreach (Vector3 vector in leftControllerData) {
            file.WriteLine(vector.x + " " + vector.y + " " + vector.z);
          }
          leftControllerData.Clear();
        }

        if (SteamVR_Input._default.inActions.GrabPinch.GetState(SteamVR_Input_Sources.LeftHand)) {
          leftControllerData.Add(LeftController.localPosition);
          Debug.Log("Pinched!");

        }
        break;
      case 1:
        //Record right controller data to txt file.
        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand)) {
          file.WriteLine();
          foreach (Vector3 vector in rightControllerData) {
            file.WriteLine(vector.x + " " + vector.y + " " + vector.z);
          }
          rightControllerData.Clear();
        }

        if (SteamVR_Input._default.inActions.GrabPinch.GetState(SteamVR_Input_Sources.RightHand)) {
          rightControllerData.Add(RightController.localPosition);
          Debug.Log("Pinched!");
          Debug.Log(leftControllerData[0]);
        }
        break;
      case 2:
        //Record both controller data to txt file.
        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand)) {
          file.WriteLine();
          //TODO: try and then see if we need to switch to alternating data
          foreach (Vector3 vector in rightControllerData) {
            file.WriteLine(vector.x + " " + vector.y + " " + vector.z);
          }
          rightControllerData.Clear();

          foreach (Vector3 vector in leftControllerData) {
            file.WriteLine(vector.x + " " + vector.y + " " + vector.z);
          }
          leftControllerData.Clear();
        }

        if (SteamVR_Input._default.inActions.GrabPinch.GetState(SteamVR_Input_Sources.RightHand)) {
          leftControllerData.Add(LeftController.localPosition);
          rightControllerData.Add(RightController.localPosition);
          Debug.Log("Pinched!");
          Debug.Log(leftControllerData[0]);
        }
        break;
    }
  }

  public void Discard() {
    leftControllerData.Clear();
    rightControllerData.Clear();
  }

  public void ChangeController(int index) {
    controllerIndex = index;
  }
}
