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
        Debug.Log("Obecjt we collided with ID before formatting: " + ni);
        //networkIDofCollidedObject = ni.ToString();
        if (ni == null)
        {
            CollisionData.collisionObjectsNetID = "environment";
        }
        else
        {
            string stringBeforeChar = ni.ToString().Substring(ni.ToString().IndexOf('('), ni.ToString().IndexOf(')'));
            stringBeforeChar =  stringBeforeChar.Substring(stringBeforeChar.IndexOf('('), stringBeforeChar.ToString().IndexOf(')'));
            stringBeforeChar = stringBeforeChar.Substring(stringBeforeChar.LastIndexOf('(') + 1);
            CollisionData.collisionObjectsNetID = stringBeforeChar;
        }



        var Dist = collision.gameObject.transform.position - collision.other.gameObject.transform.position;
        Debug.Log("distance: " + Dist);

        Debug.Log("Obecjt we collided with ID after formatting: " + CollisionData.collisionObjectsNetID);
        Debug.Log("distance: " + (collision.gameObject.transform.position - collision.other.gameObject.transform.position));
        
        //Debug.Log("distance: " + (collision.gameObject.transform.position - collision.other.gameObject.transform.position));
        //if the ni is empty or the ni id is not the person who shot the bullet
        if (ni == null || ni.GetID() != whoActivatedMe.GetActivator())
        {
            CollisionData.distance = 0;
            CollisionData.id = this.networkIdentity.GetID();


            networkIdentity.GetSocket().Emit("collisionDestroy", new JSONObject(JsonUtility.ToJson(CollisionData)));
        }

    }
}
