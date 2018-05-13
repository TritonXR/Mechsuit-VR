using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Refer to:
/// https://unity3d.com/learn/tutorials/topics/multiplayer-networking/player-health-single-player
/// </summary>
public class SimpleHealth : MonoBehaviour, IHealth {
  [SerializeField]
  public int maxHealth;
  public int[] healthResistences;

  private int currHealth;
	
  void Start() {
    currHealth = maxHealth;
  }

  public void TakeDamage(IDamage damage) {
    Debug.Log("Damage caused to: " + this.gameObject.name);
    Debug.Log("The health of this object is: " + currHealth);
    if (damage is SimpleDamage) {
      DamageType type = ((SimpleDamage)damage).type;

      float percentageReduced = 1 - ((float)healthResistences[(int)type]) / 100;
      int actualDamage = (int)(((SimpleDamage)damage).value * percentageReduced);
      currHealth = (currHealth - actualDamage < 0) ? 0 : currHealth - actualDamage;

      if (currHealth == 0) {
        Debug.Log("Destroyed: " + this.gameObject.name);
        Destroy(this.gameObject);
      } else {
        Debug.Log("Remaining health of this object is: " + currHealth);
      }
    }
  }

  public void Restore (int value, bool once) {
    if (once) {
      currHealth = (currHealth + value > maxHealth) ? maxHealth : currHealth + value;
    } else {
      // Use Coroutine to add health
    }
  }
}
