using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateMenu : MonoBehaviour {

  public CalibrateManager manager;

  public GameObject pauseMenu, calibrateMenu, HUDMenu;

  public UnityEngine.UI.Toggle firstLeft, firstRight, secondLeft, secondRight;
  public UnityEngine.UI.Button startGame;

  /// <summary>
  /// For hooking menu button to pause menu
  /// </summary>
  public SteamVR_TrackedController leftTrackedController, rightTrackedController;

  void Awake() {
    calibrateMenu = gameObject.transform.parent.gameObject;
    Calibrate();
    Time.timeScale = 0.0f;

    leftTrackedController.MenuButtonClicked += TogglePauseMenu;
    rightTrackedController.MenuButtonClicked += TogglePauseMenu;
  }

  /// <summary>
  /// Pulls up the pause menu when the user clicks the menu button.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public void TogglePauseMenu(object sender, ClickedEventArgs e) {
    if (pauseMenu.activeSelf) {
      pauseMenu.SetActive(false);
      HUDMenu.SetActive(true);
      Time.timeScale = 1.0f;
    } else if (!calibrateMenu.activeSelf) {
      pauseMenu.SetActive(true);
      HUDMenu.SetActive(false);
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
    manager.ResetCalibration();
    firstLeft.isOn = false;
    firstRight.isOn = false;
    secondLeft.isOn = false;
    secondRight.isOn = false;
    startGame.interactable = false;
  }

  public void Resume() {
    Debug.Log("Resuming...");
    calibrateMenu.SetActive(false);
    HUDMenu.SetActive(true);
    Time.timeScale = 1.0f;
  }

  public void ChangeScreen(bool isLeft, byte stage) {
    if (isLeft && stage == 1) {
      firstLeft.isOn = true;
    } else if (isLeft && stage == 2) {
      secondLeft.isOn = true;
    } else if (stage == 1) {
      firstRight.isOn = true;
    } else {
      secondRight.isOn = true;
    }

    if (manager.BothCalibrated) {
      startGame.interactable = true;
    }
  }
}