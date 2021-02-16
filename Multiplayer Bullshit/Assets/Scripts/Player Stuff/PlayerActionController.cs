using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerActionController : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject emergencyMeetingImage;
    [SerializeField] GameObject bodyReportedImage;

    MapManager mm;

    Interactable interactable = null;

    public PhotonView pv;

    PlayerManager pm;

    RoleAbility ability;

    private bool inVent = false;
    public Transform Vent1Pos;
    public Transform Vent2Pos;
    public Transform Vent3Pos;
    private float threshold = 1.0f; //magic number but it works and idk what else to do

    void Awake()
    {
        mm = GetComponent<MapManager>();
        pv = GetComponent<PhotonView>();
        pm = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        ability = GetComponent<AssassinAbility>();

        Vent1Pos = GameObject.Find("Vent1Pos").transform;
        Vent2Pos = GameObject.Find("Vent2Pos").transform;
        Vent3Pos = GameObject.Find("Vent3Pos").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return;

        Interact();

        if (inVent)
        {
            CurrentlyInVent();
        }

        if (Input.GetKeyDown("q"))
        {
            ability.InitiateAbility();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interactable"))
        {
            interactable = other.gameObject.GetComponent<Interactable>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            interactable = null;
        }
    }

    void Interact()
    {
        if (Input.GetKeyDown("e"))
        {
            if (!interactable.transform.GetChild(0).gameObject.activeInHierarchy) return;
            ChooseInteractionEvent(interactable);
        }
    }

    void ChooseInteractionEvent(Interactable interactable)
    {
        switch (interactable.interactableName)
        {
            case "Emergency button":
                pv.RPC("TurnOnEmergencyPopUp", RpcTarget.All, "Emergency meeting");
                break;

            case "Dead body":
                pv.RPC("TurnOnEmergencyPopUp", RpcTarget.All, "Body reported");
                break;

            case "Vent":
                EnterVent(interactable);
                break;

            default:
                Debug.Log("not applicable");
                break;
        }
    }


    [PunRPC]
    public void TurnOnEmergencyPopUp(string eventName)
    {
        if (eventName == "Emergency meeting")
        {
            StartCoroutine(ShowEmergencyPopUp(emergencyMeetingImage));
        } else if (eventName == "Body reported")
        {
            StartCoroutine(ShowEmergencyPopUp(bodyReportedImage));
        }
    }

    IEnumerator ShowEmergencyPopUp(GameObject eventImage)
    {
        eventImage.SetActive(true);
        yield return new WaitForSeconds(2);
        eventImage.SetActive(false);
    }

    public void TakeHit() => pv.RPC("RPC_TakeHit", RpcTarget.All);

    [PunRPC]
    public void RPC_TakeHit()
    {
        if (!pv.IsMine) return;
        mm.ResetMap();
        pm.Die();
    }

    void CurrentlyInVent()
    {
        if (Input.GetKeyDown("space"))
        {
            if ((transform.position - Vent1Pos.transform.position).sqrMagnitude < threshold)
            {
                transform.position = Vent2Pos.transform.position;
            }
            else if ((transform.position - Vent2Pos.transform.position).sqrMagnitude < threshold)
            {
                transform.position = Vent3Pos.transform.position;
            }
            else if ((transform.position - Vent3Pos.transform.position).sqrMagnitude < threshold)
            {
                transform.position = Vent1Pos.transform.position;
            }

        }
        else if (Input.GetKeyDown("a") || Input.GetKeyDown("w") || Input.GetKeyDown("d") || Input.GetKeyDown("s"))
        {
            pv.RPC("stopInvis", RpcTarget.All);
            inVent = false;
        }
    }

    void EnterVent(Interactable interactable)
    {
        inVent = true;
        transform.position = interactable.transform.GetChild(1).gameObject.transform.position;
        pv.RPC("turnInvis", RpcTarget.All);

    }

    [PunRPC]
    void turnInvis()//Interactable interactable)
    {

        //transform.position = interactable.transform.GetChild(1).gameObject.transform.position;
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
    }
    [PunRPC]
    void stopInvis()//Interactable interactable)
    {

        //transform.position = interactable.transform.GetChild(1).gameObject.transform.position;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);
    }

}
