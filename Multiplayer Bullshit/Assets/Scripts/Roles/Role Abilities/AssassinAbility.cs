using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using TMPro;

public class AssassinAbility : RoleAbility
{
    public float assassinRange;

    GameObject shurikenObject;
    [SerializeField] GameObject shurikenPrefab;
    [SerializeField] Transform shurikenTransform;

    Camera cam;
    GameObject reticle;
    Vector3 reticlePosition;
    public GameObject assassinButton;

    PhotonView photonView;

    private void Awake()
    {
        StartCoroutine(InitiateCooldown());
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        cam = Camera.main;
        reticle = GameObject.Find("Assassin Reticle");
        reticlePosition = reticle.GetComponent<RectTransform>().transform.position;
        assassinButton.SetActive(true);
    }

  public override void SetAbilityText() {
    abilityText = GameObject.Find("Main Camera/Ability Text Canvas/AbilityText").GetComponent<TextMeshProUGUI>();
    abilityText.text = "Assassin";
  }

  public override void UseAbility() => StartCoroutine(ThrowShuriken());

    IEnumerator ThrowShuriken()
    {
        Ray ray = cam.ScreenPointToRay(reticlePosition);
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit, assassinRange))
        {
            Debug.Log(hit.collider.gameObject.name);
            
            if(hit.collider.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine) 
                //&& hit.collider.gameObject.GetComponent<Role>().currRole == Role.Roles.Crewmate)
            {
                photonView.RPC("ShurikenTravel", RpcTarget.All, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
                PlayMakerFSM.BroadcastEvent("visualCooldownStart");
                yield return StartCoroutine(InitiateCooldown());
               
            }
        }
    }

    [PunRPC]
    public void ShurikenTravel(int targetID)
    {
        if (!photonView.IsMine) return;
        shurikenObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Assassin Shuriken"), shurikenTransform.position, Quaternion.identity);
        shurikenObject.GetComponent<Shuriken>().target = PhotonView.Find(targetID).transform;
    }

}
