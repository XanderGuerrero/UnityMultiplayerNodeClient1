using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltObject : MonoBehaviour
{
    [SerializeField]
    private float orbitSpeed;
    [SerializeField]
    private GameObject parent;
    [SerializeField]
    private bool rotationClokWise;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private Vector3 rotationDirection;


    public void SetupAsteroidBeltObject(float _speed, float _rotationSpeed, GameObject _parent, bool _rotateClockWise)
    {

        orbitSpeed = _speed;
        rotationSpeed = _rotationSpeed;
        parent = _parent;
        rotationClokWise = _rotateClockWise;
        rotationDirection = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }

    private void Update()
    {
        if (rotationClokWise)
        {
            transform.RotateAround(parent.transform.position, parent.transform.up, orbitSpeed * Time.deltaTime * NetworkClient.SERVER_UPDATE_TIME);
        }
        else
        {
            transform.RotateAround(parent.transform.position, parent.transform.up, -orbitSpeed * Time.deltaTime * NetworkClient.SERVER_UPDATE_TIME);
        }

        transform.Rotate(rotationDirection, rotationSpeed * Time.deltaTime * NetworkClient.SERVER_UPDATE_TIME);
    }
}
