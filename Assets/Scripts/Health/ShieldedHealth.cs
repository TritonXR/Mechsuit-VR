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

  public float currHealth;
  public float currShield;
  public float delayTime;

  public HUD hud;

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


  public virtual void TakeDamage(float value, DamageType type) {
    float damage = DamageShield(value, type);
    if (damage == value) { // Penetrates the shield
      DamageType newType = (DamageType)System.Enum.Parse(typeof(DamageType), type.ToString().Split('_')[1]);
      DamageHealth(damage, newType);
    } else {
      DamageHealth(damage, type);
    }
    hud.UpdateHealth();
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
    delayTime = delay;
    return damageToHealth;
  }

  private void DamageHealth(float value, DamageType type) {
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
    hud.UpdateHealth();
  }

  private float RestoreHealth(float value) {
    Debug.Log("Restore caused to the health of: " + this.gameObject.name);
    float remainShield = currHealth;

    currHealth = (currHealth + value >= maxHealth) ? maxHealth : currHealth + value;

    if (currHealth >= maxHealth) {
      Debug.Log("Restored to full health: " + this.gameObject.name);
      remainShield = value - (currHealth - remainShield);
    } else {
      Debug.Log("Current health of this object is: " + currHealth);
      remainShield = 0;
    }
    return remainShield;
  }

  private float RestoreShield(float value) {
    Debug.Log("Restore caused to the shield of: " + this.gameObject.name);
    float remainHealth = currShield;

    currShield = (currShield + value >= maxShield) ? maxShield : currShield + value;

    if (currShield >= maxShield) {
      Debug.Log("Restored to full shield: " + this.gameObject.name);
      remainHealth = value - (currShield - remainHealth);
    } else {
      Debug.Log("Current health of this object is: " + currHealth);
      remainHealth = 0;
    }
    return remainHealth;
  }
}
