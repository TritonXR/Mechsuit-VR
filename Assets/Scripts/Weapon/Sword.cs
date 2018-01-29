using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {

  public override void Show() {
    base.Show();

    //DO SWORD CODE FOR SHOW

  }

  public override bool DetectMovements() {
    return true;
  }

}