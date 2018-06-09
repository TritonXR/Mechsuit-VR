using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {

  private void OnTriggerEnter(Collider collider) {
    IHealth health = (IHealth)collider.gameObject.GetComponent(typeof(IHealth));
    if (health != null && health.Restorable) {
      IHealthChange restore = (IHealthChange)GetComponent(typeof(IHealthChange));
      if (restore != null) {
        Debug.Log("Potion restore triggered!");
        restore.ChangeHealth(health);
        Destroy(this.gameObject);
      }
    }
  }
}
