using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

  public GameObject calibrateMenu, pauseMenu, HUDMenu;

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
    pauseMenu.SetActive(false);
    calibrateMenu.SetActive(true);
    calibrateMenu.GetComponent<CalibrateMenu>().Calibrate();
  }

  public void Resume() {
    Debug.Log("Resuming...");
    pauseMenu.SetActive(false);
    HUDMenu.SetActive(true);
    Time.timeScale = 1.0f;
  }
}
