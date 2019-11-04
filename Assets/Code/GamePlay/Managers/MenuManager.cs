﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    private Button QueueButton;

    private SocketIOComponent socketReference;

    private SocketIOComponent SocketReference
    {
        get
        {
            return socketReference = (socketReference == null) ? FindObjectOfType<NetworkClient>() : socketReference;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //womt accept button input
        QueueButton.interactable = false;

        SceneManagementManager.Instance.LoadLevel(levelName: SceneList.ONLINE, (levelName) => {
            QueueButton.interactable = true;
        });
    }

    // Update is called once per frame
    public void OnQueue()
    {
        SocketReference.Emit("joinGame");
    }
}
