using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {

  public override void Setup() {

  }

  public override void Show() {
    base.Show();
  }

  public void OnTriggerEnter(Collider collider) {
  IHealth health = (IHealth)collider.gameObject.GetComponent(typeof(IHealth));
    if (health != null) {
      IHealthChange damage = (IHealthChange)GetComponent(typeof(IHealthChange));
      damage.ChangeHealth(health);
    }
  }
}