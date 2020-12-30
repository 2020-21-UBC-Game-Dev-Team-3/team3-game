using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class PassengerDrag : MonoBehaviour

{

    [SerializeField]private Vector3 mOffset;
    [SerializeField] private float myYpos;


    [SerializeField]private float mZCoord;



    void OnMouseDown()

    {

        if (myYpos > 33)
        {
            mZCoord = Camera.main.WorldToScreenPoint(

                gameObject.transform.position).z;



            // Store offset = gameobject world pos - mouse world pos

            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }
    }



    private Vector3 GetMouseAsWorldPoint()

    {


        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = mZCoord;



        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }



    void OnMouseDrag()

    {
        if (myYpos>33) {
            transform.position = GetMouseAsWorldPoint() + mOffset;
            }

    }

    private void Update()
    {
        myYpos = transform.position.y;
    }

}