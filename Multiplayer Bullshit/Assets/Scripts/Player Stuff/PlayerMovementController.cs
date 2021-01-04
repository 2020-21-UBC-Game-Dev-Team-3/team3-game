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

    public GameObject minigameCanvas;
    private float distToIndicator;
    private float minTaskDist = 5;
    private int numBadInteractables = 5;
    public GameObject[] badInteractables;


    private bool inVent;
    public Transform Vent1Pos;
    public Transform Vent2Pos;
    public Transform Vent3Pos;
    private float threshold = 1.0f; //magic number but it works and idk what else to do
    private float thresholdSquared;


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
        if (!minigameCanvas.activeSelf && !inVent)
        {
            GetComponent<Animator>().enabled = true;
            InteractableCheck();
            Interact();

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



            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("IsWalking", true);

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

    void CheckFalse()
    {
        GetComponent<Animator>().enabled = false;

        if (inVent)
        {
            CurrentlyInVent();
        }
    }

    void FixedUpdate()
    {
        if (!minigameCanvas.activeSelf && !inVent) { 
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
    void InteractableCheck()
    {
        foreach (GameObject interactable in badInteractables)
        {
            distToIndicator = Vector3.Distance(transform.position, interactable.transform.position);

            if (distToIndicator <= minTaskDist)
            {
                interactable.transform.GetChild(0).gameObject.SetActive(true);
            } else
            {
                interactable.transform.GetChild(0).gameObject.SetActive(false);
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
        //no changes
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
            
        } else if (Input.GetKeyDown("a") || Input.GetKeyDown("w") || Input.GetKeyDown("d") || Input.GetKeyDown("s"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            inVent = false;
        }
    }
}


