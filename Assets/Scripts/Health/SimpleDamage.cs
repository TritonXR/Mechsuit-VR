
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleDamage : MonoBehaviour, IHealthChange {
  public DamageType type;
  public int value;

  public void ChangeHealth(IHealth health) {
    health.TakeDamage(value, type);
  }
}
