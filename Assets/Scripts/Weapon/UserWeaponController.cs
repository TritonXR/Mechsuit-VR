﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserWeaponController : SimpleWeaponController {
  /* SteamVR controller */
  private SteamVR_TrackedController controller;


  /* Methods */
  /// <summary>
  /// Sets up the controller and relevant info.
  /// </summary>
  private void Awake() {
    //Gizmos.color = Color.green;
    currAmmoIndex = 0;
    currDelay = 0.0f;


    controller = GetComponent<SteamVR_TrackedController>();
    controller.TriggerClicked += FireWeapon;
    controller.Gripped += ReloadWeapon;
  }

  private void Update() {
    //Debug.Log("WeaponController.Update()");
    //if (currDelay <= 0.0f && TriggerClicked()) {
    if (currDelay > 0.0f) {
      currDelay -= Time.deltaTime;
    }
  }


  void ReloadWeapon(object sender, ClickedEventArgs e) {
    Debug.Log("Grip clicked, attempting to reload");
    weapon.ReActivate(ammoType[currAmmoIndex]);
  }

  void FireWeapon(object sender, ClickedEventArgs e) {
    if (currDelay <= 0.0f) {
      weapon.Activate(ammoType[currAmmoIndex]);
      currDelay = fireDelay[currAmmoIndex];
    }
  }

  #region Ugly Crap
  /*
   * private void OnDrawGizmos() {
    //Debug.Log("OnDrawGizmos()");
    Gizmos.DrawRay(gunBarrel.transform.position, 5*gunBarrel.transform.forward);
    Gizmos.DrawRay(gunBarrel.transform.position, 5*gunBarrel.transform.right);
    Gizmos.DrawRay(gunBarrel.transform.position, 5*gunBarrel.transform.up);
  }
   * public List<Weapon> weapons;
  public bool b_setup;

  public Text text_debug;
  public Weapon weapon_active;
  // current weapon

  public SteamVR_TrackedObject debug_tracked_object;

  // Use this for initialization
  public void Setup () {
    if (b_setup) { // already set up
      return;
    }

    b_setup = true;

    //setup weapons
    foreach (Weapon w in weapons) {
      w.Setup();
    }
  }

  // DEBUG TEST WEAPONS, SUPPORTS 7
  void Update () {

    //	In final build, add null checking for controller:
    if (controller == null) {
    	Debug.Log("Controller not initialized.");
    	return;
    }

		if (Input.GetKeyDown (KeyCode.Alpha1) || controller.GetPressDown (gripButton) && swordTrigger == true)
			setWeapon (weapons [0]);
		else if(Input.GetKeyDown (KeyCode.Alpha2) && weapons.Count > 0)
			setWeapon(weapons[1]);
		else if(Input.GetKeyDown (KeyCode.Alpha3) && weapons.Count > 1)
			setWeapon(weapons[2]);
		else if(Input.GetKeyDown (KeyCode.Alpha4) && weapons.Count > 2)
			setWeapon(weapons[3]);
		else if(Input.GetKeyDown (KeyCode.Alpha5) && weapons.Count > 3)
			setWeapon(weapons[4]);
		else if(Input.GetKeyDown (KeyCode.Alpha6) && weapons.Count > 4)
			setWeapon(weapons[5]);
		else if(Input.GetKeyDown (KeyCode.Alpha7) && weapons.Count > 5)
			setWeapon(weapons[6]);
    
  }

  //TODO, implement dictionary for weapon lookup
  public Weapon FindWeapon (string key) {
    foreach (Weapon w in weapons)
      if (w.key.Equals(key)) {
        return w;
      }

    return null;
  }

  public void SetWeapon (Weapon w) {
    //hide the current weapon
    if (weapon_active == w) {
      return; // do nothing
    } else {
      if (weapon_active != null) { // in case of first weapon
        weapon_active.Hide();
      }
      //Set as active weapon
      weapon_active = w;
      weapon_active.controller = this;
      weapon_active.Show();
      //set debug text...
      text_debug.text = "LOADED " + w.key;
    }
  }

  public void OnTriggerEnter (Collider other) {
    //if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
    // Traverses the weapon list and check whether the movement corresponds to the weapon
    foreach (Weapon w in weapons) {
      if (other.Equals(w.weaponCollider)) {
        if (w.DetectMovements()) {
          SetWeapon(w);
          break;
        }
      }
    }
    //}
  }*/
  #endregion

}