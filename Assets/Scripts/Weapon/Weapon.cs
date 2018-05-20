using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

	public GameObject modelWeapon;
	public SimpleWeaponController controller;
	public GameObject hand;

  // Collider that monitors the summoning action
  public Collider weaponCollider;

  public abstract void Setup();

	public virtual void Show () {
		//enable the model
		modelWeapon.gameObject.SetActive(true);
		//model_weapon.transform.SetParent(controller.debug_tracked_object.transform);
		modelWeapon.transform.SetParent(hand.transform);
		//model_weapon.transform.localPosition = Vector3.zero;
	}

	/* After switching weapons, hide it */
  public virtual void Hide () {
    modelWeapon.gameObject.SetActive(false);
    modelWeapon.transform.SetParent(null);
	}

	/// <summary>
  /// Activates the selected weapon. For ranged weapons like guns that mean firing a weapon; for close-range weapons like swords that can be made blank.
  /// </summary>
  /// <param name="tag">Tag for the ammo if the weapon is a ranged weapon</param>
	public virtual bool Activate (string tag) { return false; }

  /// <summary>
  /// Reactivates the selected weapon. For ranged weapons like guns that mean reloading; for close-range weapons like swords that can be made blank.
  /// </summary>
  /// <param name="tag">Tag for the ammo if the weapon is a ranged weapon</param>
  public virtual bool ReActivate(string tag) { return false; }
}