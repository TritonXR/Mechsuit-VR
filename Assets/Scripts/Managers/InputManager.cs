using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Valve.VR;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }


    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public bool GetButtonInput(ButtonInput input, Hand hand = Hand.Any) {
        PropertyInfo[] properties = typeof(SteamVR_Input_ActionSet_default).GetProperties();
        foreach (PropertyInfo property in properties) {
            if (property.Name == input.ToString()) {
                return ((SteamVR_Action_Boolean)property.GetValue(SteamVR_Actions._default)).GetStateDown(GetInputSource(hand));
            }
        }

        return false;
    }

    private SteamVR_Input_Sources GetInputSource(Hand hand) {
        return (SteamVR_Input_Sources)System.Enum.Parse(typeof(Hand), hand.ToString(), true);
    }

    private void Update() {
        if (GetButtonInput(ButtonInput.PauseGame, Hand.Any)) {
            print("Paused");
        }
    }

    public void StartRecording() {

    }

    public GestureInput StopRecording() {
        throw new System.NotImplementedException();
    }
}
