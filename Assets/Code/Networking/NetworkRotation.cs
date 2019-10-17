using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkRotation : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Referenced Values")]
    [SerializeField]
    private float oldBarrelRotation;
    private float oldShipTilt;

    [Header("Class References")]
    [SerializeField]
    private PlayerManager playermanager;


    private NetworkIdentity networkIdentity;
    private PlayerRotation player;
    private float stillCounter = 0;

    public void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();

        player = new PlayerRotation();
        player.barrelRotation = 0;
        player.shipTiltRotation = 0;

        if (!networkIdentity.IsControlling())
        {
            enabled = false;

        }



    }



    public void Update()
    {
        if (networkIdentity.IsControlling())
        {
            if (oldBarrelRotation != playermanager.GetLastRotation() || oldShipTilt != transform.localEulerAngles.z)
            {
                oldBarrelRotation = playermanager.GetLastRotation();
                oldShipTilt = transform.localEulerAngles.z;
                stillCounter = 0;
                sendData();
            }
            else {
                stillCounter += Time.deltaTime;
                if (stillCounter >= 1)
                {
                    stillCounter = 0;
                    sendData();
                }
            }
        }
    }


    public void sendData()
    {
        player.shipTiltRotation = transform.localEulerAngles.z.TwoDecimals();
        player.barrelRotation = playermanager.GetLastRotation().TwoDecimals();
        //Debug.Log("send rot data: " + player.barrelRotation);
        networkIdentity.GetSocket().Emit("updateRotation", new JSONObject(JsonUtility.ToJson(player)));
    }

}
