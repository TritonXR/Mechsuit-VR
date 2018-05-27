using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour {

  public ArmController leftController, rightController;

  public void QuitGame() {
    Debug.Log("Game quitting...");
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }

  public void ReCalibrate() {
    Debug.Log("Restarting calibration...");
    leftController.Reset();
    rightController.Reset();
  }

  public void Resume() {
    Debug.Log("Resuming...");
    GameObject menu = this.gameObject.transform.parent.gameObject;
    menu.SetActive(false);
  }
}
