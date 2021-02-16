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
  //[SerializeField] GameObject emergencyMeetingEvent;


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
    if (!switched) {
      GetComponent<Animator>().enabled = true;
      //Interact();
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

  //[PunRPC]
  //public void TurnOnEmergencyPopUp() {
  //  StartCoroutine(ShowEmergencyPopUp());
  //}

  //IEnumerator ShowEmergencyPopUp() {
  //  emergencyMeetingEvent.SetActive(true);
  //  yield return new WaitForSeconds(2);
  //  emergencyMeetingEvent.SetActive(false);
  //}

  void CheckFalse() {
    GetComponent<Animator>().enabled = false;
        Debug.Log("check2");

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
        if (!switched)
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
