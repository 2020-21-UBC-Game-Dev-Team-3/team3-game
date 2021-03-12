using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public float speed = 50f;
    public float rotationSpeed = 50f; 
    public Rigidbody rbody;
    public GameObject player;
 
    void Start()
    {
        rbody = GetComponent<Rigidbody>();

        
    }

   

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKey("w")){
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);

        }
        if (Input.GetKey("s")){
            transform.Translate(Vector3.forward * Time.deltaTime * -speed, Space.Self);
        }

        if(Input.GetKey("a")){
            transform.Rotate(Vector3.up * Time.deltaTime * -rotationSpeed, Space.Self);
        }   

        if(Input.GetKey("d")){
            transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.Self);
        }
        //float h = Input.GetAxis("Horizontal");
        // float v = Input.GetAxis("Vertical");
        // Console.WriteLine(h);
        // Console.WriteLine(v);
        //rbody.AddTorque(0f, h*turnSpeed*Time.deltaTime, 0f);
        //rbody.AddForce(transform.forward*v*accelerateSpeed*Time.deltaTime);
        // rbody.AddForce(0,0,accelerateSpeed * Time.deltaTime); //Add a forward force on the z axis
        // if(Input.GetKey("d")){
        //     rbody.AddForce(turnSpeed *Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        // }
        // if(Input.GetKey("a")){
        //     rbody.AddForce(-turnSpeed *Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        // }
        

    }


}
