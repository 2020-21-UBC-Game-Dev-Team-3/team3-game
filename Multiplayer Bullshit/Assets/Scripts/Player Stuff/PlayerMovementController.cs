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
    [SerializeField] GameObject TargetCamera;
    [SerializeField] GameObject meetingIndicator;
    //[SerializeField] GameObject emergencyMeetingEvent;
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

   if (switched)
   {
    PlayerIsComingBack();
   }

    if (!pv.IsMine) {
      Destroy(playerCamera);
      Destroy(minimapCamera);
      Destroy(TargetCamera);
      Destroy(playerIndicator);
      Destroy(meetingIndicator);
    }
  }

  // Update is called once per frame
  void Update() {
    if (!switched) {
      GetComponent<Animator>().enabled = true;
      //Interact();

      if (!pv.IsMine) return;

      //PlayerMovement();
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

    //void PlayerMovement() {
    //    if (!switched)
    //    {

    //        float horizontal = Input.GetAxisRaw("Horizontal");
    //        float vertical = Input.GetAxisRaw("Vertical");


    //        Vector3 playerMovement = new Vector3(-vertical, 0f, horizontal) * moveSpeed * Time.deltaTime;
    //        transform.Translate(playerMovement, Space.Self);

    //        SetAnimator(playerMovement);
    //    }
 // }

  void SetAnimator(Vector3 direction) {
    //Debug.Log(direction.magnitude);
    if (direction.magnitude >= 0.01f) {
      animator.SetBool("IsWalking", true);
    } else {
      animator.SetBool("IsWalking", false);
    }
  }
}
