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
    }
	}

  public GameObject Fire(string tag, Vector3 position, Quaternion rotation)
  {
    if (!poolDictionary.ContainsKey(tag)) {
      Debug.LogWarning("Projectile " + tag + " not found");
      Debug.LogWarning("Check spelling of tag");
      return null;
    }

    Debug.Log("Successful fire");

    GameObject projectile = poolDictionary[tag].Dequeue();

    projectile.SetActive(true);
    projectile.transform.position = position;
    projectile.transform.rotation = rotation;

    I_Ammo pooledProjectile = projectile.GetComponent<I_Ammo>();

    if (pooledProjectile != null) {
      Debug.Log("Projectile I_Ammo obtained");
      pooledProjectile.OnObjectSpawn();
    }

    poolDictionary[tag].Enqueue(projectile);

    return projectile;
  }
}
