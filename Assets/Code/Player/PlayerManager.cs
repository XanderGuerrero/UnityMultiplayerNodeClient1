using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
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
    private float speed = 40;

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
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        //rb.MovePosition(new Vector3(Linputs.x, 0.0f, Linputs.y) * speed * Time.deltaTime);
        //rb.MovePosition(transform.position + transform.forward * Time.deltaTime);





        //rb.MovePosition(transform.position + (transform.right * Linputs.x + transform.forward * Mathf.Clamp( Linputs.y, 0, 1)) * 1 * speed);

        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Vector3 move = transform.right * Linputs.x + transform.forward * Linputs.y * 3;

        //move left or right with no y movement
        if((Mathf.Abs(Linputs.x) > 0.2f || Mathf.Abs(Linputs.x) < 0.2f) && (Mathf.Abs(Linputs.y) < 0.2f))
        {
            Debug.Log("pushing laterally: " + Mathf.Abs(Linputs.x) + "pushing up: " + Linputs.y);
            move = new Vector3(move.x*1.5f, move.y - move.y, move.z);
            rb.MovePosition(transform.position + move);

        }
        //move left or right with no y movement
        if ((Mathf.Abs(Linputs.x) > 0.2f || Mathf.Abs(Linputs.x) < 0.2f) && Linputs.y == 0f)
        {
            Debug.Log("pushing laterally: " + Mathf.Abs(Linputs.x) + "pushing up: " + Linputs.y);
            move = new Vector3(move.x*1.5f, move.y - move.y, move.z);
            rb.MovePosition(transform.position + move);

        }
        //move diagonally up
        if ((Mathf.Abs(Linputs.x) > 0.2f && (Linputs.y) > 0.2f))
        {
            Debug.Log("pushing right: " + Mathf.Abs(Linputs.x) + "pushing up: " + Linputs.y);
            move = new Vector3(move.x, move.y - move.y, move.z);
            rb.MovePosition(transform.position + move);

        }
        //move diagonally down
        if ((Mathf.Abs(Linputs.x) > 0.2f && (Linputs.y) < -0.2f))
        {
            Debug.Log("pushing right: " + Mathf.Abs(Linputs.x) + "pushing udown: " + Linputs.y);
            move = new Vector3(move.x, move.y - move.y, move.z);
            rb.MovePosition(transform.position + move);

        }
        //move forward
        if ((Mathf.Abs(Linputs.x) < 0.2f && Mathf.Abs(Linputs.y) > 0.2f))
        {
            rb.MovePosition(transform.position + move);
        }
            ////if the user is pressing left or right
            //if (move.x > .6f || move.x <-.6f)
            //{
            //    //tilt the ship without moving in the y direction
            //    move = new Vector3(move.x, transform., move.z);
   
        //}
        //rb.MovePosition(transform.position + move);
        //Debug.Log("x : " + move.x);
        //Debug.Log("x : " + move.x);
        //transform.localRotation = Quaternion.Euler(xRotation * speed, yRotation * speed, 0f);
        //move.x = 0;
        //Debug.Log("Linputs : " + Mathf.Abs(Linputs.x));
        // rb.rotation = Quaternion.Euler(15f, 0.0f, 0f);

        //if movement is left or right, tilt the ship

        //rb.rotation = Quaternion.Euler(0.0f, 0.0f,-tilt);
        //Debug.Log
        //   ("about to move");
        //movement in 3d is x and z 

        //Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
        //rigidbody.velocity = movement * speed;
        //rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tilt);


        //this is the good code
        //transform.position += new Vector3(horizontal, 0.0f, vertical) * speed * Time.deltaTime;

        //this is experiment

        //forward
        //Vector3 moveVector = transform.forward * 10;

        ////gather players input
        //Vector3 inputs = InputManager.MainJoystick();
        //Debug.Log(inputs);
        //Vector3 yaw = inputs.x * transform.right * 300.0f * Time.deltaTime;
        //Vector3 pitch = inputs.y * transform.up * 100.5f * Time.deltaTime;
        //Vector3 dir = yaw + pitch;

        //float maxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x;

        //if (maxX < 90 && maxX > 70 || maxX > 270 && maxX < 290)
        //{

        //    moveVector += dir;
        //    transform.rotation = Quaternion.LookRotation(moveVector);
        //}
        //else
        //{

        //    moveVector += dir;
        //    transform.rotation = Quaternion.LookRotation(moveVector);

        //}
        //transform.position += moveVector * Time.deltaTime;



        //controller.Move(moveVector * Time.deltaTime);
        //transform.position += transform.forward * vertical * speed * Time.deltaTime;
        //transform.position += transform.right * horizontal * speed * Time.deltaTime;

        //transform.Rotate(new Vector3(0, 0, -horizontal * 1000 * Time.deltaTime));
        //transform.Rotate(0, rightJoyStickY, 0);
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        //transform.localRotation = Quaternion.AngleAxis(MD.x, Vector3.up);
        //transform.localRotation = Quaternion.AngleAxis(-MD.y, Vector3.forward);
    }



    private void Awake()
    {
        targetRotation = Quaternion.identity;
    }

    private void checkTilt()
    {
        Vector3 Linputs = InputManager.MainLeftJoystick();
        zRotation = Linputs.x;
        tiltAmountLaterally = Mathf.SmoothDamp(tiltAmountLaterally, 20 * zRotation, ref tiltAmountLaterally, 0.1f);
        //transform.localRotation = Quaternion.Euler( 
        //    new Vector3(transform.rotation.x, transform.rotation.y, tiltAmountLaterally)
        //    );
        //Debug.Log("tiltAmountLaterally : " + tiltAmountLaterally);
        transform.Rotate(Vector3.forward, -tiltAmountLaterally);
    }



    private void checkAiming()
    {

        if (!screenRect.Contains(Input.mousePosition))
            return;


        //get input
        //Vector3 Linputs = InputManager.MainLeftJoystick();
        Vector3 Rinputs = InputManager.MainRightJoystick();

        //Quaternion currentRotation = transform.rotation;
        //if (InputManager.MainRightJoystick() == Vector3.zero)
        //{
        //    transform.rotation = Quaternion.RotateTowards(currentRotation, startRotation, Time.deltaTime * rotateSpeed);
        //}
        //Quaternion.LookRotation(new Vector3(Input.GetAxis("K_MainVertical"), Input.GetAxis("K_MainHorizontal"),0f));

        //this worked somewhat but difficult to control once the rotation has changed
        //transform.Rotate(new Vector3(Input.GetAxis("K_MainVertical"), Input.GetAxis("K_MainHorizontal"), 0f));

        xRotation -= Rinputs.y;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += Rinputs.x;
        //yRotation = Mathf.Clamp(-90f, yRotation, 90f);
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        //Vector3 euler = transform.localEulerAngles;
        //euler.z = Mathf.Lerp(euler.z, zRotation, 10f * Time.deltaTime);
        //Vector3 euler = transform.localEulerAngles;
        //euler.z = Mathf.Lerp(euler.z, zRotation * 10, 2.0f * Time.deltaTime);
        //euler.z = Mathf.Clamp(euler.z, -10, 10);
        //transform.localEulerAngles = euler;
        //Debug.Log("euler: " + euler.z);
        //Debug.Log("transform.localEulerAngles.z: " + transform.localEulerAngles.z);

        //float tilt = zRotation - transform.position.x;

        //targetRotation = Quaternion.RotateTowards(targetRotation, Quaternion.Euler(0f, 0f, -tilt ), 100f * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation * speed, rb.rotation.z);

        //rb.MoveRotation(transform.localRotation);
        //transform.Rotate(new Vector3(0,0, transform.rotation.z),  5f * -zRotation);
        // increase 10.0f to rotate the ship faster, decrease it to rotate slower
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, zRotation * 10), 10.0f * Time.deltaTime);
        //this.transform.Rotate(Vector3.up * Rinputs.x);
        //Debug.Log(euler.z);
        //transform.localEulerAngles = euler;



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
        //transform.position -= transform.forward * Time.deltaTime * speed;
        //force = new Vector3(Linputs.x, 0.0F, Linputs.y);
        //rb.AddRelativeForce(force * 1000f);



        //float h = horizontalSpeed * Input.GetAxis("Mouse X");
        //float v = verticalSpeed * Input.GetAxis("Mouse Y");

        //Vector3 inputs = InputManager.MainJoystick();
        //target = mainCamera.ScreenToWorldPoint(new Vector3(Input.GetAxis("K_MainHorizontal"), Input.GetAxis("K_MainVertical"), inputs.z));
        //crossHairs.transform.position = new Vector3(target.x, target.y, target.z);



        //Ray cameraRay = mainCamera.ScreenPointToRay(/*Input.mousePosition*/ new Vector3(Input.GetAxis("Mouse X"),  Input.GetAxis("Mouse Y"), 0).normalized);
        //Debug.Log("mouse input position: X:" + new Vector3(h, v, 0).normalized + " and Y:" + new Vector3(h, v, 0).normalized);
        //Ray cameraRay = mainCamera.ScreenPointToRay(/*Input.mousePosition*/ new Vector3(h, v, 0).normalized);
        //Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition/* new Vector3(h, v, 0).normalized*/);
        //Plane groundPlane = new Plane(Vector3.down, Vector3.zero);
        //float rayLength;

        //if (groundPlane.Raycast(cameraRay, out rayLength))
        //{
        //    Vector3 pointToLook = cameraRay.GetPoint(rayLength);
        //    Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
        //    lastRotation = transform.localEulerAngles.z.TwoDecimals();
        //    transform.LookAt(pointToLook);
        //}

        //****************************************************************************
        //Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition/* new Vector3(h, v, 0).normalized*/);
        //Plane groundPlane = new Plane(Vector3.down, Vector3.zero);
        //float rayLength;

        //if (groundPlane.Raycast(cameraRay, out rayLength))
        //{
        //    Vector3 pointToLook = cameraRay.GetPoint(rayLength);
        //    Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
        //    //pointToLook.Normalize();
        //    //float rot = Mathf.Atan2(-pointToLook.x, -pointToLook.z) * Mathf.Rad2Deg;
        //    //transform.rotation = Quaternion.Euler(0, -rot, 0) ;
        //    //lastRotation = transform.localEulerAngles.z.TwoDecimals();
        //    //transform.LookAt(pointToLook);
        //    //transform.rotation = Quaternion.Euler(0, pointToLook.y, 0);

        //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition/*new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"),0)*/);
        //    Vector3 dif = mousePosition - transform.position;
        //    dif.Normalize();
        //    float tilt = Mathf.Atan2(-dif.x, -dif.z) * Mathf.Rad2Deg;
        //    //float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

        //    lastRotation = -tilt;
        //    transform.rotation = Quaternion.Euler(0, 0, -tilt);
        //}
        //****************************************************************************
        //Debug.Log("Rotation: " + transform.rotation);
        //transform.LookAt(pointToLook);




        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition/*new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"),0)*/);
        //Vector3 dif = mousePosition - transform.position;
        //dif.Normalize();
        //float rot = Mathf.Atan2(-dif.x, -dif.z) * Mathf.Rad2Deg;
        ////float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

        //lastRotation = -rot;
        //transform.rotation = Quaternion.Euler(0, 0, -rot);
        //Debug.Log("Rotation: " + transform.rotation);






        //float h = horizontalSpeed * Input.GetAxis("Mouse X");
        //float v = verticalSpeed * Input.GetAxis("Mouse Y");
        //Debug.Log("horizontal " + h);
        //Debug.Log("vertical " + v);
        //transform.Rotate(0, h, 0);
        ////transform.Rotate(0, v, 0);
        ////transform.Rotate(0, -h, 0);
        //transform.Rotate(0, -v, 0);
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
