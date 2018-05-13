
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleDamage : MonoBehaviour, IDamage {
  [SerializeField]
  public DamageType type;
  [SerializeField]
  public int value;
}
