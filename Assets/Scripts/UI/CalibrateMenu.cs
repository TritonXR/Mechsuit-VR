using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateMenu : MonoBehaviour {

  public ArmController leftController, rightController;

  public GameObject pauseMenu;
  private GameObject calibrateMenu;
  private void Awake() {
    calibrateMenu = gameObject.transform.parent.gameObject;
    Calibrate();
    Time.timeScale = 0.0f;
  }

  public void TogglePauseMenu(object sender, ClickedEventArgs e) {
    if (calibrateMenu.activeSelf) {
      calibrateMenu.SetActive(false);
      Time.timeScale = 1.0f;
    } else {
      calibrateMenu.SetActive(true);
      Time.timeScale = 0.0f;
    }
  }

  public void QuitGame() {
    Debug.Log("Game quitting...");
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }

  public void Calibrate() {
    Debug.Log("Restarting calibration...");
    leftController.Reset();
    rightController.Reset();
  }

  public void Resume() {
    Debug.Log("Resuming...");
    calibrateMenu.SetActive(false);
    Time.timeScale = 1.0f;
  }
}
