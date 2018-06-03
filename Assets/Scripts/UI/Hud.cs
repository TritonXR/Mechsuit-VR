using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

  public GameObject shieldFill, healthFill;
  public ShieldedHealth core;

  public void UpdateHealth() {
    shieldFill.transform.localPosition = new Vector3(-100 + 100 * (core.currShield / core.maxShield), shieldFill.transform.localPosition.y, shieldFill.transform.localPosition.z);
    healthFill.transform.localPosition = new Vector3(-100 + 100 * (core.currHealth / core.maxHealth), healthFill.transform.localPosition.y, healthFill.transform.localPosition.z);
  }
}
