using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;
using UnityEngine.UI;
public class ElevatorScript : MonoBehaviourPunCallbacks
{
    public AudioSource ding;
    PhotonView pv;
    public GameObject door;
    public GameObject bigDoor1, bigDoor2, bigDoor3;
    public Button button1, button2, button3, callButton1, callButton2, callButton3;
    public Transform floor1, floor2, floor3;
    public Transform doorOpened, doorClosed;
    public Text elevatorText;
    private bool buttonPressed;
    private bool openingDoor, closingDoor, isDoorClosed, overridder, moving, cooldown;
    private int currentFloor;
    private int destination;
    private float scale = 0.0f;
    private int bigDoorNumOpen = 1;
    private bool bigDoorNumClose = false;
    private float bigDoorScale = 0.0f;
    private bool ring;
    void Start()
    {


        Vector3 rot = transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        transform.rotation = Quaternion.Euler(rot);

        buttonPressed = false;
        currentFloor = 1;
        bigDoor1 = GameObject.Find("BigDoor1");
        bigDoor2 = GameObject.Find("BigDoor2");
        bigDoor3 = GameObject.Find("BigDoor3");
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
        switch (transform.position.y)
        {
            case 0:
                currentFloor = 1;
                break;
            case 3.6f:
                currentFloor = 2;
                break;
            case 7.08f:
                currentFloor = 3;
                break;
            default:
                break;
        }
        /*if (floor1.transform.position == transform.position)
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
        }*/
        // stops elevator at destination
        if (destination == currentFloor && !overridder)
        {
            pv.RPC("MovingFalse", RpcTarget.All);
            pv.RPC("OpeningDoor", RpcTarget.All);
            pv.RPC("CloseBigDoors", RpcTarget.All, false);
            pv.RPC("OpenBigDoor", RpcTarget.All, currentFloor);
        }
        //door Logic
        if (door.transform.localScale.y >= 1f)
        {
            closingDoor = false;
            pv.RPC("DoorClosedTrue", RpcTarget.All);
        }
        else if (door.transform.localScale.y <= 0)
        {
            openingDoor = false;
            pv.RPC("DoorClosedFalse", RpcTarget.All);
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
        if (closingDoor)
        {
            scale += 0.05f;
            door.transform.localScale = new Vector3(1f, scale, 1f);
        }
        if (openingDoor)
        {
            scale -= 0.05f;
            door.transform.localScale = new Vector3(1f, scale, 1f);
        }
        if (!bigDoorNumClose)
        {
            switch (bigDoorNumOpen)
            {
                case 1:
                    if (bigDoorScale > 0) { bigDoorScale -= 0.05f; }
                    bigDoor1.transform.localScale = new Vector3(1f, bigDoorScale, 1.2f);
                    break;
                case 2:
                    if (bigDoorScale > 0) { bigDoorScale -= 0.05f; }
                    bigDoor2.transform.localScale = new Vector3(1f, bigDoorScale, 1.2f);
                    break;
                case 3:
                    if (bigDoorScale > 0) { bigDoorScale -= 0.05f; }
                    bigDoor3.transform.localScale = new Vector3(1f, bigDoorScale, 1.2f);
                    break;
                default:
                    break;
            }
        }
        if (bigDoorNumClose)
        {
            if (bigDoorScale <= 1.2f) { bigDoorScale += 0.05f; }
            if (bigDoor3.transform.localScale.y <= 1.2f) { bigDoor3.transform.localScale = new Vector3(1f, bigDoorScale, 1.2f); };
            if (bigDoor2.transform.localScale.y <= 1.2f) { bigDoor2.transform.localScale = new Vector3(1f, bigDoorScale, 1.2f); };
            if (bigDoor1.transform.localScale.y <= 1.2f) { bigDoor1.transform.localScale = new Vector3(1f, bigDoorScale, 1.2f); };
        }
    }
    public void Button1()
    {
        if (!buttonPressed && currentFloor != 1 && !cooldown)
        {
            pv.RPC("PressButton", RpcTarget.All);
            pv.RPC("StartMove", RpcTarget.All, 1);
            if (pv.IsMine) { StartCoroutine(ElevatorTexting("Moving to floor 1")); }

        }
        else if (currentFloor != 1 && pv.IsMine)
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
            pv.RPC("StartMove", RpcTarget.All, 3);
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
        pv.RPC("CloseBigDoors", RpcTarget.All, true);
        pv.RPC("CooldownTrue", RpcTarget.All);
        pv.RPC("OverrideTrue", RpcTarget.All);
        pv.RPC("MovingTrue", RpcTarget.All);
        pv.RPC("ClosingTrue", RpcTarget.All);
        yield return new WaitForSeconds(4);
        pv.RPC("ClosingFalse", RpcTarget.All);
        if (floorNum == 1)
        {
            pv.RPC("Destination1", RpcTarget.All);
        }
        else if (floorNum == 2)
        {
            pv.RPC("Destination2", RpcTarget.All);
        }
        else if (floorNum == 3)
        {
            pv.RPC("Destination3", RpcTarget.All);
        }
        yield return new WaitForSeconds(1);
        ring = true;
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
        pv.RPC("PlayDing", RpcTarget.All);
        pv.RPC("OpeningTrue", RpcTarget.All);
        yield return new WaitForSeconds(3);
        pv.RPC("UnPressButton", RpcTarget.All);
        pv.RPC("CooldownFalse", RpcTarget.All);

    }
    [PunRPC]
    void CloseBigDoors(bool b)
    {
        bigDoorNumClose = b;
    }
    [PunRPC]
    void PlayDing()
    {
        if (ring == true)
        {
            ring = false;
            ding.Play();
        }
    }
    [PunRPC]
    void OpenBigDoor(int i)
    {
        bigDoorNumOpen = i;
    }
    public void ChangeMasterClientifAvailble()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (PhotonNetwork.PlayerList.Length <= 1)
        {
            return;
        }

        PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
    }
    public override void OnDisconnected(DisconnectCause cause)
    {

    }
}


