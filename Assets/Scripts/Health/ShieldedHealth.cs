using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedHealth : MonoBehaviour, IHealth {
  [SerializeField]
  public int MaxShield { get; set; }
  public int[] shieldResistences;

  public void TakeDamage(IDamage damage) {

  }

  public void Restore(int value, bool once) {

  }
}
