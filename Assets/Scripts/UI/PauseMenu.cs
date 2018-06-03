using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

  public SteamVR_TrackedController leftTrackedController, rightTrackedController;

  public GameObject calibrateMenu;
  private GameObject pauseMenu;
  
  private void Awake() {
    pauseMenu = gameObject.transform.parent.gameObject;
    leftTrackedController.MenuButtonClicked += TogglePauseMenu;
    rightTrackedController.MenuButtonClicked += TogglePauseMenu;
    Time.timeScale = 0.0f;
  }

  public void TogglePauseMenu(object sender, ClickedEventArgs e) {
    if (pauseMenu.activeSelf) {
      pauseMenu.SetActive(false);
      Time.timeScale = 1.0f;
    } else if (!calibrateMenu.activeSelf) {
      pauseMenu.SetActive(true);
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

  public void ReCalibrate() {
    Debug.Log("Restarting calibration...");
    pauseMenu.SetActive(false);
    calibrateMenu.SetActive(true);
  }

  public void Resume() {
    Debug.Log("Resuming...");
    pauseMenu.SetActive(false);
    Time.timeScale = 1.0f;
  }
}
