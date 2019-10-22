
using UnityEngine;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{

    public Transform cameraTarget;
    public float smoothSpeed = 0.125f;
    public Vector3 distanceFromCameraTarget;
    private string cID = string.Empty;
    private GameObject player;
    bool firstTime = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //// Update is called once per frame
    //void FixedUpdate()
    //{
       
    //}

    private void FixedUpdate()
    {
        if(firstTime)
        {
            if (NetworkClient.ClientId != string.Empty)
            {
                player = GameObject.Find("/[Server Spawned Object]/Player(" + NetworkClient.ClientId + ")");
                //Debug.Log("NetworkClient.ClientId:" + NetworkClient.ClientId);
                //Debug.Log("CAMERA:" + player.name);
                cameraTarget = player.transform;
                firstTime = false;
            }


            //GameObject[] firstList = GameObject.FindObjectsOfType<GameObject>();
            //List<Object> finalList = new List<Object>();

            //for (var i = 0; i < firstList.Length; i++)
            //{
            //    Debug.Log("in start method :" + cID);
            //    Debug.Log("gameobject name: " + firstList[i].gameObject.name);
            //    if (firstList[i].gameObject.name == "[Server Spawned Object]"/*string.Format("Player({0})", cID)*/)
            //    {
            //        Debug.Log("GameObjectName: 2___ " + firstList[i].gameObject.name);
            //        target = firstList[i].gameObject.get;
            //        //target = gameObject.transform;
            //    }
            //}
        }
        Vector3 desiredPosition = cameraTarget.position + distanceFromCameraTarget;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        transform.LookAt(cameraTarget);
    }
}
