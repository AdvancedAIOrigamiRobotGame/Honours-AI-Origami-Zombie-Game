using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectMultiplier 
{
    private Multiplier Prefab;
    private int Size;
    private List<Multiplier> AvailableObjects;

    private ObjectMultiplier(Multiplier Prefab, int Size)
    {
        this.Prefab = Prefab;
        this.Size = Size;
        AvailableObjects = new List<Multiplier>(Size);
    }

    public static ObjectMultiplier CreateInstance(Multiplier Prefab, int Size)
    {
        ObjectMultiplier pool = new ObjectMultiplier(Prefab, Size);

        GameObject poolGameObject = new GameObject(Prefab + " Pool");
        pool.CreateObjects(poolGameObject);

        return pool;
    }

    private void CreateObjects(GameObject parent)
    {
        for (int i = 0; i < Size; i++)
        {
            Multiplier mObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, parent.transform);
            mObject.Parent = this;
            mObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
        }
    }

    public Multiplier GetObject()
    {
        Multiplier instance = AvailableObjects[0];

        AvailableObjects.RemoveAt(0);

        instance.gameObject.SetActive(true);

        return instance;
    }

    public void ReturnObjectToPool(Multiplier Object)
    {
        AvailableObjects.Add(Object);
    }
}
