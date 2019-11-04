using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager3 : MonoBehaviour
{

    private float baseSpeed = 1f;
    private float rotSpeedX = 2.0f;
    private float rotSpeedY = 1.5f;
    private float rotateSpeed = 100.5f;
    private Quaternion startRotation;
    //private float speed = 40;
    private Vector3 force;
    private Rigidbody rb;
    //private float tilt = 10;




    private float lastRotation;
    public GameObject crossHairs;
    //[Header("Object References")]
    //[SerializeField]
    //private Transform barrelPivot;
    private Vector3 target;

    //shooting
    private BulletData bulletData;
    private CoolDown shootingCoolDown;

    [Header("Data")]
    [SerializeField]
    private float speed = 5;

    [Header("Class References")]
    [SerializeField]
    private NetworkIdentity networkIdentity;
    Vector2 MD;

    private Camera mainCamera;
    float horizontalSpeed = 2.0f;
    float verticalSpeed = 2.0f;
    Rect screenRect;

    [SerializeField]
    private Transform shotSpawnPoint;

    private CharacterController controller;
    private float tiltAmountLaterally = 0;
    public float tilt;
    private Quaternion targetRotation;
    private Vector3 targetPosition;
    float xRotation = 0f;
    float yRotation = 0f;
    float zRotation = 0f;
    float oldmoveX;
    public float upForce;
    public float downForce;
    public float forwardForce;
    public float leftForce;
    public float rightForce;

    void Start()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        mainCamera = FindObjectOfType<Camera>();
        shootingCoolDown = new CoolDown(.15f);
        bulletData = new BulletData();
        bulletData.position = new Position();
        bulletData.direction = new Position();
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        if (!screenRect.Contains(Input.mousePosition))
            return;
        if (networkIdentity.IsControlling())
        {
            checkShooting();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //if (!screenRect.Contains(Input.mousePosition))
        //    return;
        if (networkIdentity.IsControlling())
        {
            checkMovement();
            checkAiming();
            checkTilt();
            rb.AddRelativeForce(Vector3.up * upForce);
        }
    }

    //access to get the last rotation
    //public float GetLastRotation()
    //{
    //    return lastRotation;
    //}

    //not our player but setting their rotations on our side
    //public void SetRotation(float value)
    //{
    //    transform.rotation = Quaternion.Euler(0, 0, value);
    //}

    private float movementSpeed = 40f;
    private void checkMovement()
    {
        Vector3 Linputs = InputManager.MainLeftJoystick();


        //Vector3 move = transform.right * Linputs.x + transform.forward * Linputs.y * speed;

        //not moving
        if ((InputManager.MainLeftJoystick().x < 0.2f) && (InputManager.MainLeftJoystick().x > -0.2f) && (InputManager.MainLeftJoystick().y < 0.2f && InputManager.MainLeftJoystick().y > -0.2f))
        {
            //Debug.Log("not moving: ");
            upForce = 0f;
            //rb.AddRelativeForce(Vector3.back,ForceMode.Force);
        }
        //moving forward
        if ((InputManager.MainLeftJoystick().y > 0.2f && InputManager.MainLeftJoystick().y <= 1f))
        {
            //Debug.Log("foraward: ");

           //rb.AddRelativeForce(Vector3.forward * InputManager.MainLeftJoystick().y * movementSpeed);
            rb.velocity = (rb.transform.forward * InputManager.MainLeftJoystick().y * movementSpeed);
            //Debug.Log("forawardvelocity: " + rb.velocity);
        }
        //moving right
        if ((InputManager.MainLeftJoystick().x > 0.2f) && (InputManager.MainLeftJoystick().x <= 1f))
        {
            Debug.Log("right: ");
            //Vector3 move = new Vector3(rb.position.x, rb.position.y - rb.position.y, rb.position.z);
            rb.velocity = (rb.transform.right * InputManager.MainLeftJoystick().x * movementSpeed);
            //Vector3 vel = rb.velocity;
            //vel.y = 0f;
            //rb.velocity = vel;
            //Debug.Log("rightvelocity: " + rb.velocity);
        }
        //moving left
        if ((InputManager.MainLeftJoystick().x < -0.2f) && (InputManager.MainLeftJoystick().x <= -1f))
        {
            Debug.Log("left: ");
            rb.velocity = (rb.transform.right * InputManager.MainLeftJoystick().x * movementSpeed);
            //Vector3 vel = rb.velocity;
            //vel.y = 0f;
            //rb.velocity = vel;
            //Debug.Log("leftvelocity: " + rb.velocity);
        }
        //if moving left and up (diagonal)
        if (((InputManager.MainLeftJoystick().x < -0.2f) && (InputManager.MainLeftJoystick().x >= -1f)) && ((InputManager.MainLeftJoystick().y > 0.2f && InputManager.MainLeftJoystick().y <= 1f)))
        {
            Debug.Log("diagonally up + left: ");

            rb.velocity = (rb.transform.right * InputManager.MainLeftJoystick().x * movementSpeed) + (rb.transform.forward * InputManager.MainLeftJoystick().y * movementSpeed);
            //rb.velocity = (rb.transform.forward * InputManager.MainLeftJoystick().y * movementSpeed);
            Vector3 vel = rb.velocity;
            vel.y = 0f;
            rb.velocity = vel;
        }
        if ((InputManager.MainLeftJoystick().x > 0.2f) && (InputManager.MainLeftJoystick().x <= 1f) && ((InputManager.MainLeftJoystick().y > 0.2f && InputManager.MainLeftJoystick().y <= 1f)))
        {
            Debug.Log("diagonally up + right: ");
            //Debug.Log("right: ");
            //Vector3 move = new Vector3(rb.porightsition.x, rb.position.y - rb.position.y, rb.position.z);
            rb.velocity = (rb.transform.right * InputManager.MainLeftJoystick().x * movementSpeed) + (rb.transform.forward * InputManager.MainLeftJoystick().y * movementSpeed);
            //rb.velocity = (rb.transform.forward * InputManager.MainLeftJoystick().y * movementSpeed);
            Vector3 vel = rb.velocity;
            vel.y = 0f;
            rb.velocity = vel;
            //Debug.Log("rightvelocity: " + rb.velocity);
        }
    }



    private void Awake()
    {
        targetRotation = Quaternion.identity;
    }

    private void checkTilt()
    {
        Vector3 Linputs = InputManager.MainLeftJoystick();
        zRotation = Linputs.x;
        tiltAmountLaterally = Mathf.SmoothDamp(tiltAmountLaterally, 90 * zRotation, ref tiltAmountLaterally, 0.1f);

        transform.Rotate(Vector3.forward, -tiltAmountLaterally);
    }



    private void checkAiming()
    {

        if (!screenRect.Contains(Input.mousePosition))
            return;


        //get input
        Vector3 Linputs = InputManager.MainLeftJoystick();
        Vector3 Rinputs = InputManager.MainRightJoystick();


        xRotation -= Rinputs.y;

        yRotation += Rinputs.x;



        transform.localRotation = Quaternion.Euler(xRotation * Time.deltaTime * 100, yRotation * 100f * Time.deltaTime, rb.rotation.z);
    }


    void OnGUI()
    {
        GUILayout.Label("RightJoystick " + InputManager.MainRightJoystick().ToString());
        GUILayout.Label("LeftJoystick " + InputManager.MainLeftJoystick().ToString());
        GUILayout.Label("Mathf.Abs(Linputs.x) " + Mathf.Abs(InputManager.MainLeftJoystick().x));
        GUILayout.Label("Mathf.Abs(Linputs.y) " + Mathf.Abs(InputManager.MainLeftJoystick().y));
    }


    private void checkShooting()
    {


        shootingCoolDown.CooldownUpdate();
        if ((InputManager.RightTrigger() == 1) && !shootingCoolDown.IsOnCoolDown())
        {
            shootingCoolDown.StartCoolDown();
            //define bullet
            bulletData.activator = NetworkClient.ClientId;
            bulletData.position.x = shotSpawnPoint.position.x.TwoDecimals();
            bulletData.position.y = shotSpawnPoint.position.y.TwoDecimals();
            bulletData.position.z = shotSpawnPoint.position.z.TwoDecimals();
            bulletData.direction.x = shotSpawnPoint.forward.x;
            bulletData.direction.y = shotSpawnPoint.forward.y;
            bulletData.direction.z = shotSpawnPoint.forward.z;
            //send the bullet
            networkIdentity.GetSocket().Emit("fireBullet", new JSONObject(JsonUtility.ToJson(bulletData)));

        }
    }
}
