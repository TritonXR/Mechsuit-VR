using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {

  [Header("Body Parts")]
  public Transform Shoulder;
	public Transform UpperArm;
	public Transform Elbow;
	public Transform LowerArm;
	public Transform Hand;
	public Transform Controller;
  public Transform PlayerShoulder;

  [Header("Attributes")]
  [SerializeField]
  private bool isLeft;
  [SerializeField]
  private Vector3 controllerAngle;
  [SerializeField]
  private bool upperCanRotate;
  [SerializeField]
  private float maxArmLength;
  [SerializeField]
  [Range(0.4f, 0.6f)]
  private float forearmToArm;
  [Range(0.0f, 1.0f)]
  private float armExtend;
  private bool isCalibrated = false;
  [SerializeField]
  private float MECH_ARM_LENGTH = 5f;

  // Controller
  private SteamVR_TrackedObject viveController;
  private SteamVR_Controller.Device DeviceInput {
    get { return SteamVR_Controller.Input ((int)viveController.index); }
  }


  private float ArmLength {
    get {
      Vector3 controllerPos = Controller.transform.position;
      Vector3 shoulderPos = PlayerShoulder.transform.position;
      return Mathf.Sqrt(Mathf.Pow(controllerPos.x - shoulderPos.x, 2.0f)
        + Mathf.Pow(controllerPos.y - shoulderPos.y, 2.0f)
        + Mathf.Pow(controllerPos.z - shoulderPos.z, 2.0f));
    }
  }

  void Awake() {
    viveController = GetComponent<SteamVR_TrackedObject> ();
  }


	/// <summary>
  /// Determine left/right controller, and start calibration process
  /// </summary>
  void Start () {
    isLeft = Controller.name == "Controller (left)";
    isCalibrated = false;
    StartCoroutine(CalibrateArmLength());
  }
	
	/// <summary>
  /// If we have calibrated the controllers, update the inverse kinematics
  /// </summary>
	void Update () {
    // Shallow copy
    if (isCalibrated) {
      controllerAngle = Controller.eulerAngles;
      HandCheck ();
      //ControllerCheck ();
		  //ShoulderCheck (UpperArmCheck ( ElbowCheck ( LowerArmCheck ( HandCheck () ) ) ));
    }
	}

  //void ControllerCheck() {
  //  HandCheck();
  //}

  /// <summary>
  /// Rotation and position update for hands.
  /// </summary>
  void HandCheck() {
    /* Rotation update */
    Hand.eulerAngles = new Vector3 (controllerAngle.x, controllerAngle.y, controllerAngle.z);

    /* Position update */
    if (ArmLength > maxArmLength)
      forearmToArm = 0.6f; //1.0f;
    else if (ArmLength < 0.0f)
      forearmToArm = 0.04f; //0.0f
    else
      forearmToArm = ArmLength / maxArmLength;
    Vector3 handDirection = Vector3.Normalize(Controller.transform.position - PlayerShoulder.transform.position);
    Hand.transform.position = MECH_ARM_LENGTH * forearmToArm * maxArmLength * handDirection;

    /* After updating the hand, update the elbow */
    ElbowCheck();
  }

  /// <summary>
  /// TODO: Update elbow position
  /// </summary>
  void ElbowCheck() {

  }

  /// <summary>
  /// Rotation and TODO: position update for lower arm.
  /// </summary>
  void LowerArmCheck() {
    /* Rotation update */
    LowerArm.eulerAngles = new Vector3 (0, controllerAngle.y, 0);

    /* Update y rotation of upper arm */
    if ((isLeft && controllerAngle.y > 90.0f && controllerAngle.y < 180.0f) || (!isLeft && controllerAngle.y > 180.0f && controllerAngle.y < 270.0f)) {
      float yAngle = isLeft ? controllerAngle.y - 90.0f : controllerAngle.y + 90.0f;
      UpperArm.eulerAngles = new Vector3 (UpperArm.eulerAngles.x, yAngle, UpperArm.eulerAngles.z);
    }

    else if ((isLeft && controllerAngle.y > 270.0f) || (!isLeft && controllerAngle.y < 90)) {
      UpperArm.eulerAngles = new Vector3 (UpperArm.eulerAngles.x, controllerAngle.y, UpperArm.eulerAngles.z);
    }

    //Concept script to avoid jumping from forbidden angle to the greatest possible angle for the upper arm.
    //    if (upperCanRotate || ((isLeft && controllerAngle.y > 90.0f && controllerAngle.y < 95.0f) || (!isLeft && controllerAngle.y > 180.0f && controllerAngle.y < 185.0f))) {
    //      upperCanRotate = true;
    //      if ((controllerAngle.y > 90.0f && controllerAngle.y < 180.0f && upperCanRotate) || (!isLeft && controllerAngle.y > 180.0f && controllerAngle.y < 270.0f && upperCanRotate))
    //        UpperArmCheck ();
    //    } 

    //    else if ((isLeft && controllerAngle.y < 90.0f) || (!isLeft && controllerAngle.y < 180.0f))
    //      upperCanRotate = false;
  }

  /// <summary>
  /// x, z Rotation and position update for lower arm.
  /// </summary>
	void UpperArmCheck() {
    
	}


  /// <summary>
  /// Rotation and position update for shoulder.
  /// </summary>
  void ShoulderCheck() {

  }

  /// <summary>
  /// Calibrates the length of the arm.
  /// </summary>
  /// <returns>an IEnumerator as a coroutine.</returns>
  IEnumerator CalibrateArmLength() {
    /* Check if the controller is on */
    if (DeviceInput == null) {
      Debug.Log ("Null controller value, attempting to fetch.");
      viveController = GetComponent<SteamVR_TrackedObject> ();
      if (DeviceInput == null) {
        Debug.Log ("Fetch failed");
        //return;
        yield break;
      }
    }

    /* First check */
    Debug.Log ("Ready to check shoulder position.");

    Vector3 firstPosition;
    //yield return new WaitUntil(DeviceInput.GetHairTriggerDown);
    yield return StartCoroutine(WaitForButtonPress());
    //yield return new WaitUntil(()=> DeviceInput.GetHairTriggerDown() == true);
    //yield return new WaitWhile(DeviceInput.GetHairTriggerUp);

    Debug.Log ("Trigger pulled, expected at shoulder.");
    firstPosition = new Vector3 (
      Controller.transform.position.x,
      Controller.transform.position.y,
      Controller.transform.position.z
    );

    // Move our empty shoulder gameobject to the player's assumed shoulder position.
    PlayerShoulder.position = firstPosition;

    Debug.Log ("firstPosition is ("
                +firstPosition.x+", "
                +firstPosition.y+", "
                +firstPosition.z+")");

    yield return new WaitForSeconds(1.0f);

    /* Second check */
    Vector3 secondPosition;
    yield return new WaitUntil(DeviceInput.GetHairTriggerDown);

    Debug.Log ("Trigger pulled, expected arm to be extended.");
    secondPosition = new Vector3 (
      Controller.transform.position.x,
      Controller.transform.position.y,
      Controller.transform.position.z
    );
    Debug.Log ("secondPosition is ("
                +secondPosition.x+", "
                +secondPosition.y+", "
                +secondPosition.z+")");

    maxArmLength = Mathf.Sqrt(Mathf.Pow(firstPosition.x - secondPosition.x, 2.0f)
                             +Mathf.Pow(firstPosition.y - secondPosition.y, 2.0f)
                             +Mathf.Pow(firstPosition.z - secondPosition.z, 2.0f));
    Debug.Log (isLeft + ", length: " + maxArmLength);

    isCalibrated = true;
  }

  /// <summary>
  /// Waits for button press.
  /// </summary>
  /// <returns>an IEnumerator as a coroutine.</returns>
  IEnumerator WaitForButtonPress() {
    /*yield return new WaitWhile (DeviceInput.GetHairTriggerUp);
    Debug.Log ("Breaks from loop");
    yield break;*/
    bool wait = true;
    while (wait) {
      if (DeviceInput.GetHairTriggerDown ()) {
        Debug.Log ("Hair trigger pressed");
        wait = false;
      }
      yield return null;
    }
    yield return null;
  }
}