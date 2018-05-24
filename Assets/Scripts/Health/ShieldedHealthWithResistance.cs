using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedHealthWithResistance :  ShieldedHealth {
  /* Shield */
  public int maxShield;
  [SerializeField]
  public List<int> shieldResistances;
  public float delay;
  public float chargePerSecond;

  /* Health */
  public int maxHealth;
  [SerializeField]
  // Should shield bring extra resistances;
  public List<int> healthResistances;
  public bool restoreable; // If the health can be restored by a potion

  private float currHealth;
  private float currShield;
  private float delayTime;

  void Start() {
    currHealth = maxHealth;
    currShield = maxShield;
  }

  /// <summary>
  /// Handles the auto shield regen
  /// </summary>
  void Update() {
    if (delayTime >= 0) {
      delayTime -= Time.deltaTime;
    } else {
      Restore(chargePerSecond * Time.deltaTime, RestoreType.shield);
    }
  }


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
    Debug.Log("The shield of this object is: " + currShield);
    float damageToHealth = currShield;
    currShield = (currShield - value <= 0) ? 0 : currShield - value;
    if (currShield <= 0) {
      Debug.Log("Shield broken for:" + this.gameObject.name);
      damageToHealth = value - damageToHealth;
    } else {
      Debug.Log("Remaining shield of this object is: " + currShield);
      damageToHealth = 0;
    }

    return damageToHealth;
  }

  private void DamageHealth(float value, DamageType type) {
    Debug.Log("Damage caused to the health of: " + this.gameObject.name);
    Debug.Log("The health of this object is: " + currHealth);
    float percentageReduced = 1 - ((float)healthResistances[(int)type]) / 100;
    float actualDamage = value * percentageReduced;
    currHealth = (currHealth - actualDamage <= 0) ? 0 : currHealth - actualDamage;

    if (currHealth <= 0) {
      Debug.Log("Destroyed: " + this.gameObject.name);
      Destroy(this.gameObject);
    } else {
      Debug.Log("Remaining health of this object is: " + currHealth);
    }
  }
}
