using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    public GameObject door;
    public Transform floor1, floor2, floor3;
    public Transform doorOpened, doorClosed;
    private bool buttonPressed;
    private bool openingDoor, closingDoor, isDoorClosed, overridder, moving;
    private int currentFloor;
    private int destination;
    void Start()
    {
        buttonPressed = false;
        currentFloor = 1;

    }


    void Update()
    {
        //case switch doesn't work with transform positions apparently :/
        if (floor1.transform.position == transform.position)
        {
            currentFloor = 1;
        }
        else if (floor2.transform.position == transform.position)
        {
            currentFloor = 2;
        }
        else if (floor3.transform.position == transform.position)
        {
            currentFloor = 3;
        }
        // stops elevator at destination
        if (destination == currentFloor && !overridder)
        {
            openingDoor = true;
            moving = false;
            buttonPressed = false;
            destination = 0;
        }
        //door Logic
        if (door.transform.position == doorClosed.transform.position)
        {
            isDoorClosed = true;
        }
        else if (door.transform.position == doorOpened.transform.position)
        {
            isDoorClosed = false;
        }
        //this moves the elevator
        switch (destination)
        {
            case 1:
                transform.position = Vector3.MoveTowards(transform.position, floor1.transform.position, 1f * Time.deltaTime);
                break;
            case 2:
                transform.position = Vector3.MoveTowards(transform.position, floor2.transform.position, 1f * Time.deltaTime);
                break;
            case 3:
                transform.position = Vector3.MoveTowards(transform.position, floor3.transform.position, 1f * Time.deltaTime);
                break;
            default:
                break;

        }
        //door movement
        if (closingDoor && !isDoorClosed)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, doorClosed.transform.position, 5f * Time.deltaTime);
        }
        if (openingDoor && isDoorClosed && destination == 0 && !moving)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, doorOpened.transform.position, 5f * Time.deltaTime);
        }
    }

    public void Button1()
    {
        if (!buttonPressed && currentFloor != 1)
        {
            buttonPressed = true;
            StartCoroutine(MoveElevator(1));
        }
    }

    public void Button2()
    {
        if (!buttonPressed && currentFloor != 2)
        {
            buttonPressed = true;
            StartCoroutine(MoveElevator(2));
        }
    }

    public void Button3()
    {
        if (!buttonPressed && currentFloor != 3)
        {
            buttonPressed = true;
            StartCoroutine(MoveElevator(3));
        }
    }

    IEnumerator MoveElevator(int floorNum)
    {
        overridder = true;
        moving = true;
        closingDoor = true;
        yield return new WaitForSeconds(4);
        closingDoor = false;
        destination = floorNum;
        yield return new WaitForSeconds(1);
        overridder = false;
    }

}
