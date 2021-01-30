using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GhostController : MonoBehaviour
{

    [SerializeField] GameObject playerCamera;
    
    //public CharacterController controller;

    public float moveSpeed;

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

        if (!pv.IsMine)
        {
            Destroy(playerCamera);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return;

        GhostMovement();

    }


void GhostMovement(){

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float upDown = Input.GetAxisRaw("yAxis");


       Vector3 ghostMovement = new Vector3(horizontal, upDown, vertical) * moveSpeed * Time.deltaTime;
       transform.Translate(ghostMovement, Space.Self);

       // Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
       // moveAmount = Vector3.SmoothDamp(moveAmount, direction * moveSpeed, ref smoothMoveVelocity, smoothTime);

       //Vector3 playerMovement = new Vector3(horizontal, 0f, vertical) * moveSpeed * Time.deltaTime;

      // transform.Translate(playerMovement, Space.Self);

        //SetAnimator(playerMovement);

    }

/*
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

    */
}
