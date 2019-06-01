using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class UserWeaponController : SimpleWeaponController {
    public Hud hud;

    public List<Weapon> weapons;

    private readonly Dictionary<GestureInput, string> inputNameDict = new Dictionary<GestureInput, string> {
        { GestureInput.SummonRailgun, "railgun" },
        { GestureInput.SummonSword, "sword" }
    };

    /* Methods */
    /// <summary>
    /// Sets up the controller and relevant info.
    /// </summary>
    private void Awake() {
        //Gizmos.color = Color.green;
        currAmmoIndex = 0;
        currDelay = 0.0f;
    }

    private void Update() {
        if (currDelay > 0.0f) {
            currDelay -= Time.deltaTime;
        }

        if (InputManager.Instance.GetButtonInput(ButtonInput.FireWeapon, Hand.RightHand)) {
            FireWeapon();
        }

        if (InputManager.Instance.GetButtonInput(ButtonInput.ReloadWeapon, Hand.RightHand)) {
            ReloadWeapon();
        }

        if (InputManager.Instance.GetButtonInput(ButtonInput.SummonWeapon, Hand.RightHand)) {
            InputManager.Instance.StartRecording();
        }

        if (InputManager.Instance.GetButtonInputUp(ButtonInput.SummonWeapon, Hand.RightHand)) {
            GestureInput gesture = InputManager.Instance.StopRecording();
            print("The gesture is: " + gesture);
            Weapon weapon = (from w in weapons where w.name == inputNameDict[gesture] select w).ToArray()[0];
            print(weapon.name);
            SwitchWeapon(weapon);
        }
    }


    void ReloadWeapon() {
        Debug.Log("Grip clicked, attempting to reload");
        currentWeapon.ReActivate(ammoType[currAmmoIndex]);
        hud.UpdateAmmo();
    }

    void FireWeapon() {
        if (currDelay <= 0.0f) {
            currentWeapon.Activate(ammoType[currAmmoIndex]);
            hud.UpdateAmmo();
            currDelay = fireDelay[currAmmoIndex];
        }
    }

    void RecycleWeapon() {

    }

    void SwitchWeapon(Weapon weapon) {
        currentWeapon.Hide();
        currentWeapon = weapon;
        currentWeapon.Setup();
        currentWeapon.Show();
    }
}