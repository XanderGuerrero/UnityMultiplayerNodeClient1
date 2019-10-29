using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CollisionDestory : MonoBehaviour
{
    [SerializeField]
    private NetworkIdentity networkIdentity;
    [SerializeField]
    private WhoActivateMe whoActivatedMe;

    public void OnCollisionEnter(Collision collision)
    {
        //get the identity of the object we collided with
        NetworkIdentity ni = collision.gameObject.GetComponent<NetworkIdentity>();
        Debug.Log("distance: " + (collision.gameObject.transform.position - collision.other.gameObject.transform.position));
        //if the ni is empty or the ni id is not the person who shot the bullet
        if (ni == null || ni.GetID() != whoActivatedMe.GetActivator())        {
            networkIdentity.GetSocket().Emit("CollisionDestory", new JSONObject(JsonUtility.ToJson(new IdData()
            {
                //this id will be sent to server
                id = networkIdentity.GetID()

            })));
        }

    }
}
