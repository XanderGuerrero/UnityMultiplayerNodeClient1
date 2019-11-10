using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class NetworkClient : SocketIOComponent
{

    public const float SERVER_UPDATE_TIME = 10;

    public static Action<SocketIOEvent> OnGameStateChange = (E) => { };

    [Header("Network Client")]
    [SerializeField]
    private Transform networkContainer;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private ServerObjects serverSpawnables;
    //Objectpooler objectPooler;


    //same value across all instaniate object of type networkClient
    public static string ClientId { get; private set; }
    private Dictionary<string, NetworkIdentity> serverObjects;

    
    // Start is called before the first frame update
    //override allows us to call this start instead of calling 
    //SocketIOComponent's start method
    public override void Start()
    {
        //calls the start method of the class we inherited from
        base.Start();
        //initialize dictionary of game objects
        initialize();
        setUpEvents();
        //objectPooler = Objectpooler.Instance;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }


    public void initialize()
    {
        serverObjects = new Dictionary<string, NetworkIdentity>();
    }


    public void setUpEvents()
    {
        On("open", (E) =>
        {
            Debug.Log("connection made to the server");
        });


        On("register", (E) =>
        {
            ClientId = E.data["id"].ToString();
            ClientId = ClientId.Trim('"');
            //Debug.LogFormat("Our clients ID ({0})", ClientId);
        });

        //the event 'spawn' or any event name must match what the node server
        //even name is*************
        On("spawn", (E) =>
        {
            //handles all spawning all players
            //passed data
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            GameObject go = Instantiate(playerPrefab, networkContainer);
            go.name = string.Format("Player({0})", id);
            NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
            ni.SetControllerID(id);
            ni.SetScoketReference(this);

            serverObjects.Add(id, ni);
            //Debug.Log("Connection Made! PLAYER: " + id + "has joined the game");
        });

        On("disconnected", (E) =>
        {
            //handles all spawning all players
            //passed data
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            //Debug.LogFormat("player ({0})", id);
            GameObject go = serverObjects[id].gameObject;

            //Debug.LogFormat("player ({0}) has left the game", id);
            Destroy(go);//remove from game
            serverObjects.Remove(id);//remove from dictionary "memory"
        });


        On("updatePosition", (E) =>
        {
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            float x = E.data["position"]["x"].f;
            float y = E.data["position"]["y"].f;
            float z = E.data["position"]["z"].f;

           // Debug.LogFormat("Data back to the client X values: ({0}) ", x);
            NetworkIdentity ni = serverObjects[id];
            ni.transform.position = new Vector3(x, y, z);

        });

        On("updateRotation", (E) =>
        {
            //Debug.Log("Got Data back, ROTATION : ({0}) " + E.data);

            string id = E.data["id"].ToString();
            id = id.Trim('"');

            float barrelRotation = E.data["barrelRotation"].f;
            float shipTilt = E.data["shipTiltRotation"].f;
            float shipTiltX = E.data["shipTiltRotationX"].f;
            float shipTiltY = E.data["shipTiltRotationY"].f;

            //Debug.LogFormat("Data back to the client rotation values: ({0}) ", barrelRotation);
            NetworkIdentity ni = serverObjects[id];

            ni.transform.localEulerAngles = new Vector3(shipTiltX, shipTiltY, shipTilt);
            //ni.GetComponent<PlayerManager>().SetRotation(barrelRotation);

        });

        On("serverSpawn", (E) =>
        {
            //Debug.Log("about to fire1");
            string name = E.data["name"].str;
            name = name.Trim('"');
           
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            //Debug.Log("about to fire2");
            float x = E.data["position"]["x"].f;
            //Debug.Log("X :" + x);
            float y = E.data["position"]["y"].f;
            //Debug.Log("Y :" + y);
            float z = E.data["position"]["z"].f;
            //Debug.Log("Z :" + z);
            Debug.Log("server wants us to spawn a " + name);
            Debug.Log("ID :" + id);

            if (!serverObjects.ContainsKey(id))
            {
                //Debug.Log("about to fire4");
                ServerObjectData sod = serverSpawnables.GetObjectByName(name);
                var spawnedObject = Instantiate(sod.Prefab, networkContainer);
                //objectPooler.SpawnFromPool(name);

                spawnedObject.name = string.Format("{0}({1})", name, id);
                Debug.Log("Spawned " + spawnedObject.name);
                Debug.Log("Position " + new Vector3(x, y, z).ToString());
                spawnedObject.transform.position = new Vector3(x, y, z);
                var ni = spawnedObject.GetComponent<NetworkIdentity>();
                ni.SetControllerID(id);
                ni.SetScoketReference(this);


                //if bullet, apply directions as well
                if (name == "Bullet")
                //if (name == "Bullet(Clone)")
                {
                    //Debug.Log("about to fire5");
                    float directionX = E.data["direction"]["x"].f;
                    float directionY = E.data["direction"]["y"].f;
                    float directionZ = E.data["direction"]["z"].f;
                    string activator = E.data["activator"].ToString();
                    activator = activator.Trim('"');
                    float speed = E.data["speed"].f;
                    Debug.Log("server spawn bullet speed " + speed);

                    float rot = Mathf.Atan2(directionZ, directionX) * Mathf.Rad2Deg;
                    Vector3 currentRotation = new Vector3(0, 0, rot - 90);
                    spawnedObject.transform.rotation = Quaternion.Euler(currentRotation);

                    WhoActivateMe whoActivateMe = spawnedObject.GetComponent<WhoActivateMe>();
                    whoActivateMe.SetActivator ( activator);

                    Projectile projectile = spawnedObject.GetComponent<Projectile>();
                    projectile.Direction = new Vector3(directionX, directionY, directionZ);
                    projectile.Speed = speed;
                }
                serverObjects.Add(id, ni);
            }
        });


        On("serverSpawnExplosion", (E) =>
        {
            //Debug.Log("about to fire1");
            string name = E.data["name"].str;
            name = name.Trim('"');
            //Debug.Log("NAME :" + name);
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            //Debug.Log("about to fire2");
            float x = E.data["position"]["x"].f;
            //Debug.Log("X :" + x);
            float y = E.data["position"]["y"].f;
            //Debug.Log("Y :" + y);
            float z = E.data["position"]["z"].f;
            //Debug.Log("Z :" + z);
            Debug.Log("server wants us to spawn a " + name);
            //float speed = E.data["speed"].f;
            //Debug.Log("server spawn bullet speed " + speed);

            //make sure object is not already spawned into the game
            if (!serverObjects.ContainsKey(id))
            {
                Debug.Log("about to explode!!!!!!!!!!!!!!!!");
                ServerObjectData sod = serverSpawnables.GetObjectByName(name);
                var spawnedObject = Instantiate(sod.Prefab, networkContainer);
                spawnedObject.transform.position = new Vector3(x, y, z);
                var ni = spawnedObject.GetComponent<NetworkIdentity>();
                ni.SetControllerID(id);
                ni.SetScoketReference(this);
                //add the obj to the dictinary of objs
                serverObjects.Add(id, ni);
            }
        });


        On("serverUnspawn", (E) =>
        {
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            NetworkIdentity ni = serverObjects[id];
            serverObjects.Remove(id);
            DestroyImmediate(ni.gameObject);
        });

        On("playerDied", (E) =>
        {
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            NetworkIdentity ni = serverObjects[id];
            ni.gameObject.SetActive(false);
        });

        On("playerRespawn", (E) =>
        {
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            float x = E.data["position"]["x"].f;
            float y = E.data["position"]["y"].f;
            float z = E.data["position"]["z"].f;
            NetworkIdentity ni = serverObjects[id];
            ni.transform.position = new Vector3(x, y, z);
            ni.gameObject.SetActive(true);
        });

        On("loadGame", (E) =>
        {
            Debug.Log(message: "Switching to game");
            SceneManagementManager.Instance.LoadLevel(SceneList.LEVEL, onLevelLoaded: (levelName) =>{
                SceneManagementManager.Instance.UnLoadLevel(SceneList.MAIN_MENU);
            });
        });

        On("lobbyUpdate", (E) => {
            OnGameStateChange.Invoke(E);
        });




    }

    public void AttemptToJoinLobby()
    {
        Emit("joinGame");
    }
}

[Serializable]
public class Player
{
    public string id;
    public Position position;
    public PlayerRotation rotation;
    public PlayerRotation shipTilt;
}

[Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class PlayerRotation
{
    public float barrelRotation;
    public float shipTiltRotation;
    public float shipTiltRotationX;
    public float shipTiltRotationY;
}


[Serializable]
public class BulletData
{
    public string id;
    public string activator;
    public Position position;
    public Position direction;
}


[Serializable]
public class IdData
{
    public string id;
    public float distance;
    public string collisionObjectsNetID;
}

[Serializable]
public class ExplosionData
{
    public string id;
    public string activator;
    public Position position;
    public string bulletActivatorID;
}