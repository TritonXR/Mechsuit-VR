using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth {

  void TakeDamage(IDamage damage);
  void Restore(int value, bool once);
}
