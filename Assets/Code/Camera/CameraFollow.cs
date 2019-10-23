
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
                    this.transform.localPosition = -Vector3.forward * 12.3f + Vector3.up * 3.4f;
                    this.transform.localEulerAngles = new Vector3(7f, 0, 0);
                    firstTime = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //transform.localEulerAngles = new Vector3(cameraTarget.transform.localEulerAngles.x, cameraTarget.transform.localEulerAngles.y, cameraTarget.transform.localEulerAngles.z);
        //Vector3 desiredPosition = cameraTarget.position + distanceFromCameraTarget;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        //transform.position = smoothedPosition;
        Vector3 pointToLook = cameraTarget.transform.position;
        var distance = pointToLook.magnitude;
        var direction = pointToLook / distance; // This is now the normalized direction.
        Debug.DrawLine(this.transform.position, cameraTarget.transform.position, Color.red);
        direction.Normalize();
        //transform.LookAt(cameraTarget.transform.position);
        //transform.LookAt(cameraTarget);
    }
}
