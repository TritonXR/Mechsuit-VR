using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameManager : MonoBehaviour {
  public GameObject pauseMenu, HUDMenu, calibrateMenu;

  // Update is called once per frame
  void Update() {
    if (SteamVR_Actions._default.PauseGame.GetStateDown(SteamVR_Input_Sources.Any)) {
      TogglePauseMenu();
    }
  }

  /// <summary>
  /// Pulls up the pause menu when the user clicks the menu button.
  /// </summary>
  public void TogglePauseMenu() {
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
}
