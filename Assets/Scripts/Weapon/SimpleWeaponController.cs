using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWeaponController : MonoBehaviour {
  /* Weapon-related variables */
  public Weapon weapon;

  // TODO: refactor those out of controller because close-range weapons like swords don't need those
  public string[] ammoType;
  public float[] fireDelay;
  protected int currAmmoIndex;
  protected float currDelay;
}
