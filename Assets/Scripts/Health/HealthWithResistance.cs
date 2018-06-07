using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Refer to:
/// https://unity3d.com/learn/tutorials/topics/multiplayer-networking/player-health-single-player
/// </summary>
public class HealthWithResistance : SimpleHealth {
  [SerializeField]
  public List<int> healthResistances;

  public override void TakeDamage(float value, DamageType type) {
    Debug.Log("Damage caused to: " + this.gameObject.name);
    Debug.Log("The health of this object is: " + CurrHealth);
    float percentageReduced = 1 - ((float)healthResistances[(int)type]) / 100;
    float actualDamage = value * percentageReduced;
    CurrHealth = (CurrHealth - actualDamage <= 0) ? 0 : CurrHealth - actualDamage;

    if (CurrHealth <= 0) {
      Debug.Log("Destroyed: " + this.gameObject.name);
      Destroy(this.gameObject);
    } else {
      Debug.Log("Remaining health of this object is: " + CurrHealth);
    }
  }
}
