using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedHealth : MonoBehaviour, IHealth {
  /* Shield */
  public int maxShield;
  public float delay;
  public float chargePerSecond;

  /* Health */
  public int maxHealth;
  public bool restoreable; // If the health can be restored by a potion

  public float CurrHealth { get; protected set; }
  public float CurrShield { get; protected set; }
  public bool Restorable { get { return restoreable; } }
  private float delayTime;

  public int MaxHealth {
    get { return maxHealth; }
  }

  public Hud hud;

  void Start() {
    CurrHealth = maxHealth;
    CurrShield = maxShield;
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


  public virtual void TakeDamage(float value, DamageType type) {
    if (type.ToString().StartsWith("health")) {
      DamageType newType = (DamageType)System.Enum.Parse(typeof(DamageType), type.ToString().Split('_')[1]);
      DamageHealth(value, newType);
    } else {
      float damage = DamageShield(value, type);
      DamageHealth(damage, type);
    }

    hud.UpdateHealth(this, gameObject.tag != "Player", gameObject.name);
    if (CurrHealth <= 0) {
      //Debug.Log("Destroyed: " + this.gameObject.name);
      Destroy(this.gameObject);
    } else {
      //Debug.Log("Remaining health of this object is: " + CurrHealth);
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
   // Debug.Log("Damage caused to the shield of: " + this.gameObject.name);
    //Debug.Log("The shield of this object is: " + CurrShield);
    float damageToHealth = CurrShield;
    CurrShield = (CurrShield - value <= 0) ? 0 : CurrShield - value;
    if (CurrShield <= 0) {
      //Debug.Log("Shield broken for:" + this.gameObject.name);
      damageToHealth = value - damageToHealth;
    } else {
      //Debug.Log("Remaining shield of this object is: " + CurrShield);
      damageToHealth = 0;
    }
    delayTime = delay;
    return damageToHealth;
  }

  private void DamageHealth(float value, DamageType type) {
    //Debug.Log("Damage caused to: " + this.gameObject.name);
    //Debug.Log("The health of this object is: " + CurrHealth);
    CurrHealth = (CurrHealth - value <= 0) ? 0 : CurrHealth - value;
  }

  public void Restore(float value, RestoreType type) {
    switch (type) {
      case RestoreType.health:
        RestoreHealth(value);
        break;
      case RestoreType.shield:
        RestoreShield(value);
        break;
      case RestoreType.healthShield:
        float remainShield = RestoreHealth(value);
        RestoreShield(remainShield);
        break;
      case RestoreType.shieldHealth:
        float remainHealth = RestoreShield(value);
        RestoreHealth(remainHealth);
        break;
    }
    hud.UpdateHealth(this, gameObject.tag != "Player", gameObject.name);
  }

  private float RestoreHealth(float value) {
    //Debug.Log("Restore caused to the health of: " + this.gameObject.name);
    float remainShield = CurrHealth;

    CurrHealth = (CurrHealth + value >= maxHealth) ? maxHealth : CurrHealth + value;

    if (CurrHealth >= maxHealth) {
      //Debug.Log("Restored to full health: " + this.gameObject.name);
      remainShield = value - (CurrHealth - remainShield);
    } else {
      //Debug.Log("Current health of this object is: " + CurrHealth);
      remainShield = 0;
    }
    return remainShield;
  }

  private float RestoreShield(float value) {
    //Debug.Log("Restore caused to the shield of: " + this.gameObject.name);
    float remainHealth = CurrShield;

    CurrShield = (CurrShield + value >= maxShield) ? maxShield : CurrShield + value;

    if (CurrShield >= maxShield) {
      //Debug.Log("Restored to full shield: " + this.gameObject.name);
      remainHealth = value - (CurrShield - remainHealth);
    } else {
      //Debug.Log("Current health of this object is: " + CurrHealth);
      remainHealth = 0;
    }
    return remainHealth;
  }
}
