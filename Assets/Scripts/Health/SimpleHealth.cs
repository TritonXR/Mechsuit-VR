using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Refer to:
/// https://unity3d.com/learn/tutorials/topics/multiplayer-networking/player-health-single-player
/// </summary>
public class SimpleHealth : MonoBehaviour, IHealth {
  public int maxHealth;
  public bool restoreable; // If the health can be restored by a potion

  protected float currHealth;

  void Start() {
    currHealth = maxHealth;
  }

  public virtual void TakeDamage(float value, DamageType type) {
    Debug.Log("Damage caused to: " + this.gameObject.name);
    Debug.Log("The health of this object is: " + currHealth);
    currHealth = (currHealth - value <= 0) ? 0 : currHealth - value;

    if (currHealth <= 0) {
      Debug.Log("Destroyed: " + this.gameObject.name);
      Destroy(this.gameObject);
    } else {
      Debug.Log("Remaining health of this object is: " + currHealth);
    }
  }

  public void Restore (float value, RestoreType type) {
    if (type == RestoreType.health && restoreable) {
      Debug.Log("Restore caused to: " + this.gameObject.name);
      Debug.Log("The health of this object is: " + currHealth);

      currHealth = (currHealth + value >= maxHealth) ? maxHealth : currHealth + value;

      if (currHealth >= maxHealth) {
        Debug.Log("Restored to full health: " + this.gameObject.name);
      } else {
        Debug.Log("Current health of this object is: " + currHealth);
      }
    }
  }
}
