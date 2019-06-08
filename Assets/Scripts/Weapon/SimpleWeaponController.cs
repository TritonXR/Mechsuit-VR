using UnityEngine;

public class SimpleWeaponController : MonoBehaviour {
    /* Weapon-related variables */
    public Weapon currentWeapon;
    [HideInInspector]
    public bool equipped = true;

    // TODO: refactor those out of controller because close-range weapons like swords don't need those
    public string[] ammoType;
    public float[] fireDelay;
    protected int currAmmoIndex;
    protected float currDelay;
}
