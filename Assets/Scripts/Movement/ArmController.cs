using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {
  #region Variables
  [Header("Body Parts")]
  public Transform shoulder;
  public Transform upperArm;
  public Transform elbow;
  public Transform lowerArm;
  public Transform hand;
  public Transform controller;
  public Transform playerShoulder;
  
  [Header("Attributes")]
  [SerializeField]
  private bool upperCanRotate;
  [SerializeField]
  private float maxArmLength;
  [SerializeField]
  [Range(0.2f, 0.8f)]
  private static float forearmToArm = 0.5f;
  [Range(0.0f, 1.0f)]
  private float armExtend;
  public static bool isCalibrated = false;
  [SerializeField]
  private static float MECH_ARM_LENGTH = 6.0f;
  [SerializeField]
  private static float minArmExtend = 0.1f;
  private static float mechUpperArmLength = MECH_ARM_LENGTH * (1 - forearmToArm);
  private static float mechLowerArmLength = MECH_ARM_LENGTH * forearmToArm;

  private static float upperArmRadius = mechUpperArmLength + 0.4000002f;


  private Vector3 firstPosition, secondPosition;

  /// <summary>
  /// Stage in the calibration process.
  /// 0 = uncalibrated, 1 = shoulder calibrated, 2 = shoulder and arm length fully calibrated
  /// </summary>
  private int stage = 0;
  #endregion

  #region Attributes
  private float ArmLength {
    get {
      return Vector3.Distance(controller.position, playerShoulder.position);
    }
  }

  private bool IsLeft {
    get {
      return trackedController.controllerIndex == 2;
    }
  }

  private Vector3 ControllerAngle {
    get {
      return controller.eulerAngles;
    }
  }
  #endregion

  #region Controller and Awake
  private SteamVR_TrackedController trackedController;

  void Awake() {
    trackedController = GetComponent<SteamVR_TrackedController>();
    trackedController.TriggerClicked += Calibrate;
  }
  #endregion

  /// <summary>
  /// Determine left/right controller, and start calibration process
  /// </summary>
  void Start() {
    isCalibrated = false;

    armExtend = 1.0f;
    hand.position = upperArm.position + MECH_ARM_LENGTH * armExtend * hand.forward;
    Debug.Log("Ready to check shoulder position.");
  }


  /// <summary>
  /// If we have calibrated the controllers, update the inverse kinematics
  /// </summary>
  void Update() {
    if (isCalibrated) {
      HandCheck();
    }
  }


  #region Inverse Kinematics
  /// <summary>
  /// Rotation and position update for hands.
  /// </summary>
  void HandCheck() {
    /* Rotation update */
    hand.eulerAngles = controller.eulerAngles;

    /* Position update */
    armExtend = ArmLength / maxArmLength;
    if (armExtend > 1.0f) {
      armExtend = 1.0f;
    } else if (armExtend < minArmExtend) {
      armExtend = minArmExtend;
    }
    Vector3 handDirection = Vector3.Normalize(controller.position - playerShoulder.position);
    hand.position = upperArm.position + MECH_ARM_LENGTH * armExtend * handDirection;
    ElbowCheck();
  }


  /// <summary>
  /// Update elbow position
  /// </summary>
  void ElbowCheck() {
    // The "naive" way
    Vector3 a = (1.0f - armExtend) * hand.up + armExtend * Vector3.Normalize(upperArm.position - hand.position);
    Vector3 elbowDirection = a * forearmToArm * MECH_ARM_LENGTH;
    elbow.position = hand.position + elbowDirection;

    // Based on that, account for wrist rotations
    if (armExtend < 1.0f) {
      //float newX = Elbow.position.x;

      if (IsLeft) {
        //newX -= mechUpperArmLength * (1.0f - armExtend);
        elbow.position -= elbow.forward * mechUpperArmLength * (1.0f - armExtend);
      } else {
        //newX += mechUpperArmLength * (1.0f - armExtend);
        elbow.position += elbow.forward * mechUpperArmLength * (1.0f - armExtend);
      }

      float newY = elbow.position.y;
      float newZ = elbow.position.z;
      Vector3 newElbowPosition = new Vector3(elbow.position.x, newY, newZ);
      //float elbowDistance = Vector3.Distance(newElbowPosition, UpperArm.position);
      float elbowDistance = Vector3.Distance(elbow.position, upperArm.position);
      if (elbowDistance < mechUpperArmLength) {
        //newElbowPosition = UpperArm.position + mechUpperArmLength * Vector3.Normalize(newElbowPosition - UpperArm.position);
        newElbowPosition = upperArm.position + mechUpperArmLength * Vector3.Normalize(elbow.position - upperArm.position);
      }
      // Calculations that take isLeft into account
      elbow.position = newElbowPosition;
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
    float yAngle = ControllerAngle.y;
    lowerArm.eulerAngles = new Vector3(0, yAngle, 0);

    //float handToElbowDistance = Vector3.Distance(elbow.position, hand.position);
    //LowerArm.position = new Vector3(Elbow.position.x, Elbow.position.y + 0.5f * (handToElbowDistance - mechLowerArmLength)/* + 0.02f*/, Elbow.position.z);
    lowerArm.position = elbow.position;
    lowerArm.up = Vector3.Normalize(elbow.position - hand.position);
    lowerArm.Rotate(0.0f, hand.eulerAngles.y, 0.0f);
 
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
    Vector3 upperArmDirection = Vector3.Normalize(upperArm.position - elbow.position);
    upperArm.up = upperArmDirection;
    // Update y rotation of upper arm 
    /*float yAngle = controllerAngle.y;
    if ((isLeft && controllerAngle.y > 90.0f && controllerAngle.y < 180.0f) || (!isLeft && controllerAngle.y > 180.0f && controllerAngle.y < 270.0f)) {
      yAngle = isLeft ? controllerAngle.y - 90.0f : controllerAngle.y + 90.0f;
      UpperArm.eulerAngles = new Vector3(UpperArm.eulerAngles.x, yAngle, UpperArm.eulerAngles.z);
    } else if ((isLeft && controllerAngle.y > 270.0f) || (!isLeft && controllerAngle.y < 90)) {
      UpperArm.eulerAngles = new Vector3(UpperArm.eulerAngles.x, controllerAngle.y, UpperArm.eulerAngles.z);
    }*/
    
  }


  /// <summary>
  /// Rotation and position update for shoulder.
  /// </summary>
  void ShoulderCheck() {

  }
#endregion

  #region Calibration
  void Calibrate(object sender, ClickedEventArgs e) {
    if (stage == 0) { // First check
      Debug.Log("Trigger pulled, expected at shoulder.");
      firstPosition = controller.position;

      // Move our empty shoulder gameobject to the player's assumed shoulder position.
      playerShoulder.position = firstPosition;

      Debug.Log("firstPosition is ("
                  + firstPosition.x + ", "
                  + firstPosition.y + ", "
                  + firstPosition.z + ")");

      stage = 1;
    } else if (stage == 1) { // Second check
      Debug.Log("Trigger pulled, expected arm to be extended.");
      secondPosition = controller.position;
      Debug.Log("secondPosition is ("
                  + secondPosition.x + ", "
                  + secondPosition.y + ", "
                  + secondPosition.z + ")");

      maxArmLength = Vector3.Distance(firstPosition, secondPosition);

      string left = IsLeft ? "Left" : "Right";
      Debug.Log(left + " arm, length: " + maxArmLength);

      isCalibrated = true;
      Debug.Log("Upper Arm Length: " + mechUpperArmLength);

      stage = 2;
    }
  }
#endregion
} // end of public class armController