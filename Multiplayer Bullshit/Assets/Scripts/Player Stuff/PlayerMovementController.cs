using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject minimapCamera;
    [SerializeField] GameObject playerIndicator;
    [SerializeField] GameObject emergencyMeetingEvent;


    public GameObject floor1Button, floor2Button, floor3Button;
    public GameObject callElevatorButton1, callElevatorButton2, callElevatorButton3;
    public GameObject Elevator;
    public GameObject ElevatorStation;
    private float distToElevator;
    private string ElevatorCallName;
    private float distToElevatorCall;

    // minigame interaction stuff
    public GameObject minigameCanvas;
    private float distToIndicator;
    private float minTaskDist = 5;
    private int numBadInteractables = 5;
    public GameObject[] badInteractables;

    // venting stuff
    private bool inVent;
    public Transform Vent1Pos;
    public Transform Vent2Pos;
    public Transform Vent3Pos;
    private float threshold = 1.0f; //magic number but it works and idk what else to do
    private float thresholdSquared;

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

    Camera cam;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        cam = Camera.main;
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
        minigameCanvas = getMinigameCanvas();
        badInteractables = GameObject.FindGameObjectsWithTag("BadInteractable");
        foreach (GameObject interactable in badInteractables)
        {
            interactable.SetActive(true);
            interactable.transform.GetChild(0).gameObject.SetActive(false);
        }
        //For venting
        inVent = false;
        Vent1Pos = GameObject.FindGameObjectWithTag("Vent1Pos").transform;
        Vent2Pos = GameObject.FindGameObjectWithTag("Vent2Pos").transform;
        Vent3Pos = GameObject.FindGameObjectWithTag("Vent3Test").transform;
        thresholdSquared = threshold * threshold;
        ElevatorStation = FindClosestStation();
        ElevatorCallName = FindClosestStation().name;
        ElevatorButtonsOff();
        rotationSpeed = new Vector3(0, 40, 0);

        if (!pv.IsMine)
        {
            Destroy(playerCamera);
            Destroy(minimapCamera);
            Destroy(playerIndicator);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!minigameCanvas.activeSelf && !inVent)
        {
            GetComponent<Animator>().enabled = true;
            InteractableCheck();
            Interact();
            //This is for elevator buttons
            distToElevator = Vector3.Distance(transform.position, Elevator.transform.position);
            if (distToElevator <= 1 && pv.IsMine)
            {
                ElevatorButtonsOn();
            }
            else if (distToElevator > 1 && pv.IsMine)
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
        else
        {
            CheckFalse();
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

    //To show/hide the task button
    void InteractableCheck()
    {
        foreach (GameObject interactable in badInteractables)
        {
            distToIndicator = Vector3.Distance(transform.position, interactable.transform.position);

            if (distToIndicator <= minTaskDist)
            {
                interactable.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                interactable.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    void Interact()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Interactable"))
                {
                    if (!hit.transform.gameObject.activeInHierarchy) return;
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    ChooseInteractionEvent(interactable);
                }
                if (hit.transform.CompareTag("BadInteractable"))
                {
                    if (!hit.transform.gameObject.activeInHierarchy) return;
                    BadInteractable interactable = hit.collider.GetComponent<BadInteractable>();
                    BadChooseInteractionEvent(interactable);
                }
            }
        }
    }

    void ChooseInteractionEvent(Interactable interactable)
    {
        if (interactable.GetInteractableName() == "Emergency button")
        {
            pv.RPC("TurnOnEmergencyPopUp", RpcTarget.All);
        }
    }

    [PunRPC]
    public void TurnOnEmergencyPopUp()
    {
        StartCoroutine(ShowEmergencyPopUp());
    }

    IEnumerator ShowEmergencyPopUp()
    {
        emergencyMeetingEvent.SetActive(true);
        yield return new WaitForSeconds(2);
        emergencyMeetingEvent.SetActive(false);
    }

    void BadChooseInteractionEvent(BadInteractable interactable)
    {
        if (interactable.GetInteractableName() == "Minigame1" && interactable.transform.GetChild(0).gameObject.activeSelf == true)
        {
            minigameCanvas.SetActive(true);
        }
        if (interactable.GetInteractableName() == "Minigame2" && interactable.transform.GetChild(0).gameObject.activeSelf == true)
        {
            minigameCanvas.SetActive(true);
        }
        if (interactable.GetInteractableName() == "Vent" && interactable.transform.GetChild(0).gameObject.activeSelf == true)
        {
            EnterVent(interactable);
        }
    }

    void CheckFalse()
    {
        GetComponent<Animator>().enabled = false;

        if (inVent)
        {
            CurrentlyInVent();
        }
    }

    void CurrentlyInVent()
    {
        if (Input.GetKeyDown("space"))
        {
            if ((transform.position - Vent1Pos.transform.position).sqrMagnitude < thresholdSquared)
            {
                transform.position = Vent2Pos.transform.position;
            }
            else if ((transform.position - Vent2Pos.transform.position).sqrMagnitude < thresholdSquared)
            {
                transform.position = Vent3Pos.transform.position;
            }
            else if ((transform.position - Vent3Pos.transform.position).sqrMagnitude < thresholdSquared)
            {
                transform.position = Vent1Pos.transform.position;
            }

        }
        else if (Input.GetKeyDown("a") || Input.GetKeyDown("w") || Input.GetKeyDown("d") || Input.GetKeyDown("s"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            inVent = false;
        }
    }

    void EnterVent(BadInteractable interactable)
    {
        foreach (GameObject interactable2 in badInteractables)
        {
            interactable2.SetActive(true);
            interactable2.transform.GetChild(0).gameObject.SetActive(false);
        }

        inVent = true;
        transform.position = interactable.transform.GetChild(1).gameObject.transform.position;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

    }
}
