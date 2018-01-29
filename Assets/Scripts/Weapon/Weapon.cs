using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

	public string key;
	public GameObject model_weapon;
	public WeaponController controller;
	public GameObject hand;

	public Collider weaponCollider;
	// Collider that monitors the summoning action

	public virtual void Setup () {
		//Setup

	}

	public virtual void Show () {
		//enable the model
		model_weapon.gameObject.SetActive(true);
		//model_weapon.transform.SetParent(controller.debug_tracked_object.transform);
		model_weapon.transform.SetParent(hand.transform);
		//model_weapon.transform.localPosition = Vector3.zero;
	}

	/* After switching weapons, hide it */
  public virtual void Hide () {
    model_weapon.gameObject.SetActive(false);
    model_weapon.transform.SetParent(null);
	}

	/* Gun/Rocket launcher firing */
	public virtual void Fire () {

	}

	/* Updates after each fire */
	public virtual void UpdateStatus () {

	}

	public void Update () {
		//Debug.Log("t");
		//if (controller && controller.debug_tracked_object)
		//model_weapon.transform.position = hand.transform.position;
	}

	/* Detects the movents that correspond to the summoning action */
	public abstract bool DetectMovements ();
}