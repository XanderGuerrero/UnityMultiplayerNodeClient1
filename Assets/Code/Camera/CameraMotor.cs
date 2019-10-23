using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform LookAt;
    public float boundX = 1000.0f;
    public float boundY = 1000.5f;


    public Transform cameraTarget;
    public float smoothSpeed = 0.125f;
    public Vector3 distanceFromCameraTarget;
    private string cID = string.Empty;
    private GameObject player;
    bool firstTime = true;

    private void Update()
    {
        if (firstTime)
        {
            if (NetworkClient.ClientId != string.Empty)
            {
                player = GameObject.Find("/[Server Spawned Object]/Player(" + NetworkClient.ClientId + ")");
                //Debug.Log("NetworkClient.ClientId:" + NetworkClient.ClientId);
                //Debug.Log("CAMERA:" + player.name);
                if (player != null)
                {
                    cameraTarget = player.transform;
                    this.transform.SetParent(cameraTarget);
                    this.transform.localPosition = -Vector3.forward * 15 + Vector3.up * 10;
                    this.transform.localEulerAngles = new Vector3(12.5f, 0, 0);
                    firstTime = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;

        float dx = cameraTarget.position.x - transform.position.x;
        float dy = cameraTarget.position.y - transform.position.y;
        //x axis
        if (dx > boundX || dx < -boundX)
        {
            if (transform.position.x < cameraTarget.position.x)
            {
                delta.x = dx - boundX;

            }
            else
            {
                delta.x = dx + boundX;
            }
        }
        //y axis
        if (dy > boundY || dy < -boundY)
        {
            if (transform.position.y < cameraTarget.position.y)
            {
                delta.y = dy - boundY;

            }
            else
            {
                delta.y = dy + boundY;
            }
        }

        //move the camera
        transform.position = transform.position = delta;
    }

}
