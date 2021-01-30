using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerActionController : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject emergencyMeetingEvent;

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
            if (!interactable.indicator.activeSelf) return;
            ChooseInteractionEvent(interactable);
        }
    }

    void ChooseInteractionEvent(Interactable interactable)
    {
        if (interactable.GetInteractableName() == "Emergency button" || interactable.GetInteractableName() == "Dead body")
        {
            pv.RPC("TurnOnEmergencyPopUp", RpcTarget.All);
        }
    }

    [PunRPC]
    public void TurnOnEmergencyPopUp() => StartCoroutine(ShowEmergencyPopUp());

    IEnumerator ShowEmergencyPopUp()
    {
        emergencyMeetingEvent.SetActive(true);
        yield return new WaitForSeconds(2);
        emergencyMeetingEvent.SetActive(false);
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
