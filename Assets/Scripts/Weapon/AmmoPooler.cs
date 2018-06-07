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

  /// <summary>
  /// Creates the ammo and adds them to the magazine.
  /// </summary>
  public override void Setup() {
    foreach (KeyValuePair<string, Queue<GameObject>> pair in poolDictionary) {
      pair.Value.Clear();
      Pool pool = stringToPoolDictionary[pair.Key];
      for (int i = 0; i < pool.size; ++i) {
        GameObject obj = Instantiate(pool.prefab);
        obj.SetActive(false);
        IAmmo ammo = (IAmmo)obj.GetComponent(typeof(IAmmo));
        ammo.Weapon = controller.gameObject;
        pair.Value.Enqueue(obj);
      }
    }
  }

  /// <summary>
  /// Fires the gun.
  /// </summary>
  /// <param name="ammoTag"></param>
  /// <returns>true if successfully fires; false if not calibrated (if controlled by the user), does not contain the correct ammo type, and/or the magazine is empty</returns>
  public override bool Activate(string ammoTag) {
    if (!manager.BothCalibrated) {
      Debug.Log("You have not calibrated!");
      return false;
    }

    if (!poolDictionary.ContainsKey(ammoTag)) {
      Debug.Log("Projectile " + ammoTag + " not found");
      Debug.Log("Check spelling of tag");
      return false;
    }


    if (poolDictionary[ammoTag].Count == 0) {
      Debug.Log("Magazine empty, awaiting reload");
      return false;
    }

    GameObject projectile = poolDictionary[ammoTag].Dequeue();
    Debug.Log("Fire for " + gameObject.name + ", remaining count: " + poolDictionary[ammoTag].Count);
    projectile.SetActive(true);
    projectile.transform.position = modelWeapon.transform.position;
    projectile.transform.rotation = modelWeapon.transform.rotation;
    projectile.transform.forward = modelWeapon.transform.forward;

    IAmmo pooledProjectile = projectile.GetComponent<IAmmo>();

    if (pooledProjectile != null) {
      pooledProjectile.OnObjectSpawn();
    }

    AudioSource sound = GetComponent<AudioSource>();
    if (sound != null) {
      sound.Play();
    }
    return true;
  }

  /// <summary>
  /// Reloads the gun.
  /// </summary>
  /// <param name="ammoTag"></param>
  /// <returns>True if successfully reloads; false if the magazine is full.</returns>
  public override bool ReActivate(string ammoTag) {
    if (poolDictionary[ammoTag].Count != stringToPoolDictionary[ammoTag].size) {
      Setup();
      return true;
    } else {
      return false;
    }
  }

  void Update() {
    Debug.DrawRay(modelWeapon.transform.position, modelWeapon.transform.forward, Color.red);
  }

  public int GetPoolDictionary(string ammoTag) {
    return poolDictionary[ammoTag].Count;
  }
}
