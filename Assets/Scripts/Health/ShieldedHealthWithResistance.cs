using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedHealthWithResistance :  ShieldedHealth {
  /* Shield */
  [SerializeField]
  public List<int> shieldResistances;

  /* Health */
  [SerializeField]
  // Should shield bring extra resistances;
  public List<int> healthResistances;

  public override void TakeDamage(float value, DamageType type) {
    float damage = DamageShield(value, type);
    if (damage == value) { // Penetrates the shield
      DamageType newType = (DamageType)System.Enum.Parse(typeof(DamageType), type.ToString().Split('_')[1]);
      DamageHealth(damage, newType);
    } else {
      DamageHealth(damage, type);
    }
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="value"></param>
  /// <param name="type"></param>
  /// <returns>The remaining value to be damamged to health</returns>
  private float DamageShield(float value, DamageType type) {
    // TODO: do shield damage calculating
    if (type.ToString().StartsWith("health")) {
      return value;
    }
    // Not penetrating the shield
    Debug.Log("Damage caused to the shield of: " + this.gameObject.name);
    Debug.Log("The shield of this object is: " + CurrShield);
    float damageToHealth = CurrShield;
    CurrShield = (CurrShield - value <= 0) ? 0 : CurrShield - value;
    if (CurrShield <= 0) {
      Debug.Log("Shield broken for:" + this.gameObject.name);
      damageToHealth = value - damageToHealth;
    } else {
      Debug.Log("Remaining shield of this object is: " + CurrShield);
      damageToHealth = 0;
    }

    return damageToHealth;
  }

  private void DamageHealth(float value, DamageType type) {
    Debug.Log("Damage caused to the health of: " + this.gameObject.name);
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
