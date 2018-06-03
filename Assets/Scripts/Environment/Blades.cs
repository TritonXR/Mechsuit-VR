using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blades : MonoBehaviour {
  public void OnCollisionEnter(Collision collision) {
    IHealth health = (IHealth)collision.gameObject.GetComponent(typeof(IHealth));
    if (health != null) {
      IHealthChange damage = (IHealthChange)GetComponent(typeof(IHealthChange));
      damage.ChangeHealth(health);
    }
  }
}
