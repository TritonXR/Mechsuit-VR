﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, I_Ammo {

  public uint lifetime = 0;

  public float speed;

  private uint currTime = 0;

	// Use this for initialization
	void Start () {
    lifetime = (uint) (lifetime / Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
    if (currTime <= lifetime) {
      Debug.DrawRay(this.transform.position, this.transform.forward, Color.green);
      this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
      currTime++;
    }

    else {
      // TODO: implement reload functionality
      //Destroy(this.gameObject);
    }
	}

  public void OnObjectSpawn() {

  }

  public void OnCollisionEnter(Collision collision) {
    //Destroy(this.gameObject);
  }

}
