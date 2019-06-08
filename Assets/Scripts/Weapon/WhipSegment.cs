using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipSegment : MonoBehaviour {

    public GameObject CoreDefense = GameObject.Find("CoreDefense");

    public void OnTriggerEnter(Collider collider) {
        if (collider.gameObject != CoreDefense) {
            // Health of other
            IHealth health = (IHealth)collider.gameObject.GetComponent(typeof(IHealth));
            if (health != null) {
                IHealthChange damage = (IHealthChange)GetComponent(typeof(IHealthChange));
                damage.ChangeHealth(health);
            }
        }
    }
}
