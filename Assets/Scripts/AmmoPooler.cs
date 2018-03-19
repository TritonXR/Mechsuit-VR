using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPooler : MonoBehaviour
{
  [System.Serializable]
  public class Pool
  {
    public string tag;
    public GameObject prefab;
    public int size;
  }

  [SerializeField]
  public List<Pool> pools;
  public Dictionary<string, Queue<GameObject>> poolDictionary;

	void Awake ()
  {
    //Instance = this;
	}

  // Use this for initialization
  void Start ()
  {
    poolDictionary = new Dictionary<string, Queue<GameObject>>();

    foreach (Pool pool in pools)
    {
      Queue<GameObject> objPool = new Queue<GameObject>();

      for (int i = 0; i < pool.size; ++i)
      {
        GameObject obj = Instantiate(pool.prefab);
        obj.SetActive(false);
        objPool.Enqueue(obj);
      }
      poolDictionary.Add(pool.tag, objPool);
    }
	}

  public GameObject Fire(string ammoTag, Vector3 weaponPosition, Quaternion weaponRotation)
  {
    if (!poolDictionary.ContainsKey(ammoTag)) {
      Debug.Log("Projectile " + ammoTag + " not found");
      Debug.Log("Check spelling of tag");
      return null;
    }

    Debug.Log("Successful fire");

    GameObject projectile = poolDictionary[ammoTag].Dequeue();

    projectile.SetActive(true);
    projectile.transform.position = weaponPosition;
    projectile.transform.rotation = weaponRotation;

    I_Ammo pooledProjectile = projectile.GetComponent<I_Ammo>();

    if (pooledProjectile != null) {
      Debug.Log("Projectile I_Ammo obtained");
      pooledProjectile.OnObjectSpawn();
    }

    poolDictionary[ammoTag].Enqueue(projectile);

    return projectile;
  }
}
