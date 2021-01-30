using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject minimapCamera;

    [SerializeField] GameObject playerIndicator;

    [SerializeField] GameObject emergencyMeetingEvent;
    [SerializeField] GameObject votingManager;

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
        rotationSpeed = new Vector3(0, 40, 0);

        if (!playerPV.IsMine)
        {
            Destroy(playerCamera);
            Destroy(minimapCamera);
            Destroy(playerIndicator);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerPV.IsMine) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, direction * moveSpeed, ref smoothMoveVelocity, smoothTime);

        SetAnimator(direction);

        Interact();

    }


    //void RotatePlayer(Vector3 movement)
    //{
    //    float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
    //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

    //    transform.Rotate(Vector3.up * angle);
    //    Quaternion deltaRotation = Quaternion.Euler(0f, targetAngle, 0f);
    //    rb.MoveRotation(rb.rotation * deltaRotation);
    //}

    void SetAnimator(Vector3 direction)
    {
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

    void Interact()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Interactable"))
                {
                    if (!hit.transform.GetChild(0).gameObject.activeInHierarchy) return;
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    ChooseInteractionEvent(interactable);
                }
            }
        }
    }

    void ChooseInteractionEvent(Interactable interactable)
    {
        if(interactable.GetInteractableName() == "Emergency button")
        {
            playerPV.RPC("TurnOnEmergencyPopUp", RpcTarget.All);
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
        votingManager.SetActive(true);
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        Quaternion deltaRotation = Quaternion.Euler(moveAmount.x * rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
