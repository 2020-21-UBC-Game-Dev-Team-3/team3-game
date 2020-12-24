using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovementController : MonoBehaviour
{
    
    [SerializeField] GameObject playerCamera;
    
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
        rotationSpeed = new Vector3(0, 40, 0);

        if (!pv.IsMine)
        {
            Destroy(playerCamera);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        if (!pv.IsMine) return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        Quaternion deltaRotation = Quaternion.Euler(moveAmount.x * rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
