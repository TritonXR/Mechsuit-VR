using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementController : MonoBehaviour {
  [SerializeField]
  public List<Vector3> locations;

  public float speed;

  private int stop;
	
	// Update is called once per frame
	void Update () {
    Vector3 after = Vector3.MoveTowards(transform.position, locations[stop], speed * Time.deltaTime);
    if (after == transform.position) { // reached destination
      stop = (stop + 1) % locations.Count;
    } else {
      transform.position = after;
    }
	}
}
