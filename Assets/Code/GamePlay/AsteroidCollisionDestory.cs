using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCollisionDestory : MonoBehaviour
{
    [SerializeField]
    private NetworkIdentity networkIdentity;
    //[SerializeField]
    //private WhoActivateMe whoActivatedMe;
    private float dist;
    private IdData CollisionData;
    //private string networkIDofCollidedObject;



    // Start is called before the first frame update
    void Start()
    {
        CollisionData = new IdData();
    }



    public void OnCollisionEnter(Collision collision)
    {
        //get the identity of the object we collided with
        NetworkIdentity ni = collision.gameObject.GetComponent<NetworkIdentity>();

        //networkIDofCollidedObject = ni.ToString();
        if (ni == null)
        {
            CollisionData.collisionObjectsNetID = "environment";
        }
        else
        {
            Debug.Log("Obecjt we collided with ID before formatting: " + ni);
            Debug.Log("Obecjt we collided with ID before formatting: " + ni.GetID());
            string nameOfCollisionObj = ni.ToString().Substring(0, ni.ToString().IndexOf('('));
            //Debug.Log("gameobject name: " + this.gameObject.name);
            var name = this.gameObject.name.ToString().Substring(0, ni.ToString().IndexOf('('));
            Debug.Log("gameobject name: " + name);
            if (name == nameOfCollisionObj)
            {
                Debug.Log("Reflect!!!!");
                print("First point that collided: " + collision.contacts[0].point);
                //reflect this gameobject off of the other asteroid
                Vector3 direction = Vector3.Reflect(this.transform.position, collision.contacts[0].point);
                direction.x = direction.x.TwoDecimals();
                direction.y = direction.y.TwoDecimals();
                direction.z = direction.z.TwoDecimals();
                Asteroid newAsteroidDirData = new Asteroid();
                newAsteroidDirData.direction = direction.normalized;//new Vector3(direction.x, direction.y, direction.z); 
                newAsteroidDirData.id = this.networkIdentity.GetID();
                Vector3 newPosition = collision.contacts[0].point;
                //newPosition.x = newPosition.x.TwoDecimals();
                //newPosition.y = newPosition.y.TwoDecimals();
                //newPosition.z = newPosition.z.TwoDecimals();
                newAsteroidDirData.position = newPosition;
                //newAsteroidDirData.position.x = newPosition.x;
                //newAsteroidDirData.position.y = newPosition.y;
                //newAsteroidDirData.position.z = newPosition.z;
                //send new position, new direction, new speed, name and id, tumble value back
                //braodcast to all clients by calling a new event On("updateAstroidmovement")
                Debug.Log("CALL AsteroidUpdateDirection!!!!");
                networkIdentity.GetSocket().Emit("AsteroidUpdateDirection", new JSONObject(JsonUtility.ToJson(newAsteroidDirData)));
            }
            else
            {
                string stringBeforeChar = ni.ToString().Substring(ni.ToString().IndexOf('('), ni.ToString().IndexOf(')'));
                stringBeforeChar = stringBeforeChar.Substring(stringBeforeChar.IndexOf('('), stringBeforeChar.ToString().IndexOf(')'));
                stringBeforeChar = stringBeforeChar.Substring(stringBeforeChar.LastIndexOf('(') + 1);
                CollisionData.collisionObjectsNetID = stringBeforeChar;
                CollisionData.name = nameOfCollisionObj;


                var Dist = collision.gameObject.transform.position - collision.other.gameObject.transform.position;
                Debug.Log("distance: " + Dist);
                Debug.Log("Obecjt we collided with name after formatting: " + CollisionData.name);
                Debug.Log("Obecjt we collided with ID after formatting: " + CollisionData.collisionObjectsNetID);
                //Debug.Log("distance: " + (collision.gameObject.transform.position - collision.other.gameObject.transform.position));
                Debug.Log("CollisionData: " + CollisionData);
                //Debug.Log("distance: " + (collision.gameObject.transform.position - collision.other.gameObject.transform.position));
                //if the ni is empty or the ni id is not the person who shot the bullet
                Debug.Log("CollisionData: " + ni);
                //Debug.Log("CollisionData: " + whoActivatedMe.GetActivator());
                Debug.Log("CollisionData: " + ni.GetID());
                if (ni == null || CollisionData.name != this.gameObject.name)
                {
                    CollisionData.distance = 0;
                    CollisionData.id = this.networkIdentity.GetID();


                    networkIdentity.GetSocket().Emit("collisionDestroy", new JSONObject(JsonUtility.ToJson(CollisionData)));
                }
            }
        }
    }
}
