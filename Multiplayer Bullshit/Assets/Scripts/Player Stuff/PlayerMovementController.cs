using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;
    public GameObject floor1Button, floor2Button, floor3Button;
    public GameObject callElevatorButton1, callElevatorButton2, callElevatorButton3;
    public GameObject Elevator;
    public GameObject ElevatorStation;
    private float distToElevator;
    private string ElevatorCallName;
    private float distToElevatorCall;

    //public CharacterController controller;

    public float moveSpeed;
    public float smoothTime;
    public float turnSmoothTime;
    public float turnSmoothVelocity;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    Vector3 rotationSpeed;

    Animator animator;

    PhotonView pv;

    Rigidbody rb;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Attatches all the necessary components to player
        Elevator = GameObject.FindGameObjectWithTag("Elevator");
        floor1Button = GameObject.FindGameObjectWithTag("Floor1Button");
        floor2Button = GameObject.FindGameObjectWithTag("Floor2Button");
        floor3Button = GameObject.FindGameObjectWithTag("Floor3Button");
        callElevatorButton1 = GameObject.Find("CallFloor1Button");
        callElevatorButton2 = GameObject.Find("CallFloor2Button");
        callElevatorButton3 = GameObject.Find("CallFloor3Button");
        ElevatorStation = FindClosestStation();
        ElevatorCallName = FindClosestStation().name;
        ElevatorButtonsOff();
        rotationSpeed = new Vector3(0, 40, 0);

        if (!pv.IsMine)
        {
            Destroy(playerCamera);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //This is for elevator buttons
        distToElevator = Vector3.Distance(transform.position, Elevator.transform.position);
        if (distToElevator <= 1 && pv.IsMine)
        {
          ElevatorButtonsOn();
        }
        else if(distToElevator > 1 && pv.IsMine)
        {
           ElevatorButtonsOff();
        }

        if (!pv.IsMine) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, direction * moveSpeed, ref smoothMoveVelocity, smoothTime);

        //controller.Move(direction.normalized * moveSpeed * Time.deltaTime);
        ElevatorStation = FindClosestStation();
        ElevatorCallName = ElevatorStation.name;

        distToElevatorCall = Vector3.Distance(transform.position, ElevatorStation.transform.position);
        if (distToElevatorCall <= 2 && pv.IsMine)
        {
            switch (ElevatorCallName)
            {
                case "CallFloor1":
                    CallElevatorButtonOn1();
                    break;
                case "CallFloor2":
                    CallElevatorButtonOn2();
                    break;
                case "CallFloor3":
                    CallElevatorButtonOn3();
                    break;
                default:
                    Debug.Log("What?");
                    break;
            }
        }
        else if (distToElevatorCall > 2 && pv.IsMine)
        {
            CallElevatorButtonOff1();
            CallElevatorButtonOff2();
            CallElevatorButtonOff3();
        }


        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("IsWalking", true);

            //RotatePlayer(direction);
        }

        else
        {
            animator.SetBool("IsWalking", false);
        }

    }

    //void RotatePlayer(Vector3 movement)
    //{
    //    float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
    //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

    //    transform.Rotate(Vector3.up * angle);
    //    Quaternion deltaRotation = Quaternion.Euler(0f, targetAngle, 0f);
    //    rb.MoveRotation(rb.rotation * deltaRotation);
    //}

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        Quaternion deltaRotation = Quaternion.Euler(moveAmount.x * rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
    void ElevatorButtonsOn()
    {
        floor1Button.transform.localPosition = new Vector3(254, -128, 0);
        floor2Button.transform.localPosition = new Vector3(254, -78, 0);
        floor3Button.transform.localPosition = new Vector3(254, -28, 0);
    }
    void ElevatorButtonsOff()
    {
        floor1Button.transform.position = new Vector3(4000, 0, 0);
        floor2Button.transform.position = new Vector3(4000, 0, 0);
        floor3Button.transform.position = new Vector3(4000, 0, 0);
    }
    void CallElevatorButtonOn1()
    {
        callElevatorButton1.transform.localPosition = new Vector3(254, -78, 0);
    }
    void CallElevatorButtonOff1()
    {
        callElevatorButton1.transform.localPosition = new Vector3(4000, -136, 0);
    }
    void CallElevatorButtonOn2()
    {
        callElevatorButton2.transform.localPosition = new Vector3(254, -78, 0);
    }
    void CallElevatorButtonOff2()
    {
        callElevatorButton2.transform.position = new Vector3(4000, -136, 0);
    }

    void CallElevatorButtonOn3()
    {
        callElevatorButton3.transform.localPosition = new Vector3(254, -78, 0);
    }
    void CallElevatorButtonOff3()
    {
        callElevatorButton3.transform.position = new Vector3(4000, -136, 0);
    }

    public GameObject FindClosestStation()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("CallFloor1");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
