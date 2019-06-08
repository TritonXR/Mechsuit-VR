using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserWeaponController : SimpleWeaponController {
    public Hud hud;

    public List<Weapon> weapons;

    public float clickInterval = 0.1f;

    private readonly Dictionary<GestureInput, string> inputNameDict = new Dictionary<GestureInput, string> {
        { GestureInput.SummonRailgun, "railgun" },
        { GestureInput.SummonSword, "sword" },
        { GestureInput.SummonWhip, "whip" }
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

        if (InputManager.Instance.GetButtonInput(ButtonInput.DequipWeapon, Hand.RightHand)) {
            StartCoroutine(WaitForOtherHand(ButtonInput.DequipWeapon, Hand.LeftHand, DequipWeapon));
        }

        if (InputManager.Instance.GetButtonInput(ButtonInput.DequipWeapon, Hand.LeftHand)) {
            StartCoroutine(WaitForOtherHand(ButtonInput.DequipWeapon, Hand.RightHand, DequipWeapon));
        }

        if (InputManager.Instance.GetButtonInput(ButtonInput.SummonWeapon, Hand.RightHand)) {
            InputManager.Instance.StartRecording(Hand.RightHand);
        }

        if (InputManager.Instance.GetButtonInputUp(ButtonInput.SummonWeapon, Hand.RightHand)) {
            GestureInput gesture = InputManager.Instance.StopRecording(Hand.RightHand);
            print("The gesture is: " + gesture);
            Weapon weapon = (from w in weapons where w.name == inputNameDict[gesture] select w).ToArray()[0];
            print(weapon.name);
            SwitchWeapon(weapon);
        }
    }


    private void ReloadWeapon() {
        if (equipped) {
            Debug.Log("Grip clicked, attempting to reload");
            currentWeapon.ReActivate(ammoType[currAmmoIndex]);
            hud.UpdateAmmo();
        }
    }

    private void FireWeapon() {
        if (currDelay <= 0.0f && equipped) {
            currentWeapon.Activate(ammoType[currAmmoIndex]);
            hud.UpdateAmmo();
            currDelay = fireDelay[currAmmoIndex];
        }
    }


    private IEnumerator WaitForOtherHand(ButtonInput input, Hand hand, System.Action callback) {
        float timeElapsed = 0f;
        while (timeElapsed < clickInterval) {
            if (InputManager.Instance.GetButtonInput(input, hand)) {
                callback();
                break;
            } else {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void DequipWeapon() {
        currentWeapon.Hide();
        equipped = false;
        // TODO: if we have a hand for each weapon, we need to display a default "empty" hand here.
    }

    private void SwitchWeapon(Weapon weapon) {
        currentWeapon.Hide();
        currentWeapon = weapon;
        currentWeapon.Setup();
        currentWeapon.Show();
        equipped = true;
    }
}