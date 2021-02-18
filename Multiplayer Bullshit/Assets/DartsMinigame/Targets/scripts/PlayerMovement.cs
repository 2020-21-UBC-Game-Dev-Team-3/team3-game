using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(50, 50);
    private Vector3 mousePos = Vector3.zero;

    // Update is called once per frame
    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        mousePos = Input.mousePosition;
        /*float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);

        movement *= Time.deltaTime;

        transform.Translate(movement);*/
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }
}
