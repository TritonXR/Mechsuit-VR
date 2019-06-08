using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public GameObject pauseMenu, HUDMenu, calibrateMenu;
    public GameObject curvedUILaserBeam;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Update() {
        if (InputManager.Instance.GetButtonInput(ButtonInput.PauseGame, Hand.Any)) {
            TogglePauseMenu();
        }
    }

    /// <summary>
    /// Pulls up the pause menu when the user clicks the menu button.
    /// </summary>
    public void TogglePauseMenu() {
        if (pauseMenu.activeSelf) { // want to hide pause menu
            curvedUILaserBeam.SetActive(false);
            pauseMenu.SetActive(false);
            HUDMenu.SetActive(true);
            Time.timeScale = 1.0f;
        } else if (!calibrateMenu.activeSelf) {
            curvedUILaserBeam.SetActive(true);
            pauseMenu.SetActive(true);
            HUDMenu.SetActive(false);
            Time.timeScale = 0.0f;
        }
    }
}
