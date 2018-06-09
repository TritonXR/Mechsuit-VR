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

  public Hud hud;

  /* Properties */
  public float CurrHealth { get; protected set; }
  public int MaxHealth {
    get { return maxHealth; }
  }
  public bool Restorable { get { return restoreable; } }

  void Start() {
    CurrHealth = maxHealth;
  }

  public virtual void TakeDamage(float value, DamageType type) {
    Debug.Log("The health of " + this.gameObject.name + " is: " + CurrHealth);
    CurrHealth = (CurrHealth - value <= 0) ? 0 : CurrHealth - value;
    hud.UpdateHealth(this, gameObject.tag != "Player", gameObject.name);
    if (CurrHealth <= 0) {
      Debug.Log("Destroyed: " + this.gameObject.name);
      Destroy(this.gameObject);
    } else {
      Debug.Log("Remaining health of" + this.gameObject.name + " this object is: " + CurrHealth);
    }


  }

  public void Restore (float value, RestoreType type) {
    if (type == RestoreType.health && restoreable) {
      Debug.Log("The health of " + this.gameObject.name + " is: " + CurrHealth);

      CurrHealth = (CurrHealth + value >= maxHealth) ? maxHealth : CurrHealth + value;

      if (CurrHealth >= maxHealth) {
        Debug.Log("Restored to full health: " + this.gameObject.name);
      } else {
        Debug.Log("Current health of" + this.gameObject.name + " this object is: " + CurrHealth);
      }
    }

    hud.UpdateHealth(this, gameObject.tag != "Player", gameObject.name);
  }
}
