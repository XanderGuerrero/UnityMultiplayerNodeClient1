using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectpooler : MonoBehaviour
{
    [Header("Network Client")]
    [SerializeField]
    private Transform networkContainer;
    [SerializeField]
    public List<ServerObjectData> Objects;
    [SerializeField]
    private ServerObjects serverSpawnables;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    #region Singleton
    public static Objectpooler Instance;

    public void Awake()
    {
        Instance = this;
    }
    #endregion 


    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (ServerObjectData Objs in serverSpawnables.Objects)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < Objs.Count; i++)
            {
                ServerObjectData sod = serverSpawnables.GetObjectByName(Objs.Name);
                GameObject spawnedObject = Instantiate(sod.Prefab, networkContainer);
                spawnedObject.SetActive(false);
                //store the vullet in this Queue until called upon
                objectPool.Enqueue(spawnedObject);
            }

            poolDictionary.Add(Objs.Name, objectPool);

        }
    }

    // Update is called once per frame
    public GameObject SpawnFromPool(string Name/*, Vector3 position, Quaternion rotation*/)
    {
        if (!poolDictionary.ContainsKey(Name))
        {
            return null;
        }

        //get the gameobject
        GameObject objectToSpawn = poolDictionary[Name].Dequeue();
        //set the object to active
        objectToSpawn.SetActive(true);
        //set its properties like id name position
        //objectToSpawn.transform.position = position;
        //objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if(pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }


        //add back to queue so it can be used again later
        poolDictionary[Name].Enqueue(objectToSpawn);

        //return the object for further modification
        return objectToSpawn;
    }
}
