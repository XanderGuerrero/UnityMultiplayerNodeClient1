﻿using System.Collections;
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
    public Vector3 velocityCameraFollow = Vector3.one;
    public Vector3 behindPosition = new Vector3(0, 2, -4);
    public float angle;
    float xRotation = 0f;
    float yRotation = 0f;
    float zRotation = 0f;
    Rigidbody rb;
    [Header("Data")]
    [SerializeField]
    private float speed = 40;
    public float distanceDamp = 0.1f;

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

                if (player != null)
                {
                    cameraTarget = player.transform;
                    rb = player.GetComponent<Rigidbody>();

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

        if (player != null)
        {
            xRotation = Rinputs.y;
            //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation = Rinputs.x;
            //moving camera up and down
            transform.position = Vector3.SmoothDamp(transform.position, /*cameraTarget*/ player.transform.TransformPoint(behindPosition) + player.transform.up * InputManager.MainLeftJoystick().y, ref velocityCameraFollow, distanceDamp);
            //rotate camera to face players backside
            //transform.rotation = Quaternion.Euler(new Vector3(cameraTarget.localEulerAngles.x + angle, player.transform.localEulerAngles.y, player.transform.localEulerAngles.z));
            transform.LookAt(player.transform, player.transform.up);

        }
    }
}
