using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, I_Ammo {

  public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    this.gameObject.transform.Translate(this.transform.forward * speed);
	}

  public void OnObjectSpawn() {

  }

}
