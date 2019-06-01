using System.Collections;
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
    if (target != null) {
      //Debug.Log("WeaponController.Update()");
      //if (currDelay <= 0.0f && TriggerClicked()) {
      if (currDelay > 0.0f) {
        currDelay -= Time.deltaTime;
      } else {
        if (!FireWeapon()) {
          ReloadWeapon();
        }
      }

      this.transform.LookAt(target.transform);
    }
  }

  bool ReloadWeapon() {
    //Debug.Log("AI attempting to reload.");
    return currentWeapon.ReActivate(ammoType[currAmmoIndex]);
  }

  bool FireWeapon() {
    if (currDelay <= 0.0f) {
      //Debug.Log("AI attempting to fire.");
      currDelay = fireDelay[currAmmoIndex];
      return currentWeapon.Activate(ammoType[currAmmoIndex]);
    } else {
      return false;
    }
  }
}