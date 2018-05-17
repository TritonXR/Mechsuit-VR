using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType {
  physical,
  health_physical
};

public enum RestoreType {
  health,
  shield,
  healthShield,
  shieldHealth
};

public interface IHealthChange {
  void ChangeHealth(IHealth health);
}
