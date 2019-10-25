using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSpecifiedTime : MonoBehaviour
{
    [SerializeField]
    private NetworkIdentity networkIdentity;
    [SerializeField]
    private WhoActivateMe whoActivatedMe;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }

    // Update is called once per frame
    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);

        networkIdentity.GetSocket().Emit("CollisionDestory", new JSONObject(JsonUtility.ToJson(new IdData()
        {
            //this bullets id will be sent to server
            id = networkIdentity.GetID()

        })));

    }
}
