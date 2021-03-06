using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ElevatorInteraction : MonoBehaviour
{
    public GameObject floor1Button, floor2Button, floor3Button;
    public GameObject callElevatorButton1, callElevatorButton2, callElevatorButton3;
    public GameObject Elevator;
    public GameObject ElevatorStation;
    private float distToElevator;
    private string ElevatorCallName;
    private float distToElevatorCall;
    private PhotonView pv;

    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        Elevator = GameObject.FindGameObjectWithTag("Elevator");
        floor1Button = GameObject.Find("Floor1Button");
        floor2Button = GameObject.Find("Floor2Button");
        floor3Button = GameObject.Find("Floor3Button");
        callElevatorButton1 = GameObject.Find("CallFloor1Button");
        callElevatorButton2 = GameObject.Find("CallFloor2Button");
        callElevatorButton3 = GameObject.Find("CallFloor3Button");
        ElevatorStation = FindClosestStation();
        ElevatorCallName = FindClosestStation().name;
        ElevatorButtonsOff();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            distToElevator = Vector3.Distance(transform.position, Elevator.transform.position);
            if (distToElevator <= 1)
            {
                ElevatorButtonsOn();
            }
            else if (distToElevator > 1)
            {
                ElevatorButtonsOff();
            }
            ElevatorStation = FindClosestStation();
            ElevatorCallName = ElevatorStation.name;
            distToElevatorCall = Vector3.Distance(transform.position, ElevatorStation.transform.position);
            if (distToElevatorCall <= 2)
            {
                switch (ElevatorCallName)
                {
                    case "CallFloor1":
                        CallElevatorButtonOn1();
                        break;
                    case "CallFloor2":
                        CallElevatorButtonOn2();
                        break;
                    case "CallFloor3":
                        CallElevatorButtonOn3();
                        break;
                    default:
                        Debug.Log("What?");
                        break;
                }
            }
            else if (distToElevatorCall > 2)
            {
                CallElevatorButtonOff1();
                CallElevatorButtonOff2();
                CallElevatorButtonOff3();
            }
        }
    }
    void ElevatorButtonsOn()
    {
        floor1Button.transform.localPosition = new Vector3(254, -128, 0);
        floor2Button.transform.localPosition = new Vector3(254, -78, 0);
        floor3Button.transform.localPosition = new Vector3(254, -28, 0);
    }
    void ElevatorButtonsOff()
    {
        floor1Button.transform.position = new Vector3(4000, 0, 0);
        floor2Button.transform.position = new Vector3(4000, 0, 0);
        floor3Button.transform.position = new Vector3(4000, 0, 0);
    }
    void CallElevatorButtonOn1()
    {
        callElevatorButton1.transform.localPosition = new Vector3(254, -78, 0);
    }
    void CallElevatorButtonOff1()
    {
        callElevatorButton1.transform.localPosition = new Vector3(4000, -136, 0);
    }
    void CallElevatorButtonOn2()
    {
        callElevatorButton2.transform.localPosition = new Vector3(254, -78, 0);
    }
    void CallElevatorButtonOff2()
    {
        callElevatorButton2.transform.position = new Vector3(4000, -136, 0);
    }

    void CallElevatorButtonOn3()
    {
        callElevatorButton3.transform.localPosition = new Vector3(254, -78, 0);
    }
    void CallElevatorButtonOff3()
    {
        callElevatorButton3.transform.position = new Vector3(4000, -136, 0);
    }

    public GameObject FindClosestStation()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("CallFloor");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
