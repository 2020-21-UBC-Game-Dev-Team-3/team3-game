using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;
    public GameObject floor1Button, floor2Button, floor3Button;
    public GameObject Elevator;
    private float distToElevator;

    //public GameObject[] taskLocations;
    //public GameObject taskButton;
    public GameObject minigameCanvas;
    //private float distToTask;
    private float distToIndicator;
    private float minTaskDist = 5;
    private int numTasks = 2;
    //private bool taskFlag;
    public bool inMinigame;
    public GameObject[] taskIndicators;

    //public CharacterController controller;

    public float moveSpeed;
    public float smoothTime;
    public float turnSmoothTime;
    public float turnSmoothVelocity;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    Vector3 rotationSpeed;

    Animator animator;

    PhotonView playerPV;

    Rigidbody rb;

    Camera cam;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerPV = GetComponent<PhotonView>();
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        //For minigames
        minigameCanvas = getMinigameCanvas();
        //taskLocations = GameObject.FindGameObjectsWithTag("TaskLocation");
        
        //taskButton = GameObject.FindGameObjectWithTag("TaskButton");
        taskIndicators = new GameObject[numTasks];
        taskIndicators[0] = GameObject.FindGameObjectWithTag("Task1Indicator");
        taskIndicators[1] = GameObject.FindGameObjectWithTag("Task2Indicator");
        foreach (GameObject indicator in taskIndicators)
        {
            indicator.SetActive(false);
        }
        //taskButton.SetActive(false);
        inMinigame = false;

        //Attatches all the necessary components to player
        Elevator = GameObject.FindGameObjectWithTag("Elevator");
        floor1Button = GameObject.FindGameObjectWithTag("Floor1Button");
        floor2Button = GameObject.FindGameObjectWithTag("Floor2Button");
        floor3Button = GameObject.FindGameObjectWithTag("Floor3Button");
        ElevatorButtonsOff();
        rotationSpeed = new Vector3(0, 40, 0);

        if (!playerPV.IsMine)
        {
            Destroy(playerCamera);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(minigameCanvas.activeSelf);
        if (!minigameCanvas.activeSelf)
        {
            Interact();
            //taskButtonCheck();
            taskIndicatorCheck();

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

            if (!playerPV.IsMine) return;

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
        if (!minigameCanvas.activeSelf) { 
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
    /*void taskButtonCheck()
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
    }*/

    void taskIndicatorCheck()
    {
        foreach (GameObject indicator in taskIndicators)
        {
            distToIndicator = Vector3.Distance(transform.position, indicator.transform.position);

            if (distToIndicator <= minTaskDist)
            {
                indicator.SetActive(true);
            } else
            {
                indicator.SetActive(false);
            }
        }

    }

    GameObject getMinigameCanvas()
    {
        List<GameObject> allObjects = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                allObjects.Add(go);
        }

        foreach (GameObject gameObject in allObjects)
        {
            if (gameObject.tag == "MinigameCanvas")
            {
                return gameObject;
            }
        }
        //should never get here
        return null;
      
    }

    //Set in/out of minigame to stop player movement while in minigame
    /*public void setInMinigame()
    {
        StartCoroutine(setInMinigameCoroutine());
        Debug.Log("got here");
    }
    public void setOutMinigame()
    {
        inMinigame = false;
    }

    IEnumerator setInMinigameCoroutine()
    {
        inMinigame = true;
        Debug.Log("Set in minigame");
        Debug.Log(inMinigame);
        yield return new WaitForSeconds(2);
    }*/

    void Interact()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("TaskLocation") /*|| hit.transform.CompareTag("Interactable")*/)
                {
                    if (!hit.transform.gameObject.activeInHierarchy) return;
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    ChooseInteractionEvent(interactable);
                }
            }
        }
    }
    void ChooseInteractionEvent(Interactable interactable)
    {
        if (interactable.GetInteractableName() == "Minigame1" && taskIndicators[0].activeSelf)
        {
            Debug.Log("minigame 1");
            minigameCanvas.SetActive(true);

        }
        if (interactable.GetInteractableName() == "Minigame2" && taskIndicators[1].activeSelf)
        {
            Debug.Log("minigame 2");
            minigameCanvas.SetActive(true);
        }
    }
}


