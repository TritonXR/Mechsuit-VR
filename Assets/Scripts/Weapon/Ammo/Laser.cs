using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IAmmo {

  public uint lifetime;

  public float speed;

  private uint currTime = 0;

  public GameObject Weapon { get; set; }

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
      Destroy(this.gameObject);
    }
	}

  public void OnObjectSpawn() {

  }

  /// <summary>
  /// After colliding with an object with a health, reduces its health.
  /// </summary>
  /// <param name="collder"></param>
  public void OnTriggerEnter(Collider collider) {
    print(collider.gameObject.name);
    if (collider.gameObject != Weapon) {
      IHealth health = (IHealth)collider.gameObject.GetComponent(typeof(IHealth));
      if (health != null) {
        IHealthChange damage = (IHealthChange)GetComponent(typeof(IHealthChange));
        damage.ChangeHealth(health);
      }
    }
  }

  public void OnCollisionEnter(Collision collision) {
    print(collision.gameObject.name);
  }

}
