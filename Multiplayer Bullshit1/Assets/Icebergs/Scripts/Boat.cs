using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class boat : MonoBehaviour
{
    public float speed = 50f;
    public float rotationSpeed = 50f; 
    public Rigidbody rbody;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow)){
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);

        }
        if(Input.GetKey(KeyCode.DownArrow)){
            transform.Translate(Vector3.forward * Time.deltaTime * -speed, Space.Self);
        }

        if(Input.GetKey(KeyCode.LeftArrow)){
            transform.Rotate(Vector3.up * Time.deltaTime * -rotationSpeed, Space.Self);
        }   

        if(Input.GetKey(KeyCode.RightArrow)){
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
