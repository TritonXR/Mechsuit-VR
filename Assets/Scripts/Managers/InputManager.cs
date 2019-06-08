using System.Collections;
using System.Reflection;
using UnityEngine;
using Valve.VR;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }

    public Transform steamCamera;
    public Transform leftController, rightController;

    private GestureRecognition leftGR;
    private GestureRecognition rightGR;

    private readonly string leftFileName = "D:/MechSuit-VR/MachineLearning/Murui_Data/MSVR_Gestures_Left.dat";
    private readonly string rightFileName = "D:/MechSuit-VR/MachineLearning/Murui_Data/MSVR_Gestures_Right.dat";

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        leftGR = InitializeGR(leftFileName);
        rightGR = InitializeGR(rightFileName);
    }

    #region Button input
    public bool GetButtonInput(ButtonInput input, Hand hand = Hand.Any) {
        PropertyInfo[] properties = typeof(SteamVR_Input_ActionSet_default).GetProperties();
        foreach (PropertyInfo property in properties) {
            if (property.Name == input.ToString()) {
                return ((SteamVR_Action_Boolean)property.GetValue(SteamVR_Actions._default)).GetStateDown(GetInputSource(hand));
            }
        }

        return false;
    }

    public bool GetButtonInputUp(ButtonInput input, Hand hand = Hand.Any) {
        PropertyInfo[] properties = typeof(SteamVR_Input_ActionSet_default).GetProperties();
        foreach (PropertyInfo property in properties) {
            if (property.Name == input.ToString()) {
                return ((SteamVR_Action_Boolean)property.GetValue(SteamVR_Actions._default)).GetStateUp(GetInputSource(hand));
            }
        }

        return false;
    }


    private SteamVR_Input_Sources GetInputSource(Hand hand) {
        return (SteamVR_Input_Sources)System.Enum.Parse(typeof(SteamVR_Input_Sources), hand.ToString(), true);
    }
    #endregion

    private GestureRecognition InitializeGR (string fileName) {
        GestureRecognition gr = new GestureRecognition();
        gr.loadFromFile(fileName);
        print("Loaded from file");
        return gr;
    }

    /// <summary>
    /// If the system is recording a gesture.
    /// </summary>
    private bool isRecording;
    private IEnumerator leftRecord, rightRecord;

    public void StartRecording(Hand hand = Hand.Both) {
        isRecording = true;
        if (hand == Hand.LeftHand || hand == Hand.Both) {
            leftGR.startStroke(steamCamera.position, steamCamera.rotation);
            leftRecord = Record(leftGR, leftController);
            StartCoroutine(leftRecord);
        }

        if (hand == Hand.RightHand || hand == Hand.Both) {
            rightGR.startStroke(steamCamera.position, steamCamera.rotation);
            rightRecord = Record(rightGR, rightController);
            StartCoroutine(rightRecord);
        }
    }


    public GestureInput StopRecording(Hand hand = Hand.Both) {
        isRecording = false;
        if (hand == Hand.LeftHand) {
            StopCoroutine(leftRecord);
            int result = leftGR.endStroke();
            return (GestureInput)result;
        }

        if (hand == Hand.RightHand) {
            StopCoroutine(rightRecord);
            int result = rightGR.endStroke();
            return (GestureInput)result;
        }

        if (hand == Hand.Both) {
            StopCoroutine(leftRecord);
            StopCoroutine(rightRecord);

            int leftResult = leftGR.endStroke();
            int rightResult = rightGR.endStroke();
            print("Left: " + (GestureInput)leftResult);
            print("Right: " + (GestureInput)rightResult);
            return (GestureInput) (leftResult == rightResult ? leftResult : -1);
        }

        return GestureInput.NotRecognized;
    }

    private IEnumerator Record(GestureRecognition gr, Transform controller) {
        while (isRecording) {
            gr.contdStroke(controller.localPosition, controller.localRotation);
            yield return null;
        }
    }
}
