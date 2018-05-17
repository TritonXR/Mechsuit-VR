
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimedDamage : MonoBehaviour, IHealthChange, ITimedAction {
  [SerializeField]
  public float damagePerSecond;
  [SerializeField]
  public float staticDamage;
  [SerializeField]
  [Range(0, 100)]
  // chance to be casted this DOT.
  public int castProbability;

  public void ChangeHealth(IHealth health) {

  }
  
  public void PerformAction() {
    throw new System.NotImplementedException();
  }
}
