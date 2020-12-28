using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;
    public GameObject floor1Button, floor2Button, floor3Button;
    public GameObject Elevator;
    private float distToElevator;

    public GameObject[] taskLocations;
    public GameObject taskButton;
    private float distToTask;
    private float minTaskDist = 3;
    private bool taskFlag;
    public bool inMinigame;

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
        //For minigames
        taskLocations = GameObject.FindGameObjectsWithTag("TaskLocation");
        taskButton = GameObject.FindGameObjectWithTag("TaskButton");
        taskButton.SetActive(false);
        inMinigame = false;

        //Attatches all the necessary components to player
        Elevator = GameObject.FindGameObjectWithTag("Elevator");
        floor1Button = GameObject.FindGameObjectWithTag("Floor1Button");
        floor2Button = GameObject.FindGameObjectWithTag("Floor2Button");
        floor3Button = GameObject.FindGameObjectWithTag("Floor3Button");
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
        Debug.Log(inMinigame);
        if (!inMinigame)
        {
            taskButtonCheck();

            //This is for elevator buttons
            distToElevator = Vector3.Distance(transform.position, Elevator.transform.position);
            if (distToElevator <= 1)
            {
                ElevatorButtonsOn();
            }
            else
            {
                ElevatorButtonsOff();
            }

            if (!pv.IsMine) return;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            moveAmount = Vector3.SmoothDamp(moveAmount, direction * moveSpeed, ref smoothMoveVelocity, smoothTime);

            //controller.Move(direction.normalized * moveSpeed * Time.deltaTime);


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
        if (!inMinigame) { 
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        Quaternion deltaRotation = Quaternion.Euler(moveAmount.x * rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
            }
    }
    void ElevatorButtonsOn()
    {
        floor1Button.SetActive(true);
        floor2Button.SetActive(true);
        floor3Button.SetActive(true);
    }
    void ElevatorButtonsOff()
    {
        floor1Button.SetActive(false);
        floor2Button.SetActive(false);
        floor3Button.SetActive(false);
    }

    //To show/hide the task button
    void taskButtonCheck()
    {
        taskFlag = false;

        foreach (GameObject location in taskLocations)
        {
            distToTask = Vector3.Distance(transform.position, location.transform.position);

            if (distToTask <= minTaskDist)
            {
                taskButton.SetActive(true);
                taskFlag = true;
                break;
            }
        }

        if (!taskFlag)
        {
            taskButton.SetActive(false);
        }
    }

    //Set in/out of minigame to stop player movement while in minigame
    public void setInMinigame()
    {
        inMinigame = true;
        Debug.Log("Set in minigame");
        Debug.Log(inMinigame);
    }
    /*public void setOutMinigame()
    {
        inMinigame = false;
    }*/
}
