using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBackup : MonoBehaviour {

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

  /// <summary>
  /// if we can boost move
  /// </summary>
  bool CanBoost {
    get {
      Debug.Log ("Velocity: " + suitBody.velocity.magnitude);
      return suitBody.velocity.magnitude <= 0.0001f;
    }
  }

  /* Cooldown */
  float timeStamp;
  public float coolDownTime;
  private bool coolDown;

  /*** Methods start here ***/

  /// <summary>
  /// Start this instance.
  /// </summary>
  void Start() {
    //get the device associated with that tracked object (which is how you access buttons and stuff)
    Debug.Log("DEVICE: " + Device);

    coolDown = false;
  }

  /// <summary>
  /// Raises the trigger enter event.
  /// </summary>
  /// <param name="other">Other.</param>
  void OnTriggerEnter(Collider other) {
    if(other.tag == "ForwardCollider") {
      Debug.Log ("Enters collider");
      Vector3 velocity = Device.velocity;
      // TODO DOES THIS WORK?
      //
      //StartPulse();
      if (CanBoost) {
        timeStamp = Time.time;
        ForwardPulse (velocity);
      } else {

        ForwardPulse(velocity);
      }
    }
  }

  void OnTriggerExit (Collider other) {

  }


  /// <summary>
  /// Forwards the pulse.
  /// </summary>
  public void ForwardPulse(Vector3 velocity) {
    suitBody.AddForce (GetForwardDirection () * 100000);
    //calc force
    //float m = velocity.magnitude / maxPulseMagnitude;

    //Debug.Log(m);

    // set the scale of the sphere
    //sphere.transform.localScale = m * Vector3.one;

    // apply force to the mechsuit
    //suitBody.AddForce (GetForwardDirection () * m * pulseStrength);

    // TODO CREATE VARIABLE FOR DELAY
    //waitSeconds (0.1f);
    //EndPulse();
  }

  /// <summary>
  /// Ends the pulse.
  /// </summary>
  public void EndPulse() {
    Debug.Log ("ENDING PULSE");
    suitBody.AddForce (suitBody.velocity * -50 * pulseStrength);
    //reset other stuff
  }

  /// <summary>
  /// Gets the forward direction.
  /// </summary>
  /// <returns>The forward direction.</returns>
  public Vector3 GetForwardDirection() {
    return cameraPlayer.transform.forward;
  }


  /// <summary>
  /// Waits the seconds.
  /// </summary>
  /// <returns>The seconds.</returns>
  /// <param name="time">Time.</param>
  IEnumerator waitSeconds(float time) {
    yield return new WaitForSeconds (time);
  }

  /// <summary>
  /// Reflects the pulse.
  /// </summary>
  /*
   * public void ReflectPulse() {
    if (isColliding) {
      ForwardPulse();
    }
  }
    //
    //Colliding, reflect....
    float d = getDistanceInCollider();// Vector3.Distance(trackedObj.transform.position, point_enter);
    //float t = d / max_pulse_distance;

    dis = d;

    //t = Mathf.Clamp (t, 0.0f, 1.0f);



    //float force = t * max_pulse_force;

    Vector3 forward = getForwardDirection ();
    forward.y = 0;
    rb.AddForce (forward * d);
    Debug.Log(d);

    //SIZE SPHERE


  /// <summary>
  /// Gets the distance in collider.
  /// </summary>
  /// <returns>The distance in collider.</returns>
  public float GetDistanceInCollider() {
    Vector3 fake_point_enter = cameraPlayer.transform.position + enterDisplacement;

    //TODO
    //ROTATE FAKE POINT ENTER TO ORIENT WITH CAMERA
    //fake_point_enter.rotate(  );
    //
    //Transform cameraTransform = camera_player.transform;
    //Vector3 cameraPosition = cameraTransform.position;
    //transform.RotateAround(0, MoveColliderRight.rotation.y, 0);

    return Vector3.Distance(fake_point_enter, trackedObj.transform.position);
  }


 

  /// <summary>
  /// When trigger exits colliders, end pulse..
  /// </summary>
  /// <param name="other">Other.</param>
  void onTriggerExit(Collider other) {
    if(other.tag == "ForwardCollider") {
      EndPulse();
    }
  }

  /// <summary>
  /// Starts the pulse.
  /// </summary>
  public void StartPulse() {
    //Are we already colliding?
    if (canBoost) {
      Debug.Log("START PULSE");

      canBoost = false; // start colliding
      pointEnter = trackedObj.transform.position;
      enterDisplacement = cameraPlayer.transform.position - pointEnter;

      //ReflectPulse();
      ForwardPulse();
    }
  }
  */
}
