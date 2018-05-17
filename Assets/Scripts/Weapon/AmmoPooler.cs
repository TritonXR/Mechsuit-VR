using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPooler : Weapon {
  /// <summary>
  /// A Pool corresponds to a kind of ammo
  /// </summary>
  [System.Serializable]
  public class Pool {
    public string tag;
    public GameObject prefab;
    public int size;
  }

  /// <summary>
  /// A list of supported ammos by this weapon
  /// </summary>
  [SerializeField]
  public List<Pool> pools;

  private Dictionary<string, Pool> stringToPoolDictionary;
  private Dictionary<string, Queue<GameObject>> poolDictionary;

  /// <summary>
  /// Setup the weapon by initializing the magazine
  /// </summary>
  void Start() {
    stringToPoolDictionary = new Dictionary<string, Pool>();
    poolDictionary = new Dictionary<string, Queue<GameObject>>();
    foreach (Pool pool in pools) {
      stringToPoolDictionary.Add(pool.tag, pool);
      Queue<GameObject> objPool = new Queue<GameObject>();
      poolDictionary.Add(pool.tag, objPool);
    }
    Setup();
  }

  public override void Setup() {
    foreach (KeyValuePair<string, Queue<GameObject>> pair in poolDictionary) {
      Pool pool = stringToPoolDictionary[pair.Key];
      for (int i = 0; i < pool.size; ++i) {
        GameObject obj = Instantiate(pool.prefab);
        obj.SetActive(false);
        pair.Value.Enqueue(obj);
      }
    }
  }

  public override void Fire(string ammoTag) {
    if (!ArmController.isCalibrated) {
      Debug.Log("You have not calibrated!");
      return;
    }

    if (!poolDictionary.ContainsKey(ammoTag)) {
      Debug.Log("Projectile " + ammoTag + " not found");
      Debug.Log("Check spelling of tag");
      return;
    }


    if (poolDictionary[ammoTag].Count == 0) {
      Debug.Log("Magazine empty, awaiting reload");
      return;
    }

    Debug.Log("Successful fire");
    GameObject projectile = poolDictionary[ammoTag].Dequeue();

    Vector3 weaponPosition = modelWeapon.transform.position;
    Quaternion weaponRotation = modelWeapon.transform.rotation;
    Vector3 forwardDirection = modelWeapon.transform.forward;

    projectile.SetActive(true);
    projectile.transform.position = new Vector3(weaponPosition.x, weaponPosition.y, weaponPosition.z);
    projectile.transform.rotation = new Quaternion(weaponRotation.x, weaponRotation.y, weaponRotation.z, weaponRotation.w);
    projectile.transform.forward = new Vector3(forwardDirection.x, forwardDirection.y, forwardDirection.z);

    IAmmo pooledProjectile = projectile.GetComponent<IAmmo>();

    if (pooledProjectile != null) {
      Debug.Log("Projectile I_Ammo obtained");
      pooledProjectile.OnObjectSpawn();
    }
  }

public override void Reload(string ammoTag) {
  Setup();
}

void Update() {
  Debug.DrawRay(modelWeapon.transform.position, modelWeapon.transform.forward, Color.red);
}
}
