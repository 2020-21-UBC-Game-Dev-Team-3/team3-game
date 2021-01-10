using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;
using UnityEngine.UI;

public class ElevatorScript : MonoBehaviour
{
    PhotonView pv;
    public GameObject door;
    public Button button1, button2, button3, callButton1, callButton2, callButton3;
    public Transform floor1, floor2, floor3;
    public Transform doorOpened, doorClosed;
    public Text elevatorText;
    private bool buttonPressed;
    private bool openingDoor, closingDoor, isDoorClosed, overridder, moving, cooldown;
    private int currentFloor;
    private int destination;
    void Start()
    {


        Vector3 rot = transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        transform.rotation = Quaternion.Euler(rot);

        buttonPressed = false;
        currentFloor = 1;
        floor1 = GameObject.Find("Floor1pos").transform;
        Debug.Log(GameObject.Find("Floor1pos").name);
        floor2 = GameObject.Find("Floor2pos").transform;
        floor3 = GameObject.Find("Floor3pos").transform;
        button1 = GameObject.Find("Floor1Button").GetComponent<Button>();
        button2 = GameObject.Find("Floor2Button").GetComponent<Button>();
        button3 = GameObject.Find("Floor3Button").GetComponent<Button>();
        callButton1 = GameObject.Find("CallFloor1Button").GetComponent<Button>();
        callButton2 = GameObject.Find("CallFloor2Button").GetComponent<Button>();
        callButton3 = GameObject.Find("CallFloor3Button").GetComponent<Button>();
        elevatorText = GameObject.Find("ElevatorText").GetComponent<Text>();
        elevatorText.text = "";
        button1.onClick.AddListener(delegate { Button1(); });
        button2.onClick.AddListener(delegate { Button2(); });
        button3.onClick.AddListener(delegate { Button3(); });
        callButton1.onClick.AddListener(delegate { Button1(); });
        callButton2.onClick.AddListener(delegate { Button2(); });
        callButton3.onClick.AddListener(delegate { Button3(); });
    }
    private void Awake()
    {
        cooldown = false;
        pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    void Update()
    {
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
            pv.RPC("MovingFalse", RpcTarget.All);
            pv.RPC("OpeningDoor", RpcTarget.All);
        }
        //door Logic
        if (door.transform.position == doorClosed.transform.position)
        {
            pv.RPC("DoorClosedTrue", RpcTarget.All);
        }
        else if (door.transform.position == doorOpened.transform.position)
        {
            pv.RPC("DoorClosedFalse", RpcTarget.All);
        }
        //this moves the elevator
        switch (destination)
        {
            case 1:
                transform.position = Vector3.MoveTowards(transform.position, floor1.transform.position, 0.5f * Time.deltaTime);
                break;
            case 2:
                transform.position = Vector3.MoveTowards(transform.position, floor2.transform.position, 0.5f * Time.deltaTime);
                break;
            case 3:
                transform.position = Vector3.MoveTowards(transform.position, floor3.transform.position, 0.5f * Time.deltaTime);
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
        if (!buttonPressed && currentFloor != 1 && !cooldown)
        {
            pv.RPC("PressButton", RpcTarget.All);
            pv.RPC("StartMove", RpcTarget.All,1);
            if (pv.IsMine) { StartCoroutine(ElevatorTexting("Moving to floor 1")); }

        }
        else if(currentFloor != 1 && pv.IsMine)
        {
            StartCoroutine(ElevatorTexting("Elevator is busy"));
        }
    }
    IEnumerator ElevatorTexting(string text)
    {
        elevatorText.text = text;
        yield return new WaitForSeconds(2);
        elevatorText.text = "";
    }
    public void Button2()
    {
        if (!buttonPressed && currentFloor != 2 && !cooldown)
        {            
            pv.RPC("PressButton", RpcTarget.All);
            pv.RPC("StartMove", RpcTarget.All, 2);
            if (pv.IsMine) { StartCoroutine(ElevatorTexting("Moving to floor 2")); }

        }
        else if (currentFloor != 2 && pv.IsMine)
        {
            StartCoroutine(ElevatorTexting("Elevator is busy"));
        }
    }
    public void Button3()
    {
        if (!buttonPressed && currentFloor != 3 && !cooldown)
        {            
            pv.RPC("PressButton", RpcTarget.All);
            pv.RPC("StartMove", RpcTarget.All,3);
            if (pv.IsMine) { StartCoroutine(ElevatorTexting("Moving to floor 3")); }
        }
        else if (currentFloor != 3 && pv.IsMine)
        {
            StartCoroutine(ElevatorTexting("Elevator is busy"));
        }
    }
    [PunRPC]
    public void StartMove(int i)
    {
        StartCoroutine(MoveElevator(i));
    }
    IEnumerator MoveElevator(int floorNum)
    {
        pv.RPC("CooldownTrue", RpcTarget.All);
        pv.RPC("OverrideTrue", RpcTarget.All);
        pv.RPC("MovingTrue", RpcTarget.All);
        pv.RPC("ClosingTrue", RpcTarget.All);
        yield return new WaitForSeconds(4);
        pv.RPC("ClosingFalse", RpcTarget.All);
        if(floorNum == 1)
        {
            pv.RPC("Destination1", RpcTarget.All);
        }
        else if(floorNum == 2)
        {
            pv.RPC("Destination2", RpcTarget.All);
        }
        else if (floorNum == 3)
        {
            pv.RPC("Destination3", RpcTarget.All);
        }
        yield return new WaitForSeconds(1);
        pv.RPC("OverrideFalse", RpcTarget.All);
    }
    [PunRPC]
    public void ClosingTrue()
    {
        closingDoor = true;
    }
    [PunRPC]
    public void ClosingFalse()
    {
        closingDoor = false;
    }
    [PunRPC]
    public void OpeningTrue()
    {
        openingDoor = true;
    }
    [PunRPC]
    public void OpeningFalse()
    {
        openingDoor = false;
    }
    [PunRPC]
    public void MovingTrue()
    {
        moving = true;
    }
    [PunRPC]
    public void MovingFalse()
    {
        moving = false;
    }
    [PunRPC]
    public void PressButton()
    {
        buttonPressed = true;
    }
    [PunRPC]
    void UnPressButton()
    {
        buttonPressed = false;
    }
    [PunRPC]
    void Destination0()
    {
        destination = 0;
    }
    [PunRPC]
    void Destination1()
    {
        destination = 1;
    }
    [PunRPC]
    void Destination2()
    {
        destination = 2;
    }
    [PunRPC]
    void Destination3()
    {
        destination = 3;
    }
    [PunRPC]
    void CooldownTrue()
    {
        cooldown = true;
    }
    [PunRPC]
    void CooldownFalse()
    {
        cooldown = false;
    }
    [PunRPC]
    void OverrideTrue()
    {
        overridder = true;
    }
    [PunRPC]
    void OverrideFalse()
    {
        overridder = false;
    }
    [PunRPC]
    void OpeningDoor()
    {
        StartCoroutine(OpenDoor());
    }
    [PunRPC]
    void DoorClosedTrue()
    {
        isDoorClosed = true;
    }
    [PunRPC]
    void DoorClosedFalse()
    {
        isDoorClosed = false;
    }
    IEnumerator OpenDoor()
    { 
        pv.RPC("Destination0", RpcTarget.All);
        pv.RPC("OpeningTrue", RpcTarget.All);
        yield return new WaitForSeconds(6);
        pv.RPC("UnPressButton", RpcTarget.All);
        pv.RPC("CooldownFalse", RpcTarget.All);

    }

}
