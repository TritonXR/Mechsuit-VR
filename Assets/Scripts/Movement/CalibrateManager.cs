using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateManager : MonoBehaviour {
  public ArmController left, right;

  public CalibrateMenu menu;

  public bool BothCalibrated {
    get {
      return left.IsCalibrated && right.IsCalibrated;
    }
  }

  public void NotifyMenu(bool isLeft, byte stage) {
    menu.ChangeScreen(isLeft, stage);
  }

  public void ResetCalibration() {
    left.Reset();
    right.Reset();
  }
}
