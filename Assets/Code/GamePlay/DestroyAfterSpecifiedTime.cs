using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSpecifiedTime : MonoBehaviour
{
    [SerializeField]
    private NetworkIdentity networkIdentity;
    [SerializeField]
    private WhoActivateMe whoActivatedMe;
    public float timeToExplode = 3;
    public GameObject Explosion;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }

    // Update is called once per frame
    IEnumerator wait()
    {
        yield return new WaitForSeconds(timeToExplode);
        GameObject explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
        //Debug.Log("this.networkIdentity.GetID(): " + this.networkIdentity.GetID());
        networkIdentity.GetSocket().Emit("BulletDestory", new JSONObject(JsonUtility.ToJson(new IdData()
        {
        //this bullets id will be sent to server
            id = this.networkIdentity.GetID()

        })));

        Destroy(this.gameObject, 1.5f);
        Destroy(explosion, 1.5f);
    }
}
