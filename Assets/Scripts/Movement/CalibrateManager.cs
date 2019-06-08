using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monitors the two calibrations.
/// TODO: make it singleton
/// </summary>
public class CalibrateManager : MonoBehaviour {
  public ArmCalibration left, right;

  public CalibrateMenu menu;

  public bool BothCalibrated {
    get {
      return left.Calibrated && right.Calibrated;
    }
  }

  public void NotifyMenu(bool isLeft, CalibrationStage stage) {
    menu.ChangeScreen(isLeft, stage);
  }

  public void ResetCalibration() {
    left.Reset();
    right.Reset();
  }
}
