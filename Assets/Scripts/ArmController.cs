using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {

	public Transform Shoulder;
	public Transform UpperArm;
	public Transform Elbow;
	public Transform LowerArm;
	public Transform Hand;
	public Transform Controller;
  public Transform PlayerShoulder;


  /// <summary>
  /// left -- true, right -- false
  /// </summary>
  [SerializeField]
  private bool isLeft;
  [SerializeField]
  private Vector3 controllerAngle;
  [SerializeField]
  private bool upperCanRotate;
  [SerializeField]
  private float maxArmLength;

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
    StartCoroutine(Calibrate());
  }
	
	// Update is called once per frame
	void Update () {
    controllerAngle = Controller.eulerAngles;
    ControllerCheck ();
		// ShoulderCheck (UpperArmCheck ( ElbowCheck ( LowerArmCheck ( HandCheck () ) ) ));
	}

	void ShoulderCheck() {

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

	void HandCheck() {
    //Hand.eulerAngles = new Vector3 (controllerAngle.x - 90.0f, -controllerAngle.z, controllerAngle.y);
    Hand.eulerAngles = new Vector3 (controllerAngle.x, controllerAngle.y, controllerAngle.z);
    LowerArmCheck();
	}

  void ControllerCheck(){
    HandCheck();
  }

  float CalibrateArmLength() {
    float toRet = Mathf.Sqrt(Mathf.Pow(Hand.transform.position.x - Shoulder.transform.position.x, 2.0f)
                    + Mathf.Pow(Hand.transform.position.y - Shoulder.transform.position.y, 2.0f)
                    + Mathf.Pow(Hand.transform.position.z - Shoulder.transform.position.z, 2.0f));
    Debug.Log ("ArmLength is " + toRet);
    return toRet;
  }

  IEnumerator Calibrate() {
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

    Debug.Log ("Trigger pulled, expected at shoulder.");
    firstPosition = new Vector3 (
      Controller.transform.position.x,
      Controller.transform.position.y,
      Controller.transform.position.z
    );
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

    /*maxArmLength = Mathf.Sqrt(Mathf.Pow(firstPosition.transform.position.x - secondPosition.transform.position.x, 2.0f)
      + Mathf.Pow(firstPosition.transform.position.y - secondPosition.transform.position.y, 2.0f)
      + Mathf.Pow(firstPosition.transform.position.z - secondPosition.transform.position.z, 2.0f));*/
    maxArmLength = Mathf.Sqrt(Mathf.Pow(firstPosition.x - secondPosition.x, 2.0f)
                             +Mathf.Pow(firstPosition.y - secondPosition.y, 2.0f)
                             +Mathf.Pow(firstPosition.z - secondPosition.z, 2.0f));
    Debug.Log (isLeft + ", length: " + maxArmLength);

    isCalibrated = true;
  }

  IEnumerator WaitForButtonPress() {
    /*while (!DeviceInput.GetHairTriggerDown ()) {
      yield return new WaitForSeconds(2.0f);
    }*/
    yield return new WaitWhile (() => DeviceInput.GetHairTriggerUp());
    Debug.Log ("Breaks from loop");
    yield break;
  }
}