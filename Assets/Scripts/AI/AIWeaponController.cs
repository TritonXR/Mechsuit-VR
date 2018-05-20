﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponController : SimpleWeaponController {

  public GameObject target;

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
    //Debug.Log("WeaponController.Update()");
    //if (currDelay <= 0.0f && TriggerClicked()) {
    if (currDelay > 0.0f) {
      currDelay -= Time.deltaTime;
    }

    else {
      if (!FireWeapon() && !ReloadWeapon()) {
        Debug.Log("You have not calibrated, or the ammo type is incorrect!");
      }
    }

    this.transform.LookAt(target.transform);
  }

  bool ReloadWeapon() {
    Debug.Log("AI attempting to reload.");
    return weapon.ReActivate(ammoType[currAmmoIndex]);
  }

  bool FireWeapon() {
    if (currDelay <= 0.0f) {
      Debug.Log("AI attempting to fire.");
      currDelay = fireDelay[currAmmoIndex];
      return weapon.Activate(ammoType[currAmmoIndex]);
    } else {
      return false;
    }
  }
}