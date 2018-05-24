using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType {
  none,
  health_none,
  physical,
  health_physical
};

public enum RestoreType {
  health,
  shield,
  healthShield,
  shieldHealth
};

/// <summary>
/// Interface for scripts that deals a change in health. This means either reducing health or restoring health.
/// </summary>
public interface IHealthChange {
  /// <summary>
  /// Reduces or restores health.
  /// </summary>
  /// <param name="health">Health script to be changed</param>
  void ChangeHealth(IHealth health);
}
