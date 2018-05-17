using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth {

  void TakeDamage(float value, DamageType type);
  void Restore(float value, RestoreType type);
}
