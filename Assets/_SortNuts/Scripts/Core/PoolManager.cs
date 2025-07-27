using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public static PoolManager Instance { get { return instance; } }

    public GameObject nutPrefab;
    public GameObject boltPrefab;

    private Queue<GameObject> nutPool = new Queue<GameObject>();
    private Queue<GameObject> boltPool = new Queue<GameObject>();

    public int nutPoolSize = 50;
    public int boltPoolSize = 10;
    
    public Transform boardParent;          // Object chứa các Bolt (kéo Board vào Inspector)

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        boardParent = GameObject.Find("Board").transform;
        InitPool(boltPrefab, boltPool, boltPoolSize);
        InitPool(nutPrefab, nutPool, nutPoolSize);
    }

    private void InitPool(GameObject prefab, Queue<GameObject> pool, int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, boardParent);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetNut()
    {
        if (nutPool.Count > 0)
        {
            var nut = nutPool.Dequeue();
            nut.SetActive(true);
            return nut;
        }
        else
        {
            // Tùy bạn, có thể instantiate mới nếu cần
            var nut = Instantiate(nutPrefab, boardParent);
            return nut;
        }
    }

    public void ReturnNut(GameObject nut)
    {
        nut.SetActive(false);
        nutPool.Enqueue(nut);
        nut.transform.SetParent(boardParent);
    }

    public GameObject GetBolt()
    {
        if (boltPool.Count > 0)
        {
            var bolt = boltPool.Dequeue();
            bolt.SetActive(true);
            return bolt;
        }
        else
        {
            var bolt = Instantiate(boltPrefab, boardParent);
            return bolt;
        }
    }

    public void ReturnBolt(GameObject bolt)
    {
        bolt.SetActive(false);
        boltPool.Enqueue(bolt);
    }
}