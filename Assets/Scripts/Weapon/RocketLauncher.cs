using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon {

	public override void Show ()
	{
		base.Show ();

		//ROCKET LAUNCHER STUFF
	}

	public void rocketLauncherViewFinderCode()
	{

	}

  public override bool DetectMovements() {
    return false;
  }

}
