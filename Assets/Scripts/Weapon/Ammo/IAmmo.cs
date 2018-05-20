using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAmmo {

  GameObject Weapon { get; set; }
  void OnObjectSpawn();
}
