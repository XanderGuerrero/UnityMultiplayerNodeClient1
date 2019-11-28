using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CollisionDestory : MonoBehaviour
{
    [SerializeField]
    private NetworkIdentity networkIdentity;
    [SerializeField]
    private WhoActivateMe whoActivatedMe;
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
            CollisionData.id = this.networkIdentity.GetID();
            networkIdentity.GetSocket().Emit("collisionDestroy", new JSONObject(JsonUtility.ToJson(CollisionData)));
        }
        else
        {
            Debug.Log("Obecjt we collided with ID before formatting: " + ni);
            Debug.Log("Obecjt we collided with ID before formatting: " + ni.GetID());
            string nameOfCollisionObj = ni.ToString().Substring(0, ni.ToString().IndexOf('('));


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
            Debug.Log("CollisionData: " + whoActivatedMe.GetActivator());
            Debug.Log("CollisionData: " + ni.GetID());
            if (ni == null || ni.GetID() != whoActivatedMe.GetActivator())
            {
                CollisionData.distance = 0;
                CollisionData.id = this.networkIdentity.GetID();


                networkIdentity.GetSocket().Emit("collisionDestroy", new JSONObject(JsonUtility.ToJson(CollisionData)));
            }
        }
    }
}
