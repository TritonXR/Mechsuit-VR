using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRestore : MonoBehaviour, IHealthChange {
  [SerializeField]
  public RestoreType type;
  [SerializeField]
  public int value;

  public void ChangeHealth(IHealth health) {
    health.Restore(value, type);
  }
}
