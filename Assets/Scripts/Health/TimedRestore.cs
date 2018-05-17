
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimedRestore : MonoBehaviour, IHealthChange, ITimedAction {
  [SerializeField]
  public float restorePerSecond;

  public void ChangeHealth(IHealth health) {

  }

  public void PerformAction() {
    throw new System.NotImplementedException();
  }
}
