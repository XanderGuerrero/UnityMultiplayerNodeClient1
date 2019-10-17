using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkIdentity : MonoBehaviour
{
    [Header("Helpful Values")]
    [SerializeField]
    //[GreyOut]
    private string id;

    [SerializeField]
    //[GreyOut]
    private bool isControlling;

    //so we can send events and position to the server
    private SocketIOComponent socket;


    public void Awake()
    {
        isControlling = false;
    }

    public void SetControllerID(string ID)
    {
        id = ID;
        isControlling = (NetworkClient.ClientId == ID) ? true : false;// check incoming id versus the one saved from the server

    }

    public void SetScoketReference(SocketIOComponent Socket)
    {
        socket = Socket;
    }
    public string GetID()
    {
        return id;
    }

    public bool IsControlling()
    {
        return isControlling;
    }

    public SocketIOComponent GetSocket()
    {
        return socket;
    }

}
