using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using Random = UnityEngine.Random;

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
    float tumble;
    public GameObject Planet;

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

        On("updateAsteroid", (E) =>
        {
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            //Debug.LogFormat("Data back to the client ID values: ({0}) ", id);
            float x = E.data["position"]["x"].f;
            float y = E.data["position"]["y"].f;
            float z = E.data["position"]["z"].f;
            float rotationX = E.data["rotationX"].f;
            float rotationY = E.data["rotationY"].f;
            float rotationZ = E.data["rotationZ"].f;
            float directionX = E.data["direction"]["x"].f;
            float directionY = E.data["direction"]["y"].f;
            float directionZ = E.data["direction"]["z"].f;
            //Debug.LogFormat("Data back to the client X values: ({0}) ", x);
            //Debug.LogFormat("Data back to the client Y values: ({0}) ", y);
            //Debug.LogFormat("Data back to the client Z values: ({0}) ", z);
            // Debug.LogFormat("Data back to the client X values: ({0}) ", x);
            NetworkIdentity ni = serverObjects[id];
            ni.transform.position = new Vector3(x, y, z);
            //calculate rotation
            //float rot = Mathf.Atan2(directionZ, directionX) * Mathf.Rad2Deg;
            //Vector3 currentRotation = new Vector3(0, 0, rot - 90);
            //ni.transform.rotation = Quaternion.Euler(currentRotation);

            //WhoActivateMe whoActivateMe = spawnedObject.GetComponent<WhoActivateMe>();
            //whoActivateMe.SetActivator(activator);

            //AsteroidMovement asteroid = ni.GetComponent<AsteroidMovement>();
            //asteroid = ni.GetComponent<AsteroidMovement>();
            //asteroid.Direction = new Vector3(directionX, directionY, directionZ);
            //asteroid.Speed = speed;
            //asteroid.Tumble = tumble;
            //asteroid.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            //asteroid.RotationX = rotationX;
            //asteroid.RotationX = rotationY;
            //asteroid.RotationX = rotationZ;
            //Debug.Log(" updated asteroid Edata: " + E.data);

        });

        On("updatePosition", (E) =>
        {
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            //Debug.LogFormat("Data back to the client ID values: ({0}) ", id);
            float x = E.data["position"]["x"].f;
            float y = E.data["position"]["y"].f;
            float z = E.data["position"]["z"].f;
            //Debug.LogFormat("Data back to the client X values: ({0}) ", x);
            //Debug.LogFormat("Data back to the client Y values: ({0}) ", y);
            //Debug.LogFormat("Data back to the client Z values: ({0}) ", z);
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
           // Debug.Log("barrel rotation value: " + barrelRotation);
            float shipTilt = E.data["shipTiltRotation"].f;
            float shipTiltX = E.data["shipTiltRotationX"].f;
            float shipTiltY = E.data["shipTiltRotationY"].f;

            Debug.LogFormat("Data back to the client rotation values: ({0}) ", barrelRotation);
            NetworkIdentity ni = serverObjects[id];

            ni.transform.localEulerAngles = new Vector3(shipTiltX, shipTiltY, shipTilt);
            //ni.GetComponent<PlayerManager>().SetRotation(barrelRotation);


        });

        On("updateAI_Rotation", (E) =>
        {
            //Debug.Log("Got AI Data back, ROTATION : ({0}) " + E.data);

            string id = E.data["id"].ToString();
            id = id.Trim('"');

            //float barrelRotation = E.data["barrelRotation"].f;
            // Debug.Log("barrel rotation value: " + barrelRotation);
            float shipTilt = E.data["shipTiltRotation"].f;
            float shipTiltX = E.data["shipTiltRotationX"].f;
            float shipTiltY = E.data["shipTiltRotationY"].f;

            //Debug.LogFormat("Data back to the client rotation values: ({0}) ", barrelRotation);
            NetworkIdentity ni = serverObjects[id];
            if (ni.gameObject.activeInHierarchy)
            {
                //ni.transform.position = new Vector3(x, y, z);
                //Debug.Log("ABOUT TO UPDATE THE AI POSITION, DATA : ({0}) " + E.data);
                //StartCoroutine(AIPositionSmoothing(ni.transform, new Vector3(x, y, z)));

                //ni.transform.position = Vector3.Lerp(ni.transform.position, new Vector3(shipTiltX, y, z), 10f * Time.deltaTime);
                //ni.transform.localEulerAngles = new Vector3(shipTiltX, shipTiltY, shipTilt);
                //ni.GetComponent<AIManager>().SetBarrelRotation(barrelRotation);
                //ni.GetComponent<FlockAI>().SetEnemyShipRotation(shipTiltX, shipTiltY, shipTilt);
                //FlockAI AI = ni.GetComponent<FlockAI>();
                //AI.SetEnemyShipRotation(shipTiltX, shipTiltY, shipTilt);

                ni.transform.localEulerAngles = new Vector3(shipTiltX, shipTiltY, shipTilt);

            }
            //ni.transform.localEulerAngles = new Vector3(shipTiltX, shipTiltY, shipTilt);
            //ni.GetComponent<PlayerManager>().SetRotation(barrelRotation);


        });

        On("UpdateAI", (E) =>
        {
            //Debug.Log("Got Data back, ROTATION : ({0}) " + E.data);

            string id = E.data["id"].ToString();
            id = id.Trim('"');
            float x = E.data["position"]["x"].f;
            float y = E.data["position"]["y"].f;
            float z = E.data["position"]["z"].f;
            //spawnedObject.name = string.Format("{0}({1})", name, id);
            //Debug.Log("about to fire5");
            float directionX = E.data["direction"]["x"].f;
            float directionY = E.data["direction"]["y"].f;
            float directionZ = E.data["direction"]["z"].f;
            //string activator = E.data["activator"].ToString();
            //activator = activator.Trim('"');
            float speed = E.data["speed"].f;
            //Debug.Log("server spawn bullet speed " + speed);
            //float speed = E.data["speed"].f;
            //float barrelRotation = E.data["barrelRotation"].f;
            //Debug.Log("barrel rotation value: " + barrelRotation);
            //float shipTilt = E.data["shipTiltRotation"].f;
            //float shipTiltX = E.data["shipTiltRotationX"].f;
            //float shipTiltY = E.data["shipTiltRotationY"].f;
            NetworkIdentity ni = serverObjects[id];
            if (ni.gameObject.activeInHierarchy)
            {
                //Debug.Log("ABOUT TO UPDATE THE AI POSITION, DATA : ({0}) " + E.data);
                //ni.GetComponent<FlockAI>().Position = new Vector3(x, y, z);
                //ni.GetComponent<FlockAI>().Speed = speed;
                //ni.transform.position = new Vector3(x, y, z);
                StartCoroutine(AIPositionSmoothing(ni.transform, new Vector3(x, y, z)));
                //ni.transform.position = Vector3.Lerp(ni.transform.position, new Vector3(x, y, z), 10f * Time.deltaTime);
                //ni.transform.localEulerAngles = new Vector3(shipTiltX, shipTiltY, shipTilt);
                //ni.GetComponent<AIManager>().SetBarrelRotation(barrelRotation);
                //ni.GetComponent<AIManager>().SetEnemyShipRotation(shipTiltX, shipTiltY, shipTilt);
                //calculate rotation
                float rot = Mathf.Atan2(directionX, directionZ) * Mathf.Rad2Deg;
                float pitch = -Mathf.Asin(directionY) * Mathf.Rad2Deg;
                Vector3 currentRotation = new Vector3(pitch, rot, 0);
                //ni.transform.rotation = Quaternion.Euler(currentRotation);

                FlockAI AI = ni.GetComponent<FlockAI>();
                AI.SetEnemyShipRotation(rot, pitch);
                //AI.Direction = new Vector3(directionX, directionY, directionZ);
                //AI.Speed = speed;

                //Debug.Log("AI direction is: " + AI.Direction + " speed: " + speed);
            }









        });

        On("AsteroidRespawn", (E) =>
        {
            string id = E.data["id"].ToString();
            id = id.Trim('"');
            float x = E.data["position"]["x"].f;
            float y = E.data["position"]["y"].f;
            float z = E.data["position"]["z"].f;
            NetworkIdentity ni = serverObjects[id];
            ni.transform.position = new Vector3(x, y, z);

            //Debug.Log("If NAME entered!!!!!!: " + name);
            //Debug.Log("tumble!!!!!!: " + E.data.ToString());
            //ni.name = string.Format("{0}({1})", name, id);
            //Debug.Log("spawnedObject.name: " + spawnedObject.name);
            tumble = E.data["tumble"].f;
            //Debug.Log("tumble!!!!!!: " + E.data.ToString());
            float directionX = E.data["direction"]["x"].f;
            float directionY = E.data["direction"]["y"].f;
            float directionZ = E.data["direction"]["z"].f;
            float speed = E.data["speed"].f;
            float scaleX = E.data["scale"]["x"].f;
            float scaleY = E.data["scale"]["y"].f;
            float scaleZ = E.data["scale"]["z"].f;
            float rotationX = E.data["rotationX"].f;
            float rotationY = E.data["rotationY"].f;
            float rotationZ = E.data["rotationZ"].f;
            //Debug.Log("server spawn asteroid rotation x: " + E.data["rotationX"].f);
            //string activator = E.data["activator"].ToString();
            //activator = activator.Trim('"');
            //float speed = E.data["speed"].f;
            //Debug.Log("speed!!!!!!: " + E.data["speed"].f);

            //calculate rotation
            float rot = Mathf.Atan2(directionZ, directionX) * Mathf.Rad2Deg;
            Vector3 currentRotation = new Vector3(0, 0, rot);
            ni.transform.rotation = Quaternion.Euler(currentRotation);

            AsteroidMovement asteroid = ni.GetComponent<AsteroidMovement>();
            asteroid = ni.GetComponent<AsteroidMovement>();
            asteroid.Direction = new Vector3(directionX, directionY, directionZ);
            asteroid.Speed = speed;
            asteroid.Tumble = tumble;
            asteroid.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            asteroid.RotationX = rotationX;
            asteroid.RotationX = rotationY;
            asteroid.RotationX = rotationZ;
            //Debug.Log("Edata: " + E.data);


            ni.gameObject.SetActive(true);
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
            //Debug.Log("server wants us to spawn a " + name);
            //Debug.Log("ID :" + id);

            if (!serverObjects.ContainsKey(id))
            {
                //Debug.Log("about to fire4");
                ServerObjectData sod = serverSpawnables.GetObjectByName(name);
                var spawnedObject = Instantiate(sod.Prefab, networkContainer);
                //objectPooler.SpawnFromPool(name);

                
                //Debug.Log("Spawned " + spawnedObject.name);
                //Debug.Log("Position " + new Vector3(x, y, z).ToString());
                spawnedObject.transform.position = new Vector3(x, y, z);
                //Debug.Log("Position " + spawnedObject.transform.position.ToString());
                var ni = spawnedObject.GetComponent<NetworkIdentity>();
                ni.SetControllerID(id);
                ni.SetScoketReference(this);

                //Debug.Log("If NAME: " + name);
                //if bullet, apply directions as well
                if (name == "Bullet")
                //if (name == "Bullet(Clone)")
                {
                    spawnedObject.name = string.Format("{0}({1})", name, id);
                    //Debug.Log("about to fire5");
                    float directionX = E.data["direction"]["x"].f;
                    float directionY = E.data["direction"]["y"].f;
                    float directionZ = E.data["direction"]["z"].f;
                    string activator = E.data["activator"].ToString();
                    activator = activator.Trim('"');
                    float speed = E.data["speed"].f;
                    //Debug.Log("server spawn bullet speed " + speed);


                    //calculate rotation
                    float rot = Mathf.Atan2(directionZ, directionX) * Mathf.Rad2Deg;
                    Vector3 currentRotation = new Vector3(0, 0, rot);
                    spawnedObject.transform.rotation = Quaternion.Euler(currentRotation);

                    WhoActivateMe whoActivateMe = spawnedObject.GetComponent<WhoActivateMe>();
                    whoActivateMe.SetActivator ( activator);

                    Projectile projectile = spawnedObject.GetComponent<Projectile>();
                    projectile.Direction = new Vector3(directionX, directionY, directionZ);
                    projectile.Speed = speed;
                }
                //if Asteroid1, apply tumble and  as well
                if (name == "Asteroid1")
                {
                    //Debug.Log("If NAME entered!!!!!!: " + name);
                    //Debug.Log("tumble!!!!!!: " + E.data.ToString());
                    string activator = E.data["activator"].ToString();
                    Random.InitState(10);
                    spawnedObject.name = string.Format("{0}({1})", name, id);

                    spawnedObject.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                    spawnedObject.transform.localScale = new Vector3(E.data["scale"]["z"].f.TwoDecimals(), E.data["scale"]["z"].f.TwoDecimals(), E.data["scale"]["z"].f.TwoDecimals());
                    WhoActivateMe whoActivateMe = spawnedObject.GetComponent<WhoActivateMe>();
                    whoActivateMe.SetActivator(activator);
                    spawnedObject.AddComponent<BeltObject>().SetupAsteroidBeltObject(Random.Range(1, 3f), Random.Range(1, 1), Planet, true);

                }
                if (name == "AI_Base" || name == "ENEMY_AI")
                {
                    spawnedObject.name = string.Format("{0}({1})", name, id);
                }
                if (name == "FLOCK_AI")
                {
                    spawnedObject.name = string.Format("{0}({1})", name, id);
                    spawnedObject.AddComponent<FlockAI>();
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
           // Debug.Log("server wants us to spawn a " + name);
            //float speed = E.data["speed"].f;
            //Debug.Log("server spawn bullet speed " + speed);

            //make sure object is not already spawned into the game
            if (!serverObjects.ContainsKey(id))
            {
                //Debug.Log("about to explode!!!!!!!!!!!!!!!!");
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
            if (ni.GetComponent<AIManager>())
            {
                ni.GetComponent<AIManager>().StopCoroutines();
            }
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


    private IEnumerator AIPositionSmoothing(Transform AiTransform, Vector3 goalPosition)
    {
        float count = 0.1f;//make sure to sync this with the server ai_base.speed
        float currentTime = 0.0f;
        Vector3 startPositiion = AiTransform.position;

        while(currentTime < count)
        {
            currentTime += Time.deltaTime;

            if (currentTime < count)
            {
                AiTransform.position = Vector3.Lerp(startPositiion, goalPosition, currentTime / count);
            }
            yield return new WaitForEndOfFrame();

            if(AiTransform == null)
            {
                currentTime = count;
                yield return null;
            }

        }
        yield return null;
    }
    void OnGUI()
    {
        GUILayout.Label("Tumble: " + tumble);
        GUILayout.Label("Tumble: " + tumble);
        GUILayout.Label("Tumble: " + tumble);
    }
}


[Serializable]
public class Asteroid
{
    public string id;
    public string name;
    public Vector3 position;
    //public PlayerRotation rotation;
    public Vector3 direction;
    //public Vector3 tumble;
    //public Vector3 speed;
    //public float distance;
    //public string collisionObjectsNetID;
    //public float rotationX;
    //public float rotationY;
    //public float rotationZ;
}


[Serializable]
public class Player
{
    public string id;
    public Position position;
    public PlayerRotation rotation;
    public PlayerRotation shipTilt;
}


//to send asteroid data back to the server to use to update the other players screens


[Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class Direction
{
    public float x;
    public float y;
    public float z;
}

//[Serializable]
//public class Rotation
//{
//    public float x;
//    public float y;
//    public float z;
//}

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
    public string name;
    public float distance;
    public string collisionObjectsNetID;
    public Vector3 position;
}

[Serializable]
public class ExplosionData
{
    public string id;
    public string activator;
    public Position position;
    public string bulletActivatorID;
}