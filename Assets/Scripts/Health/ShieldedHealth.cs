using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedHealth : MonoBehaviour, IHealth {
  public int maxShield;
  public int[] shieldResistences;

  public int maxHealth;
  public int[] healthResistences;
  public bool restoreable; // If the health can be restored by a potion

  private float currHealth;
  private float currShield;

  void Start() {
    currHealth = maxHealth;
  }

  public void TakeDamage(float value, DamageType type) {
  }

  public void Restore(float value, RestoreType type) {

  }
}
