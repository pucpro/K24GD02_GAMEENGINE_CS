using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    private Dictionary<GameObject, Queue<GameObject>> pool = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        GameObject obj;
        if (pool[prefab].Count > 0)
        {
            obj = pool[prefab].Dequeue();
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, pos, rot);
        }

        // Reset Rigidbody
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        return obj;
    }

    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);

        foreach (var kv in pool)
        {
            if (obj.name.StartsWith(kv.Key.name))
            {
                kv.Value.Enqueue(obj);
                return;
            }
        }

        Destroy(obj);
    }
}