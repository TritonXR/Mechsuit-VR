using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ArmCalibration : MonoBehaviour {
  public bool isLeft;
  public Transform controller;
  public CalibrateManager manager;

  private CalibrationStage stage;
  private ArmController armController;

  private Vector3 firstPosition, secondPosition;

  public bool Calibrated {
    get { return stage == CalibrationStage.arm; }
  }


  private void Start() {
    armController = GetComponent<ArmController>();
  }

  private void Update() {
    if (InputManager.Instance.GetButtonInput(ButtonInput.Calibrate, GetSource())) {
      Calibrate();
    }
  }

  public void Reset() {
    stage = CalibrationStage.none;
  }

  public void Calibrate() {
    if (stage == CalibrationStage.none) { // First check
      Debug.Log("Trigger pulled, expected at shoulder.");
      firstPosition = controller.position;
      // Move our empty shoulder gameobject to the player's assumed shoulder position.
      armController.playerShoulder.position = firstPosition;
      stage = CalibrationStage.shoulder;
    } else if (stage == CalibrationStage.shoulder) { // Second check
      Debug.Log("Trigger pulled, expected arm to be extended.");
      secondPosition = controller.position;
      armController.maxArmLength = Vector3.Distance(firstPosition, secondPosition);
      stage = CalibrationStage.arm;
    }
    manager.NotifyMenu(isLeft, stage);
  }

  private Hand GetSource() {
        return isLeft ? Hand.LeftHand : Hand.RightHand;
  }
}
