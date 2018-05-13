
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimedDamage : MonoBehaviour, IDamage {
  [SerializeField]
  public float damagePerSecond { get; set; }
  [SerializeField]
  [Range(0, 100)]
  // chance to be casted this DOT.
  public int castProbability;
}
