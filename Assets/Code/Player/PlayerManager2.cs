using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager2 : MonoBehaviour
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
        if (!screenRect.Contains(Input.mousePosition))
            return;
        if (networkIdentity.IsControlling())
        {
            checkMovement();
            checkAiming();
            checkTilt();
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


    private void checkMovement()
    {
        Vector3 Linputs = InputManager.MainLeftJoystick();


        Vector3 move = transform.right * Linputs.x + transform.forward * Linputs.y * speed;

        //move left or right with no y movement
        if ((Mathf.Abs(Linputs.x) > 0.2f || Mathf.Abs(Linputs.x) < 0.2f) && (Mathf.Abs(Linputs.y) < 0.2f))
        {
            //Debug.Log("pushing laterally: " + Mathf.Abs(Linputs.x) + "pushing up: " + Linputs.y);
            move = new Vector3(move.x * 1.5f, move.y - move.y, move.z);
            rb.MovePosition(transform.position + move);

        }
        //move left or right with no y movement
        if ((Mathf.Abs(Linputs.x) > 0.2f || Mathf.Abs(Linputs.x) < 0.2f) && Linputs.y == 0f)
        {
            //Debug.Log("pushing laterally: " + Mathf.Abs(Linputs.x) + "pushing up: " + Linputs.y);
            move = new Vector3(move.x * 1.5f, move.y - move.y, move.z);
            rb.MovePosition(transform.position + move);

        }
        //move diagonally up
        if ((Mathf.Abs(Linputs.x) > 0.2f && (Linputs.y) > 0.2f))
        {
            //Debug.Log("pushing right: " + Mathf.Abs(Linputs.x) + "pushing up: " + Linputs.y);
            move = new Vector3(move.x, move.y - move.y, move.z);
            rb.MovePosition(transform.position + move);

        }
        //move diagonally down
        if ((Mathf.Abs(Linputs.x) > 0.2f && (Linputs.y) < -0.2f))
        {
            //Debug.Log("pushing right: " + Mathf.Abs(Linputs.x) + "pushing udown: " + Linputs.y);
            move = new Vector3(move.x, move.y - move.y, move.z);
            rb.MovePosition(transform.position + move);

        }
        //move forward
        if ((Mathf.Abs(Linputs.x) < 0.2f && Mathf.Abs(Linputs.y) > 0.2f))
        {
            rb.MovePosition(transform.position + move);
        }
        //rb.MovePosition(transform.position + move);


    }



    private void Awake()
    {
        targetRotation = Quaternion.identity;
    }

    private void checkTilt()
    {
        Vector3 Linputs = InputManager.MainLeftJoystick();
        zRotation = Linputs.x;
        tiltAmountLaterally = Mathf.SmoothDamp(tiltAmountLaterally, 40 * zRotation, ref tiltAmountLaterally, 0.1f);

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
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += Rinputs.x;
        //zRotation += Linputs.x;
        xRotation = Mathf.Clamp(xRotation, -80, 80);


        transform.localRotation = Quaternion.Euler(xRotation * Time.deltaTime * 100, yRotation * 100f * Time.deltaTime, rb.rotation.z);


        //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        //pitch and yaw code
        //Vector3 moveVector = transform.forward * baseSpeed;

        ////moveVector.z = rb.velocity.x * -tilt;
        ////Debug.Log(moveVector);
        //Vector3 yaw = Rinputs.x * transform.right * rotSpeedX * Time.deltaTime;
        //Vector3 pitch = Rinputs.y * transform.up * rotSpeedY * Time.deltaTime;
        //Vector3 dir = yaw + pitch;
        ////dont let player go too far up or down, add the direction to move vector
        //float MaxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x;
        ////Quaternion.LookRotation(moveVector + dir).eulerAngles.x;
        ////if (MaxX < 90 && MaxX > 70 || MaxX > 270 && MaxX < 290)
        ////{


        ////}
        ////else
        ////{
        //    moveVector += dir;

        //    transform.rotation = Quaternion.LookRotation(moveVector);
        //transform.Translate(moveVector);
        //}
        //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        //transform.rotation *= Quaternion.Euler(pitchDelta * Time.deltaTime, yawDelta * Time.deltaTime, rollDelta * Time.deltaTime);
    }


    //void OnGUI()
    //{
    //    GUILayout.Label("RightJoystick " + InputManager.MainRightJoystick().ToString());
    //    GUILayout.Label("LeftJoystick " + InputManager.MainLeftJoystick().ToString());
    //    GUILayout.Label("Force " + force);
    //}


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
