using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnCollisionEnter(Collision collision) {
    Debug.Log("Triggered OnCollisionEnter");
    if (collision.gameObject.tag == "Projectile") {
      //Destroy(this.gameObject);
      Debug.Log("Ping!");
    }
  }
}
