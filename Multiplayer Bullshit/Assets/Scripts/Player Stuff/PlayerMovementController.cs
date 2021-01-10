using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerMovementController : MonoBehaviour {
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
  public GameObject[] interactables;
  static bool switched;

  // venting stuff
  private bool inVent;
  public Transform Vent1Pos;
  public Transform Vent2Pos;
  public Transform Vent3Pos;
  private float threshold = 1.0f; //magic number but it works and idk what else to do
  private float thresholdSquared;

  //public CharacterController controller;

  public float moveSpeed;

  Animator animator;

  PhotonView pv;

  Rigidbody rb;

  Camera cam;

  void Awake() {
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody>();
    pv = GetComponent<PhotonView>();
    cam = Camera.main;
  }

  // Start is called before the first frame update
  void Start() {
    //Attatches all the necessary components to player
    Elevator = GameObject.FindGameObjectWithTag("Elevator");
    floor1Button = GameObject.FindGameObjectWithTag("Floor1Button");
    floor2Button = GameObject.FindGameObjectWithTag("Floor2Button");
    floor3Button = GameObject.FindGameObjectWithTag("Floor3Button");
    callElevatorButton1 = GameObject.Find("CallFloor1Button");
    callElevatorButton2 = GameObject.Find("CallFloor2Button");
    callElevatorButton3 = GameObject.Find("CallFloor3Button");

    interactables = GameObject.FindGameObjectsWithTag("Interactable");
    foreach (GameObject interactable in interactables) {
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

   if (switched)
   {
    PlayerIsComingBack();
   }

    if (!pv.IsMine) {
      Destroy(playerCamera);
      Destroy(minimapCamera);
      Destroy(playerIndicator);
    }
  }

  // Update is called once per frame
  void Update() {
    if (!switched && !inVent) {
      GetComponent<Animator>().enabled = true;
      Interact();
      //This is for elevator buttons
      distToElevator = Vector3.Distance(transform.position, Elevator.transform.position);
      if (distToElevator <= 1 && pv.IsMine) {
        ElevatorButtonsOn();
      } else if (distToElevator > 1 && pv.IsMine) {
        ElevatorButtonsOff();
      }

      if (!pv.IsMine) return;

      PlayerMovement();

      //controller.Move(direction.normalized * moveSpeed * Time.deltaTime);
      ElevatorStation = FindClosestStation();
      ElevatorCallName = ElevatorStation.name;

      distToElevatorCall = Vector3.Distance(transform.position, ElevatorStation.transform.position);
      if (distToElevatorCall <= 2 && pv.IsMine) {
        switch (ElevatorCallName) {
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
      } else if (distToElevatorCall > 2 && pv.IsMine) {
        CallElevatorButtonOff1();
        CallElevatorButtonOff2();
        CallElevatorButtonOff3();
      }
    } else {
            Debug.Log("check1");
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

  void ElevatorButtonsOn() {
    floor1Button.transform.localPosition = new Vector3(254, -128, 0);
    floor2Button.transform.localPosition = new Vector3(254, -78, 0);
    floor3Button.transform.localPosition = new Vector3(254, -28, 0);
  }
  void ElevatorButtonsOff() {
    floor1Button.transform.position = new Vector3(4000, 0, 0);
    floor2Button.transform.position = new Vector3(4000, 0, 0);
    floor3Button.transform.position = new Vector3(4000, 0, 0);
  }
  void CallElevatorButtonOn1() {
    callElevatorButton1.transform.localPosition = new Vector3(254, -78, 0);
  }
  void CallElevatorButtonOff1() {
    callElevatorButton1.transform.localPosition = new Vector3(4000, -136, 0);
  }
  void CallElevatorButtonOn2() {
    callElevatorButton2.transform.localPosition = new Vector3(254, -78, 0);
  }
  void CallElevatorButtonOff2() {
    callElevatorButton2.transform.position = new Vector3(4000, -136, 0);
  }

  void CallElevatorButtonOn3() {
    callElevatorButton3.transform.localPosition = new Vector3(254, -78, 0);
  }
  void CallElevatorButtonOff3() {
    callElevatorButton3.transform.position = new Vector3(4000, -136, 0);
  }

  public GameObject FindClosestStation() {
    GameObject[] gos;
    gos = GameObject.FindGameObjectsWithTag("CallFloor1");
    GameObject closest = null;
    float distance = Mathf.Infinity;
    Vector3 position = transform.position;
    foreach (GameObject go in gos) {
      Vector3 diff = go.transform.position - position;
      float curDistance = diff.sqrMagnitude;
      if (curDistance < distance) {
        closest = go;
        distance = curDistance;
      }
    }
    return closest;
  }


  void Interact() {
    if (Input.GetMouseButtonDown(0)) {
      Ray ray = cam.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out RaycastHit hit)) {
        if (hit.transform.CompareTag("Interactable")) {
          if (!hit.transform.gameObject.activeInHierarchy) return;
          Interactable interactable = hit.collider.GetComponent<Interactable>();
          ChooseInteractionEvent(interactable);
        }
        
      }
    }
  }

  void ChooseInteractionEvent(Interactable interactable) {
    if (interactable.GetInteractableName() == "Emergency button") {
      pv.RPC("TurnOnEmergencyPopUp", RpcTarget.All);
    }
    if (interactable.GetInteractableName() == "Minigame1" && interactable.transform.GetChild(0).gameObject.activeSelf == true)
    {
      PlayerIsSwitchingScene();
      SceneManager.LoadScene(sceneName: "LifeBoat Minigame", LoadSceneMode.Single);
    }
    if (interactable.GetInteractableName() == "Minigame2" && interactable.transform.GetChild(0).gameObject.activeSelf == true)
    {
      PlayerIsSwitchingScene();
      SceneManager.LoadScene(sceneName: "Lights Minigame", LoadSceneMode.Single);
    }
    if (interactable.GetInteractableName() == "Vent" && interactable.transform.GetChild(0).gameObject.activeSelf == true)
    {
      EnterVent(interactable);
    }
  }

  [PunRPC]
  public void TurnOnEmergencyPopUp() {
    StartCoroutine(ShowEmergencyPopUp());
  }

  IEnumerator ShowEmergencyPopUp() {
    emergencyMeetingEvent.SetActive(true);
    yield return new WaitForSeconds(2);
    emergencyMeetingEvent.SetActive(false);
  }

  void CheckFalse() {
    GetComponent<Animator>().enabled = false;
        Debug.Log("check2");

    if (inVent) {
            Debug.Log("check3");
      CurrentlyInVent();
    }
  }

  void CurrentlyInVent() {
        Debug.Log("check4");
    if (Input.GetKeyDown("space")) {
            Debug.Log("owo");
      if ((transform.position - Vent1Pos.transform.position).sqrMagnitude < thresholdSquared) {
        transform.position = Vent2Pos.transform.position;
      } else if ((transform.position - Vent2Pos.transform.position).sqrMagnitude < thresholdSquared) {
        transform.position = Vent3Pos.transform.position;
      } else if ((transform.position - Vent3Pos.transform.position).sqrMagnitude < thresholdSquared) {
        transform.position = Vent1Pos.transform.position;
      }

    } else if (Input.GetKeyDown("a") || Input.GetKeyDown("w") || Input.GetKeyDown("d") || Input.GetKeyDown("s")) {
      pv.RPC("stopInvis", RpcTarget.All);
      inVent = false;
    }
  }

  void EnterVent(Interactable interactable) {
    foreach (GameObject interactable2 in interactables) {
      interactable2.SetActive(true);
      interactable2.transform.GetChild(0).gameObject.SetActive(false);
    }

    inVent = true;
    transform.position = interactable.transform.GetChild(1).gameObject.transform.position;
    pv.RPC("turnInvis", RpcTarget.All);

  }

   [PunRPC]
   void turnInvis()//Interactable interactable)
   {

        //transform.position = interactable.transform.GetChild(1).gameObject.transform.position;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
   }
   [PunRPC]
   void stopInvis()//Interactable interactable)
   {

        //transform.position = interactable.transform.GetChild(1).gameObject.transform.position;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
   }

    void PlayerIsSwitchingScene()
    {
        PlayerPrefs.SetFloat("X", transform.position.x);
        PlayerPrefs.SetFloat("Y", transform.position.y);
        PlayerPrefs.SetFloat("Z", transform.position.z);

        switched = true;
    }
    void PlayerIsComingBack()
    {
        transform.position = new Vector3(PlayerPrefs.GetFloat("X"), PlayerPrefs.GetFloat("Y"), PlayerPrefs.GetFloat("Z"));

        switched = false;
    }

    void PlayerMovement() {
        if (!switched && !inVent)
        {

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");


            Vector3 playerMovement = new Vector3(-vertical, 0f, horizontal) * moveSpeed * Time.deltaTime;
            transform.Translate(playerMovement, Space.Self);

            SetAnimator(playerMovement);
        }
  }

  void SetAnimator(Vector3 direction) {
    //Debug.Log(direction.magnitude);
    if (direction.magnitude >= 0.01f) {
      animator.SetBool("IsWalking", true);
    } else {
      animator.SetBool("IsWalking", false);
    }
  }
}
