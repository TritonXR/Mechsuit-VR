using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MURUI_GR : MonoBehaviour {
    public Transform SteamCamera;
    public SteamVR_Behaviour_Pose trackedObj;
    public SteamVR_Action_Boolean triggerPulled = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

    private GestureRecognition gr;

    private int swingMove;
    private int summonRailgun;
    private int summonWhip;


    // Start is called before the first frame update
    void Start() {
        gr = new GestureRecognition();
        gr.loadFromFile("D:/MechSuit-VR/MachineLearning/Murui_Data/MSVR_Gestures.dat");
        swingMove = gr.createGesture("Swing move");
        summonRailgun = gr.createGesture("Summon railgun");
        summonWhip = gr.createGesture("Summon whip");

    }

    private bool isRecording;
    private bool isRecognizing;

    // Update is called once per frame
    void Update() {

        if (!isRecognizing) {
            //Record Data
            Record(swingMove);
        } else {
            //Recognize Data
            int result = Record();
            if (result == swingMove) {
                Debug.Log("This gesture is: swingMove");
            }
        }

        //Train Data
        if (Input.GetKeyDown(KeyCode.T)) {
            Train();
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            if (!isRecognizing) 
            {
                isRecognizing = true;
                Debug.Log("True.");
            }

            else {
                isRecognizing = false;
                Debug.Log("False.");
            }
        }
    }

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
                Debug.Log("Still recording.");
            }
        } else if (isRecording) {
            isRecording = false;
            Debug.Log("Done recording!!");
            return gr.endStroke();

        }

        return -1;
    }

    void Train() {
        gr.setMaxTrainingTime(10000); // Set training time to 10 seconds.
        gr.startTraining();
        print("Training done");
    }

    private void OnApplicationQuit() {
        gr.saveToFile("D:/MechSuit-VR/MachineLearning/Murui_Data/MSVR_Gestures.dat");
    }
}
