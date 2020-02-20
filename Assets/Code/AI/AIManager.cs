using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    [SerializeField]
    private Transform EnemyTransform;

    private Coroutine EnemyRotationCoroutine;
    //[SerializeField]
    //private Transform BarrelTransform;

    //public void SetBarrelRotation(float value)
    //{
    //    BarrelTransform.localEulerAngles = new Vector3(x: 0, y: value , z: 0);
    //}

    public void SetEnemyShipRotation(float valueX, float valueY, float valueZ)
    {
        //StartCoroutine(AnimateEnemyShipTurn(EnemyTransform.localEulerAngles.x, valueX, EnemyTransform.localEulerAngles.y, valueY, EnemyTransform.localEulerAngles.z, valueZ));
        EnemyRotationCoroutine = StartCoroutine(AnimateEnemyShipTurn(EnemyTransform, new Vector3(valueX, valueY, valueZ)));
        EnemyTransform.localEulerAngles = new Vector3(x: valueX, y: valueY, z: valueZ);
    }

    public void StopCoroutines()
    {
        if (EnemyRotationCoroutine != null)
        {
            StopCoroutine(EnemyRotationCoroutine);
        }
    }

    private IEnumerator AnimateEnemyShipTurn(Transform AiTransform, Vector3 goalPosition)
    {
        //float count = 0.1f;//in sync with server update.
        //float currentTime = 0.0f;

        //currentTime += Time.deltaTime;

        //if(currentTime < count)
        //{
        //    float x = Mathf.Lerp(startRotationX, valueX, currentTime / count);
        //    float y = Mathf.Lerp(startRotationY, valueY, currentTime / count);
        //    float z = Mathf.Lerp(startRotationZ, valueZ, currentTime / count);
        //    EnemyTransform.localEulerAngles = new Vector3(x: x, y: x, z: z);
        //}
        float count = 0.01f;//make sure to sync this with the server ai_base.speed
        float currentTime = 0.0f;
        Vector3 startPositiion = AiTransform.localEulerAngles;


        while (currentTime < count)
        {
            currentTime += Time.deltaTime;

            if (currentTime < count)
            {
                AiTransform.localEulerAngles = Vector3.Lerp(startPositiion, goalPosition, currentTime / count);
            }
            yield return new WaitForEndOfFrame();

            if (AiTransform == null)
            {
                currentTime = count;
                yield return null;
            }
        }
        yield return null;
    }
}
