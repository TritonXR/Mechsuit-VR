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

  private SteamVR_TrackedObject viveController;

  // Use this for initialization
  private SteamVR_Controller.Device DeviceInput {
    get { return SteamVR_Controller.Input ((int)viveController.index); }
  }

  void Awake() {
    viveController = GetComponent<SteamVR_TrackedObject> ();
  }

	// Use this for initialization
  void Start () {
    isLeft = Controller.name == "Controller (left)";
    isCalibrated = false;
    StartCoroutine(CalibrateArmLength());
  }
	
	// Update is called once per frame
	void Update () {
    // Shallow copy
    controllerAngle = Controller.eulerAngles;
    if (isCalibrated)
      ControllerCheck ();
		// ShoulderCheck (UpperArmCheck ( ElbowCheck ( LowerArmCheck ( HandCheck () ) ) ));
	}

  void ControllerCheck() {
    HandCheck();
  }

  void HandCheck() {
    //Hand.eulerAngles = new Vector3 (controllerAngle.x - 90.0f, -controllerAngle.z, controllerAngle.y);
    Hand.eulerAngles = new Vector3 (controllerAngle.x, controllerAngle.y, controllerAngle.z);
    float currArmLength = getArmLength ();
    if (currArmLength > maxArmLength)
      forearmToArm = 1.0f;
    else if (currArmLength < 0.0f)
      forearmToArm = 0.0f;
    else
      forearmToArm = currArmLength / maxArmLength;
    Vector3 handDirection = Vector3.Normalize(Controller.transform.position - PlayerShoulder.transform.position);
    Hand.transform.position = forearmToArm * maxArmLength * handDirection;
    ElbowCheck();
  }

  void ElbowCheck() {

  }

  void LowerArmCheck() {
    LowerArm.eulerAngles = new Vector3 (0, controllerAngle.y, 0);

    if ((isLeft && controllerAngle.y > 90.0f && controllerAngle.y < 180.0f) || (!isLeft && controllerAngle.y > 180.0f && controllerAngle.y < 270.0f)) {
      UpperArmCheck ();
    }

    else if ((isLeft && controllerAngle.y > 270.0f) || (!isLeft && controllerAngle.y < 90)) {
      UpperArmCheckReverse ();
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

	void UpperArmCheck() {
    //float yAngle = isLeft ? controllerAngle.y - 90.0f : controllerAngle.y + 90.0f;
    //float yAngle = isLeft ? controllerAngle.y - 90.0f : controllerAngle.y - 180.0f; // WORKS FOR RIGHT ARM, LEFT MISALIGNED BUT DOES NOT SNAP
    float yAngle = isLeft ? controllerAngle.y - 90.0f : controllerAngle.y + 90.0f;
    UpperArm.eulerAngles = new Vector3 (0, yAngle, 0);
	}

  void UpperArmCheckReverse() {
    //float yAngle = isLeft ? controllerAngle.y - 90.0f : controllerAngle.y + 90.0f;
    //float yAngle = isLeft ? controllerAngle.y - 90.0f : controllerAngle.y - 180.0f; // WORKS FOR RIGHT ARM, LEFT MISALIGNED BUT DOES NOT SNAP
    UpperArm.eulerAngles = new Vector3 (0, controllerAngle.y, 0);
  }

  void ShoulderCheck() {

  }

  private float getArmLength() {
    Vector3 controllerPos = Controller.transform.position;
    Vector3 shoulderPos = PlayerShoulder.transform.position;
    return Mathf.Sqrt(Mathf.Pow(controllerPos.x - shoulderPos.x, 2.0f)
                     +Mathf.Pow(controllerPos.y - shoulderPos.y, 2.0f)
                     +Mathf.Pow(controllerPos.z - shoulderPos.z, 2.0f));
  }
    
  IEnumerator CalibrateArmLength() {
    if (DeviceInput == null) {
      Debug.Log ("Null controller value, attempting to fetch.");
      viveController = GetComponent<SteamVR_TrackedObject> ();
      if (DeviceInput == null) {
        Debug.Log ("Fetch failed");
        //return;
        yield break;
      }
    }

    // First check
    Debug.Log ("Ready to check shoulder position.");

    Vector3 firstPosition;
    yield return new WaitUntil(DeviceInput.GetHairTriggerDown);
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

    // Second check
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

  IEnumerator WaitForButtonPress() {
    yield return new WaitWhile (DeviceInput.GetHairTriggerUp);
    Debug.Log ("Breaks from loop");
    yield break;
  }
}