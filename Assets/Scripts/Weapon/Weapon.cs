using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

	public GameObject modelWeapon;
	public WeaponController controller;
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

	/* Gun/Rocket launcher firing */
	public virtual void Fire (string ammoTag) {}

  /* Gun/Rocket launcher reloading */
  public virtual void Reload(string ammoTag) {}
}