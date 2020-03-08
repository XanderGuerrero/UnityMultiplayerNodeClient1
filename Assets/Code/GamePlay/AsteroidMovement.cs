using System.Collections;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float tumble;
    public Vector3 direction;
    public float speed;
    public float RotationX;
    public float RotationY;
    public float RotationZ;

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

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = direction * speed * NetworkClient.SERVER_UPDATE_TIME * Time.deltaTime;
        transform.position += new Vector3(pos.x, pos.y, pos.z);
        transform.Rotate(new Vector3(RotationX, RotationY, RotationZ) * Time.deltaTime * NetworkClient.SERVER_UPDATE_TIME );
    }
}
