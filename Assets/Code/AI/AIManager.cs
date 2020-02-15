using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    [SerializeField]
    private Transform EnemyTransform;

    [SerializeField]
    private Transform BarrelTransform;

    public void SetBarrelRotation(float value)
    {
        BarrelTransform.localEulerAngles = new Vector3(x: 0, y: value + 90, z: 0);
    }

}
