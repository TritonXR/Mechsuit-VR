using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using System.IO;

public class MotionRecorder : MonoBehaviour {

  public Transform LeftController { get { return transform.GetChild(0); } }
  public Transform RightController { get { return transform.GetChild(1); } }

  public List<Vector3> controllerData;

  public int controllerIndex;

  public Dictionary<int, string> fileMap; // map from motion index to data file

  public StreamWriter file;

  // Use this for initialization
  void Start() {
    controllerData = new List<Vector3>();

    fileMap = new Dictionary<int, string>() {
      {0, @"D:\MechSuit-VR\MachineLearning\Data\MoveForward.txt" },
      {1, @"D:\MechSuit-VR\MachineLearning\Data\SummonSword.txt" },
      {2, @"D:\MechSuit-VR\MachineLearning\Data\SummonBow.txt" }
    };

    file = new StreamWriter(fileMap[0], true);
  }

  void OnDestroy() {
    if (file != null) {
      WriteData();
      file.Flush();
      file.Close();
    }
  }

  // Update is called once per frame
  void Update() {
    switch (controllerIndex) {
      case 0:
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand)) {
          WriteData();
        }

        if (SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.LeftHand)) {
          controllerData.Add(LeftController.localPosition);
          controllerData.Add(LeftController.localEulerAngles);
        }
        break;

      case 1:
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand)) {
          WriteData();
        }

        if (SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.RightHand)) {
          controllerData.Add(RightController.localPosition);
          controllerData.Add(RightController.localEulerAngles);
        }
        break;

      case 2: // both hands: use right hand to start/stop recording
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand)) {
          WriteData();
        }

        if (SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.RightHand)) {
          controllerData.Add(LeftController.localPosition);
          controllerData.Add(LeftController.localEulerAngles);
          controllerData.Add(RightController.localPosition);
          controllerData.Add(RightController.localEulerAngles);
        }
        break;
    }
  }

  public void WriteData() {
    //Record left controller data to txt file.
    file.WriteLine();
    for (int i = 0; i < controllerData.Count; i+= 2) {
      file.Write(controllerData[i].x + " " + controllerData[i].y + " " + controllerData[i].z + " "); // position
      file.Write(controllerData[i + 1].x + " " + controllerData[i + 1].y + " " + controllerData[i + 1].z); // rotation
      file.WriteLine();
    }
    controllerData.Clear();
  }

  public void Discard() {
    controllerData.Clear();
  }

  public void ChangeController(int index) {
    controllerIndex = index;
  }

  public void ChangeMotion(int index) {
    file.Flush();
    file.Close();
    file = new StreamWriter(fileMap[index], true);
  }
}
