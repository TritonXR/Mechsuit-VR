using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
    public string name;
    public CalibrateManager manager;

    public abstract void Setup();

    public virtual void Show() {
        //enable the model
        gameObject.SetActive(true);
    }

    /* After switching weapons, hide it */
    public virtual void Hide() {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates the selected weapon. For ranged weapons like guns that mean firing a weapon; for close-range weapons like swords that can be made blank.
    /// </summary>
    /// <param name="tag">Tag for the ammo if the weapon is a ranged weapon</param>
    public virtual bool Activate(string tag) { return false; }

    /// <summary>
    /// Reactivates the selected weapon. For ranged weapons like guns that mean reloading; for close-range weapons like swords that can be made blank.
    /// </summary>
    /// <param name="tag">Tag for the ammo if the weapon is a ranged weapon</param>
    public virtual bool ReActivate(string tag) { return false; }
}