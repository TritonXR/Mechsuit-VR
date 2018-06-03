using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {

  private void OnCollisionEnter(Collision collision) {
    IHealth health = (IHealth)collision.gameObject.GetComponent(typeof(IHealth));
    if (health != null) {
      IHealthChange restore = (IHealthChange)GetComponent(typeof(IHealthChange));
      restore.ChangeHealth(health);
      Destroy(this.gameObject);
    }
  }
}
