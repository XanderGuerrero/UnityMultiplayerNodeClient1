using System.Collections;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float tumble;
    public Vector3 direction;
    public float speed;

    public Vector3 Direction
    {
        get
        {
            return new Vector3(direction.x, direction.y, direction.z);
        }
        set
        {
            direction = value;
        }
    }
    public float Tumble
    {
        get
        {
            return this.tumble;
        }
        set
        {
            tumble = value;
        }
    }

    public float Speed
    {
        get
        {
            return this.speed;
        }
        set
        {
            speed = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //speed = Random.Range(5f, 25).TwoDecimals();
        ///tumble = Random.Range(0.5f, 5).TwoDecimals();
        //Debug.Log("tumble value is :" + tumble);
        rb = GetComponent<Rigidbody>();
        //rb.angularVelocity = Random.insideUnitSphere * (tumble);
        //rb.velocity = direction * speed * NetworkClient.SERVER_UPDATE_TIME * Time.deltaTime;
        //Vector3 pos = direction * speed * NetworkClient.SERVER_UPDATE_TIME * Time.deltaTime;
        //transform.position += new Vector3(pos.x, pos.y, pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = direction * speed * NetworkClient.SERVER_UPDATE_TIME * Time.deltaTime;
        transform.position += new Vector3(pos.x, pos.y, pos.z);
    }
}
