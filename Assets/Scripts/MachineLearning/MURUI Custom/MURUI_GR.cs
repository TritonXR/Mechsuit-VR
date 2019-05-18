using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR;

public class MURUI_GR : MonoBehaviour {
    public Transform SteamCamera;
    public SteamVR_Behaviour_Pose trackedObj;
    public SteamVR_Action_Boolean triggerPulled = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

    private GestureRecognition gr;


    public bool isLeft;

    public string FileName {
        get {
            return isLeft ?
                "D:/MechSuit-VR/MachineLearning/Murui_Data/MSVR_Gestures_Left.dat" :
                "D:/MechSuit-VR/MachineLearning/Murui_Data/MSVR_Gestures_Right.dat";
        }
    }

    List<int> gestures;
    int currIndex;
    // Start is called before the first frame update
    void Start() {
        gr = new GestureRecognition();
        if (File.Exists(FileName)) {
            gr.loadFromFile(FileName);
            gestures = new List<int>() {
                0, 1, 2, 3, 4, 5
            };
            print("Loaded from file");
        } else {
            gestures = new List<int>() {
                gr.createGesture("Swing move"),
                gr.createGesture("Summon sword"),
                gr.createGesture("Summon bow"),
                gr.createGesture("Summon railgun"),
                gr.createGesture("Summon whip"),
                gr.createGesture("Summon gauntlets")
            };
            print("Created new file");
        }
    }

    private bool isRecording;
    private bool isRecognizing;

    // Update is called once per frame
    void Update() {
        if (!isRecognizing) {
            //Record Data
            Record(gestures[currIndex]);
        } else {
            //Recognize Data
            int result = Record();

            if (result != -2) {
                bool found = false;
                foreach (int gesture in gestures) {
                    if (result == gesture) {
                        print(string.Format("Gesture is {0}", gr.getGestureName(gesture)));
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    print("Not found");
                }
            }
        }

        //Train Data
        if (Input.GetKeyDown(KeyCode.T)) {
            Train();
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            SwitchMode();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gesture"></param>
    /// <returns>-2 if in the process of recording.</returns>
    int Record(int gesture = -1) {
        if (triggerPulled.GetState(trackedObj.inputSource)) {
            if (!isRecording) {
                Vector3 hmd_p = SteamCamera.position;
                Quaternion hmd_q = SteamCamera.rotation;
                gr.startStroke(hmd_p, hmd_q, gesture);
                isRecording = true;
                Debug.Log("Recording.");
            } else {
                // repeat the following while performing the gesture with your controller:
                Vector3 p = trackedObj.transform.localPosition;
                Quaternion q = trackedObj.transform.localRotation;
                gr.contdStroke(p, q);
                // ^ repeat while performing the gesture with your controller.
            }
        } else if (isRecording) {
            isRecording = false;
            Debug.Log("Done recording!!");
            return gr.endStroke();
        }

        return -2;
    }

    public void Train() {
        gr.setMaxTrainingTime(10000); // Set training time to 10 seconds.
        gr.startTraining();
        print("Training done");
    }

    public void SwitchMode() {
        isRecognizing = !isRecognizing;
        print(isRecognizing ? "Recognization mode" : "Training mode");
    }

    public void SwitchGesture(int index) {
        currIndex = index;
    }

    private void OnApplicationQuit() {
        gr.saveToFile(FileName);
    }
}
