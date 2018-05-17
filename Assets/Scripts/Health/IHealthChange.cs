using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType {
  physical
};

public enum RestoreType {
  health,
  shield
};

public interface IHealthChange {
  void ChangeHealth(IHealth health);
}
