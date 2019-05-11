﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using Valve.VR;

public class GestureAgent : Agent {

  //[Header("Agent for recognizing controller gestures")]
  public Transform LeftController { get { return transform.GetChild(0); } }
  public Transform RightController { get { return transform.GetChild(1); } }

  public List<Vector3> controllerData;
  public int controllerIndex;

  public override void InitializeAgent() {
    //ballRb = ball.GetComponent<Rigidbody>();
    controllerData = new List<Vector3>();
  }

  public override void CollectObservations() {
    /*
    AddVectorObs(gameObject.transform.rotation.z);
    AddVectorObs(gameObject.transform.rotation.x);
    AddVectorObs(ball.transform.position - gameObject.transform.position);
    AddVectorObs(ballRb.velocity);
    */
    // Change this to check that the vive trackpad is pressed down
    if (SteamVR_Actions._default.Teleport.GetState(SteamVR_Input_Sources.RightHand)) {
      AddVectorObs(RightController.localPosition);
      AddVectorObs(RightController.localEulerAngles);
    }
  }

  public override void AgentAction(float[] vectorAction, string textAction) {

    /*if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous) {
      var actionZ = 2f * Mathf.Clamp(vectorAction[0], -1f, 1f);
      var actionX = 2f * Mathf.Clamp(vectorAction[1], -1f, 1f);

      if ((gameObject.transform.rotation.z < 0.25f && actionZ > 0f) ||
          (gameObject.transform.rotation.z > -0.25f && actionZ < 0f)) {
        gameObject.transform.Rotate(new Vector3(0, 0, 1), actionZ);
      }

      if ((gameObject.transform.rotation.x < 0.25f && actionX > 0f) ||
          (gameObject.transform.rotation.x > -0.25f && actionX < 0f)) {
        gameObject.transform.Rotate(new Vector3(1, 0, 0), actionX);
      }
    }
    if ((ball.transform.position.y - gameObject.transform.position.y) < -2f ||
        Mathf.Abs(ball.transform.position.x - gameObject.transform.position.x) > 3f ||
        Mathf.Abs(ball.transform.position.z - gameObject.transform.position.z) > 3f) {
      Done();
      SetReward(-1f);
    } else {
      SetReward(0.1f);
    }*/
    // TODO discrete or continuous action space?
    if (brain.brainParameters.vectorActionSpaceType == SpaceType.discrete) {
      Debug.Log(vectorAction);
    }
  }

  public override void AgentReset() {
    /*
    gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    gameObject.transform.Rotate(new Vector3(1, 0, 0), Random.Range(-10f, 10f));
    gameObject.transform.Rotate(new Vector3(0, 0, 1), Random.Range(-10f, 10f));
    ballRb.velocity = new Vector3(0f, 0f, 0f);
    ball.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 4f, Random.Range(-1.5f, 1.5f))
                                  + gameObject.transform.position;
    */
  }
}