using System.Collections;
using UnityEngine;

public class RandomAsteroidRotator : MonoBehaviour
{
    private Rigidbody rb;
    private float tumble;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(5f, 25).TwoDecimals();
        tumble = Random.Range(0.5f, 5).TwoDecimals();
        Debug.Log("tumble value is :" + tumble);
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere * (tumble);
        rb.velocity = -transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
