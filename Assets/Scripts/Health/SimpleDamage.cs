﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleDamage : MonoBehaviour, IHealthChange {
  [SerializeField]
  public DamageType type;
  [SerializeField]
  public int value;

  public void ChangeHealth(IHealth health) {
    health.TakeDamage(value, type);
  }
}