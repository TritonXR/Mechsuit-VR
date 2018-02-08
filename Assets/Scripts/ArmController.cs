using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {

  #region Variables
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
  [Range(0.2f, 0.8f)]
  private static float forearmToArm = 0.5f;
  [Range(0.0f, 1.0f)]
  private float armExtend;
  private bool isCalibrated = false;
  [SerializeField]
  private static float MECH_ARM_LENGTH = 5f;
  [SerializeField]
  private static float minArmExtend = 0.1f;
  private static float mechUpperArmLength = MECH_ARM_LENGTH * (1 - forearmToArm);
  #endregion

  #region Controller and Awake
  private SteamVR_TrackedObject viveController;
  private SteamVR_Controller.Device DeviceInput {
    get { return SteamVR_Controller.Input((int)viveController.index); }
  }

  void Awake() {
    viveController = GetComponent<SteamVR_TrackedObject>();
  }
  #endregion

  private float ArmLength {
    get {
      Vector3 controllerPos = Controller.transform.position;
      Vector3 shoulderPos = PlayerShoulder.transform.position;
      return Mathf.Sqrt(Mathf.Pow(controllerPos.x - shoulderPos.x, 2.0f)
        + Mathf.Pow(controllerPos.y - shoulderPos.y, 2.0f)
        + Mathf.Pow(controllerPos.z - shoulderPos.z, 2.0f));
    }
  }

  /// <summary>
  /// Determine left/right controller, and start calibration process
  /// </summary>
  void Start() {
    isLeft = Controller.name == "Controller (left)";
    isCalibrated = false;
    StartCoroutine(CalibrateArmLength());
    Debug.Log("UPL: " + mechUpperArmLength);
  }

  /// <summary>
  /// If we have calibrated the controllers, update the inverse kinematics
  /// </summary>
  void Update() {
    // Shallow copy
    if (isCalibrated) {
      HandCheck();
      /* After updating the hand, update the elbow */
      
      
      //ControllerCheck ();
      //ShoulderCheck (UpperArmCheck ( ElbowCheck ( LowerArmCheck ( HandCheck () ) ) ));
    } else {
      armExtend = 1.0f;
      Hand.transform.position = Shoulder.position + MECH_ARM_LENGTH * armExtend * Vector3.forward;
    }
  }

  /// <summary>
  /// Rotation and position update for hands.
  /// </summary>
  void HandCheck() {
    /* Rotation update */
    controllerAngle = Controller.eulerAngles;
    Hand.eulerAngles = new Vector3(controllerAngle.x, controllerAngle.y, controllerAngle.z);

    /* Position update */
    armExtend = ArmLength / maxArmLength;
    if (armExtend > 1.0f)
      armExtend = 1.0f;
    else if (armExtend < minArmExtend)
      armExtend = minArmExtend;
    //Vector3 handDirection = Vector3.Normalize(Controller.transform.position - PlayerShoulder.transform.position);
    Vector3 handDirection = Vector3.Normalize(Controller.position - PlayerShoulder.position);
    Hand.position = Shoulder.position + MECH_ARM_LENGTH * armExtend * handDirection;
    ElbowCheck();
  }

  /// <summary>
  /// Update elbow position
  /// </summary>
  void ElbowCheck() {
    // The "naive" way
    Vector3 a = (1.0f - armExtend) * Hand.up + armExtend * Vector3.Normalize(Shoulder.position - Hand.position);
    Vector3 elbowDirection = a * forearmToArm * MECH_ARM_LENGTH;
    Elbow.position = Hand.position + elbowDirection;

    // Based on that, account for wrist rotations
    if (armExtend < 1.0f) {
      float newX = Elbow.position.x;

      if (isLeft) {
        newX -= mechUpperArmLength * (1.0f - armExtend);
      }
      
      else {
        newX += mechUpperArmLength * (1.0f - armExtend);
      }

      float newY = Elbow.position.y;
      float newZ = Elbow.position.z;
      //if (isLeft) xOffset *= -1;
      // Calculations that take isLeft into account
      // Elbow.position.x * armExtend + (mechUpperArmLength + Shoulder.position.x) * (1 - armExtend)
      Elbow.position = new Vector3(newX, newY, newZ);
    }

    LowerArmCheck();
    UpperArmCheck();
  }

  /// <summary>
  /// Rotation and Position update for lower arm.
  /// </summary>
  void LowerArmCheck() {

    /* Rotation update */
    //LowerArm.eulerAngles = new Vector3(LowerArm.eulerAngles.x, controllerAngle.y, LowerArm.eulerAngles.z);
    float yAngle = controllerAngle.y;
    LowerArm.eulerAngles = new Vector3(0, yAngle, 0);

    /* Update y rotation of upper arm */
    if ((isLeft && controllerAngle.y > 90.0f && controllerAngle.y < 180.0f) || (!isLeft && controllerAngle.y > 180.0f && controllerAngle.y < 270.0f)) {
      yAngle = isLeft ? controllerAngle.y - 90.0f : controllerAngle.y + 90.0f;
      UpperArm.eulerAngles = new Vector3(UpperArm.eulerAngles.x, yAngle, UpperArm.eulerAngles.z);
    } else if ((isLeft && controllerAngle.y > 270.0f) || (!isLeft && controllerAngle.y < 90)) {
      UpperArm.eulerAngles = new Vector3(UpperArm.eulerAngles.x, controllerAngle.y, UpperArm.eulerAngles.z);
    }

    Vector3 lowerArmDirection = Vector3.Normalize(Elbow.position - Hand.position);
    //LowerArm.position = Elbow.position;
    
    LowerArm.position = new Vector3(Elbow.position.x, Elbow.position.y + 0.07f, Elbow.position.z);
    LowerArm.up = lowerArmDirection;
    LowerArm.Rotate(0.0f, Hand.eulerAngles.y, 0.0f);
    //LowerArm.eulerAngles = new Vector3(LowerArm.eulerAngles.x, yAngle, LowerArm.eulerAngles.z);


    #region Concept script to avoid jumping from forbidden angle to the greatest possible angle for the upper arm.
    //    if (upperCanRotate || ((isLeft && controllerAngle.y > 90.0f && controllerAngle.y < 95.0f) || (!isLeft && controllerAngle.y > 180.0f && controllerAngle.y < 185.0f))) {
    //      upperCanRotate = true;
    //      if ((controllerAngle.y > 90.0f && controllerAngle.y < 180.0f && upperCanRotate) || (!isLeft && controllerAngle.y > 180.0f && controllerAngle.y < 270.0f && upperCanRotate))
    //        UpperArmCheck ();
    //    } 

    //    else if ((isLeft && controllerAngle.y < 90.0f) || (!isLeft && controllerAngle.y < 180.0f))
    //      upperCanRotate = false;
    #endregion
  }

  /// <summary>
  /// x, z Rotation and position update for lower arm.
  /// </summary>
	void UpperArmCheck() {
    Vector3 upperArmDirection = Vector3.Normalize(UpperArm.position - Elbow.position);
    UpperArm.up = upperArmDirection;
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
      Debug.Log("Null controller value, attempting to fetch.");
      viveController = GetComponent<SteamVR_TrackedObject>();
      if (DeviceInput == null) {
        Debug.Log("Fetch failed");
        //return;
        yield break;
      }
    }

    /* First check */
    Debug.Log("Ready to check shoulder position.");

    Vector3 firstPosition;
    yield return new WaitUntil(DeviceInput.GetHairTriggerDown);

    Debug.Log("Trigger pulled, expected at shoulder.");
    firstPosition = new Vector3(
      Controller.transform.position.x,
      Controller.transform.position.y,
      Controller.transform.position.z
    );

    // Move our empty shoulder gameobject to the player's assumed shoulder position.
    PlayerShoulder.position = firstPosition;

    Debug.Log("firstPosition is ("
                + firstPosition.x + ", "
                + firstPosition.y + ", "
                + firstPosition.z + ")");

    yield return new WaitForSeconds(1.0f);

    /* Second check */
    Vector3 secondPosition;
    yield return new WaitUntil(DeviceInput.GetHairTriggerDown);

    Debug.Log("Trigger pulled, expected arm to be extended.");
    secondPosition = new Vector3(
      Controller.transform.position.x,
      Controller.transform.position.y,
      Controller.transform.position.z
    );
    Debug.Log("secondPosition is ("
                + secondPosition.x + ", "
                + secondPosition.y + ", "
                + secondPosition.z + ")");

    maxArmLength = Mathf.Sqrt(Mathf.Pow(firstPosition.x - secondPosition.x, 2.0f)
                             + Mathf.Pow(firstPosition.y - secondPosition.y, 2.0f)
                             + Mathf.Pow(firstPosition.z - secondPosition.z, 2.0f));
    Debug.Log(isLeft + ", length: " + maxArmLength);

    isCalibrated = true;
  }
}