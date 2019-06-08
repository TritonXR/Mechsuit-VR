using UnityEngine;

public class CalibrateMenu : MonoBehaviour {
    public CalibrateManager manager;

    public GameObject calibrateMenu, HUDMenu;

    public UnityEngine.UI.Toggle firstLeft, firstRight, secondLeft, secondRight;
    public UnityEngine.UI.Button startGame;


    void Awake() {
        Calibrate();
        Time.timeScale = 0.0f;
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
        GameManager.Instance.curvedUILaserBeam.SetActive(false);
    }

    public void ChangeScreen(bool isLeft, CalibrationStage stage) {
        if (isLeft && stage == CalibrationStage.shoulder) {
            firstLeft.isOn = true;
        } else if (isLeft && stage == CalibrationStage.arm) {
            secondLeft.isOn = true;
        } else if (stage == CalibrationStage.shoulder) {
            firstRight.isOn = true;
        } else {
            secondRight.isOn = true;
        }

        if (manager.BothCalibrated) {
            startGame.interactable = true;
        }
    }
}