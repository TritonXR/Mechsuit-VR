using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: delegate Damage and Restore to do the coroutine.
public interface ITimedAction {
  void PerformAction();
}
