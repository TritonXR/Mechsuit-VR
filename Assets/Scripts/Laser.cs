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
    Debug.DrawRay(this.transform.position, this.transform.forward, Color.green);
    this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

  public void OnObjectSpawn() {

  }

  public void OnCollisionEnter(Collision collision) {
    //Destroy(this.gameObject);
  }

}
