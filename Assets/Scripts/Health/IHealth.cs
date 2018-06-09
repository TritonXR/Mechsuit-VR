using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth {
  float CurrHealth { get; }
  int MaxHealth { get; }
  bool Restorable { get; }
  void TakeDamage(float value, DamageType type);
  void Restore(float value, RestoreType type);
}
