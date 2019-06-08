using UnityEngine;

public class Whip : Weapon {
    public int[] damages;

    // Start is called before the first frame update
    public override void Setup() {
        int index = 0;
        foreach (Transform segment in transform) {
            segment.gameObject.AddComponent<WhipSegment>();
            SimpleDamage damage = segment.gameObject.AddComponent<SimpleDamage>();
            damage.type = DamageType.physical;
            damage.value = damages[index++];
        }
    }
}
