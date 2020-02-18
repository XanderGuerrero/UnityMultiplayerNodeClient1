using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    [SerializeField]
    private Transform EnemyTransform;

    //[SerializeField]
    //private Transform BarrelTransform;

    //public void SetBarrelRotation(float value)
    //{
    //    BarrelTransform.localEulerAngles = new Vector3(x: 0, y: value , z: 0);
    //}

    public void SetEnemyShipRotation(float valueX, float valueY, float valueZ)
    {
        EnemyTransform.localEulerAngles = new Vector3(x: valueX, y: valueY, z: valueZ);
    }
}
