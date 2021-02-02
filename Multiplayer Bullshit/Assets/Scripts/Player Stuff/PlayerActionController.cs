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

    PhotonView pv;

    PlayerManager pm;

    RoleAbility ability;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return;

        Interact();

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

}
