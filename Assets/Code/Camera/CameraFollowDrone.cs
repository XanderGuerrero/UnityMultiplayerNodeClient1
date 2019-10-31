using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowDrone : MonoBehaviour
{

    private Transform cameraTarget;
    //public float smoothSpeed = 0.125f;
    public Vector3 distanceFromCameraTarget;
    private string cID = string.Empty;
    private GameObject player;
    bool firstTime = true;
    private Vector3 velocityCameraFollow;
    public Vector3 behindPosition = new Vector3(0, 2, -4);
    public float angle;
    float xRotation = 0f;
    float yRotation = 0f;
    float zRotation = 0f;
    Rigidbody rb;
    [Header("Data")]
    [SerializeField]
    private float speed = 40;


    void Awake()
    {
 
    }
    // Start is called before the first frame update
    void Update()
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
                    rb = player.GetComponent<Rigidbody>();
                    //this.transform.SetParent(cameraTarget);
                    //this.transform.localPosition = -Vector3.forward * 15 + Vector3.up * 10;
                    //this.transform.localEulerAngles = new Vector3(12.5f, 0, 0);
                    firstTime = false;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 Linputs = InputManager.MainLeftJoystick();
        Vector3 Rinputs = InputManager.MainRightJoystick();

        //Quaternion currentRotation = transform.rotation;
        //if (InputManager.MainRightJoystick() == Vector3.zero)
        //{
        //    transform.rotation = Quaternion.RotateTowards(currentRotation, startRotation, Time.deltaTime * rotateSpeed);
        //}
        //Quaternion.LookRotation(new Vector3(Input.GetAxis("K_MainVertical"), Input.GetAxis("K_MainHorizontal"),0f));

        //this worked somewhat but difficult to control once the rotation has changed
        //transform.Rotate(new Vector3(Input.GetAxis("K_MainVertical"), Input.GetAxis("K_MainHorizontal"), 0f));
        if (player != null)
        {
            xRotation = Rinputs.y;
            //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation = Rinputs.x;
            //moving camera up and down
            transform.position = Vector3.SmoothDamp(transform.position, cameraTarget.TransformPoint(behindPosition) + Vector3.up * Linputs.y, ref velocityCameraFollow, .01f);
            //rotate camera to face players backside
            transform.rotation = Quaternion.Euler(new Vector3(cameraTarget.localEulerAngles.x + angle, cameraTarget.localEulerAngles.y, cameraTarget.localEulerAngles.z));
            // Quaternion.Euler(rb.rotation.x * speed, player.gameObject.transform.localRotation.y * speed, rb.rotation.z);
            //transform.localRotation = Quaternion.Euler(rb.rotation.x, rb.rotation.y, rb.rotation.z);
            //transform.Rotate(new Vector3(0,0,transform.eulerAngles.z), xRotation * speed);
        }
    }
}
