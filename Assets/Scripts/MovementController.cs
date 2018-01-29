﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

  public Vector3 pointEnter;

  public Camera cameraPlayer;

  /// <summary>
  /// This is the mechsuit body, or the camera rig itself.
  /// </summary>
  public Rigidbody suitBody;

  /*public float maxPulseForce;
  public float maxPulseDistance;

  public Vector3 enterDisplacement;

  public float dis;

  public Transform MoveColliderRight;
  */
  public float maxPulseMagnitude;

  public float pulseStrength;

  //reference to the tracked-object component on the controller/HMD GameObject
  public SteamVR_TrackedObject trackedObj;

  /// <summary>
  /// The controller.
  /// </summary>
  /// <value>The device.</value>
  public SteamVR_Controller.Device Device {
    get { return SteamVR_Controller.Input ((int)trackedObj.index); }
  }

  /* A sphere used to debug the force of movement. */
  public GameObject sphere;

  /* cooldown */
  private readonly float cooldownTime = 1.0f;
  float timeLeft;
  /*bool Cooldown {
    get { return timeLeft > 0.0f; }
  }*/
  bool cooldown;

  /*** Methods start here ***/

  /// <summary>
  /// Start this instance.
  /// </summary>
  void Start() {
    //get the device associated with that tracked object (which is how you access buttons and stuff)
    Debug.Log("DEVICE: " + Device);
  }

  /// <summary>
  /// Raises the trigger enter event.
  /// </summary>
  /// <param name="other">Other.</param>
  void OnTriggerEnter(Collider other) {
    Debug.Log ("Enters collider " + other.tag);
    if(other.tag == "ForwardCollider") {
      Debug.Log ("Enters CORRECT collider");
      if (!cooldown) {
        //NEVER EXECUTED PROPERLY
        //timeLeft = cooldownTime;
        //Debug.Log ("Boost activated! timeLeft is " + timeLeft);
        ForwardPulse ();
      }
    }
  }

  void Update() {
    //Debug.Log ("Update(): timeLeft is " + timeLeft);
    /*if (cooldown) {
      Debug.Log ("Cooling down, timeLeft is " + timeLeft);
      timeLeft -= Time.deltaTime;
    }*/
  }


  /// <summary>
  /// Forwards the pulse.
  /// </summary>
  public void ForwardPulse() {
    cooldown = true;
    suitBody.velocity = Vector3.zero;
    suitBody.AddForce (GetForwardDirection () * 125000);
    Invoke ("EndPulse", cooldownTime);
    //calc force
    //float m = velocity.magnitude / maxPulseMagnitude;

    //Debug.Log(m);

    // set the scale of the sphere
    //sphere.transform.localScale = m * Vector3.one;

    // apply force to the mechsuit
    //suitBody.AddForce (GetForwardDirection () * m * pulseStrength);
  }

  public void EndPulse() {
    cooldown = false;
  }

  /// <summary>
  /// Gets the forward direction.
  /// </summary>
  /// <returns>The forward direction.</returns>
  public Vector3 GetForwardDirection() {
    return cameraPlayer.transform.forward;
  }
}
