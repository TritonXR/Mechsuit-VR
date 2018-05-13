using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour {

  public void QuitGame() {
    Debug.Log("Game quitting...");
    Application.Quit();
  }

  public void ReCalibrate() {
    Debug.Log("Restarting calibration...");
    ArmController.isCalibrated = false;
  }

  public void Resume() {
    Debug.Log("Resuming...");
    GameObject menu = GameObject.Find("CurvedCanvas");
    menu.SetActive(false);
  }
}
