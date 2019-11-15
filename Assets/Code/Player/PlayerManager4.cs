using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager4 : MonoBehaviour
{

    private float baseSpeed = 1f;
    private float rotSpeedX = 2.0f;
    private float rotSpeedY = 1.5f;
    private float rotateSpeed = 100.5f;
    private Quaternion startRotation;
    private Vector3 force;
    private Rigidbody rb;
    private float lastRotation;
    public GameObject crossHairs;

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
    public float wantedYrotation;
    public float currentYrotation;
    public float wantedXrotation;
    public float currentXrotation;
    public float wantedZrotation;
    public float currentZrotation;
    public float rotateAmount = 2.5f;
    public float rotationYveloctiy = 2.5f;
    public float rotationXveloctiy = 2.5f;
    public float rotationZveloctiy = 2.5f;

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
            movementUpDown();
            checkMovement();
            checkAiming();
            ClampingSpeedValues();
            swerve();
            //checkTilt();
            rb.AddRelativeForce(Vector3.up * upForce);
            rb.rotation = Quaternion.Euler(
                new Vector3(currentXrotation, currentYrotation, -tiltAmountSideways)
                );
        }
    }


    public float sideMovementAmount = 3.0f;
    public float tiltAmountSideways;
    public float tiltAmountVelocity;

    private void swerve()
    {
        ////moving right & moving left
        if (((InputManager.MainLeftJoystick().x > 0.2f) && (InputManager.MainLeftJoystick().x <= 1f)) || ((InputManager.MainLeftJoystick().x < -0.2f) && (InputManager.MainLeftJoystick().x <= -1f)))
        {
            //Debug.Log("foraward: ");
            rb.AddRelativeForce(Vector3.right * InputManager.MainLeftJoystick().x * sideMovementAmount);
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 30 * InputManager.MainLeftJoystick().x, ref tiltAmountVelocity, 0.15f);
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.3f);
        }

    }

    private void movementUpDown()
    {
        //not moving
        if (InputManager.MainLeftJoystick() == new Vector3(0, 0, 0))
        {
            upForce = 0;
        }
        //forward
        if (((InputManager.MainLeftJoystick().y > 0.2f && InputManager.MainLeftJoystick().y <= 1f)) || ((InputManager.MainLeftJoystick().y < -0.2f && InputManager.MainLeftJoystick().y <= -1f)))
        {
            //Debug.Log("foraward: ");
            rb.velocity = rb.velocity;
            upForce = 0;
        }
        ////moving right
        if ((InputManager.MainLeftJoystick().x > 0.2f) && (InputManager.MainLeftJoystick().x <= 1f))
        {
            //Debug.Log("foraward: ");
            upForce = 2000;
        }
        ////moving left
        if ((InputManager.MainLeftJoystick().x < -0.2f) && (InputManager.MainLeftJoystick().x <= -1f))
        {
            //Debug.Log("left: ");
            upForce = 2000;
        }


        //if (InputManager.AButton())
        //{
        //    Debug.Log("PRESSED A");
        //    upForce = 450;
        //    //rb.AddRelativeForce(Vector3.up * upForce);
        //}
        //if (InputManager.BButton())
        //{
        //    upForce = -200;
        //}
        //if (InputManager.YButton())
        //{
        //    upForce = 98.1f;
        //}
        //if (InputManager.XButton())
        //{
        //    upForce = 0;
        //}
    }

    private Vector3 velocityToSmoothDampToZero;
    private void ClampingSpeedValues()
    {
        //forward
        if ((InputManager.MainLeftJoystick().y > 0.2f && InputManager.MainLeftJoystick().y <= 1f))
        {
            //Debug.Log("foraward: ");
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Lerp(rb.velocity.magnitude, 10.0f, Time.deltaTime * 10f));
        }
        //if not moving
        if (InputManager.MainLeftJoystick() == new Vector3(0, 0, 0))
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
        }
        ////moving right
        if ((InputManager.MainLeftJoystick().x > 0.2f) && (InputManager.MainLeftJoystick().x <= 1f))
        {
            //Debug.Log("foraward: ");
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Lerp(rb.velocity.magnitude, 10.0f, Time.deltaTime * 10f));
        }
        ////moving left
        if ((InputManager.MainLeftJoystick().x < -0.2f) && (InputManager.MainLeftJoystick().x <= -1f))
        {
            //Debug.Log("left: ");
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Lerp(rb.velocity.magnitude, 10.0f, Time.deltaTime * 10f));
        }
    }

    private float movementSpeed = 5000f;
    private void checkMovement()
    {
        Vector3 Linputs = InputManager.MainLeftJoystick();


        //moving forward
        if ((InputManager.MainLeftJoystick().y > 0.2f && InputManager.MainLeftJoystick().y <= 1f))
        {
            //Debug.Log("foraward: ");
            rb.AddRelativeForce(Vector3.forward * InputManager.MainLeftJoystick().y * movementSpeed);
        }
        ////moving right
        if ((InputManager.MainLeftJoystick().x > 0.2f) && (InputManager.MainLeftJoystick().x <= 1f))
        {
            //Debug.Log("foraward: ");
            rb.AddRelativeForce(Vector3.right * InputManager.MainLeftJoystick().x * movementSpeed);
            tiltAmountLaterally = Mathf.SmoothDamp(tiltAmountLaterally, 20 * InputManager.MainLeftJoystick().x, ref tiltAmountLaterally, 0.1f);
        }
        ////moving left
        if ((InputManager.MainLeftJoystick().x < -0.2f) && (InputManager.MainLeftJoystick().x <= -1f))
        {
            //Debug.Log("left: ");
            rb.AddRelativeForce(Vector3.right * InputManager.MainLeftJoystick().x * movementSpeed);
            tiltAmountLaterally = Mathf.SmoothDamp(tiltAmountLaterally, 20 * InputManager.MainLeftJoystick().x, ref tiltAmountLaterally, 0.1f);
        }
        ////if moving left and up (diagonal)
        //if (((InputManager.MainLeftJoystick().x < -0.2f) && (InputManager.MainLeftJoystick().x >= -1f)) && ((InputManager.MainLeftJoystick().y > 0.2f && InputManager.MainLeftJoystick().y <= 1f)))
        //{
        //    Debug.Log("diagonally up + left: ");

        //}
        //if ((InputManager.MainLeftJoystick().x > 0.2f) && (InputManager.MainLeftJoystick().x <= 1f) && ((InputManager.MainLeftJoystick().y > 0.2f && InputManager.MainLeftJoystick().y <= 1f)))
        //{

        //}
    }



    private void Awake()
    {
        targetRotation = Quaternion.identity;
    }

    //private void checkTilt()
    //{
    //    Vector3 Linputs = InputManager.MainLeftJoystick();
    //    zRotation = Linputs.x;
    //    tiltAmountLaterally = Mathf.SmoothDamp(tiltAmountLaterally, zRotation, ref tiltAmountLaterally, 0.1f);

    //    transform.Rotate(Vector3.forward, -tiltAmountLaterally);
    //}



    private void checkAiming()
    {

        //if (!screenRect.Contains(Input.mousePosition))
        //    return;


        ////get input
        //Vector3 Linputs = InputManager.MainLeftJoystick();
        Vector3 Rinputs = InputManager.MainRightJoystick();
        Vector3 Linputs = InputManager.MainLeftJoystick();
        zRotation -= Linputs.x;

        xRotation -= Rinputs.y;

        yRotation += Rinputs.x;

        currentXrotation = Mathf.SmoothDamp(currentXrotation, xRotation, ref rotationXveloctiy, 0.15f);

        currentYrotation = Mathf.SmoothDamp(currentYrotation, yRotation, ref rotationYveloctiy, 0.15f);

        //currentZrotation = Mathf.SmoothDamp(currentZrotation, zRotation, ref rotationZveloctiy, 0.15f);


    }


    void OnGUI()
    {
        GUILayout.Label("Forward direction x " + shotSpawnPoint.forward.x);
        GUILayout.Label("Forward direction y " + shotSpawnPoint.forward.y);
        GUILayout.Label("Forward direction z " + shotSpawnPoint.forward.z);
        //GUILayout.Label("LeftJoystick " + InputManager.MainLeftJoystick().ToString());
        //GUILayout.Label("Mathf.Abs(MainLeftJoystick.x) " + Mathf.Abs(InputManager.MainLeftJoystick().x));
        //GUILayout.Label("Mathf.Abs(MainLeftJoystick.y) " + Mathf.Abs(InputManager.MainLeftJoystick().y));
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
            Debug.Log("fire: ");
        }
    }
}
