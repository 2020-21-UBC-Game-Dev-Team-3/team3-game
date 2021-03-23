using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class AssassinAbility : RoleAbility
{
    public float assassinRange;

    GameObject shurikenObject;
    [SerializeField] GameObject shurikenPrefab;
    [SerializeField] Transform shurikenTransform;

    Camera cam;
    GameObject reticle;
    Vector3 reticlePosition;

    private void Awake()
    {
        StartCoroutine(InitiateCooldown());
    }

    private void Start()
    {
        cam = Camera.main;
        reticle = GameObject.Find("Assassin Reticle");
        reticlePosition = reticle.GetComponent<RectTransform>().transform.position;
    }

    public override void UseAbility() => StartCoroutine(ThrowShuriken());

    IEnumerator ThrowShuriken()
    {
        Ray ray = cam.ScreenPointToRay(reticlePosition);
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit, assassinRange))
        {
            Debug.Log(hit.collider.gameObject.name);
            
            if(hit.collider.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine 
                && hit.collider.gameObject.GetComponent<Role>().currRole == Role.Roles.Crewmate)
            {
                pv.RPC("ShurikenTravel", RpcTarget.All, hit.collider.gameObject);
                yield return StartCoroutine(InitiateCooldown());
            }
        }
    }

    [PunRPC]
    public void ShurikenTravel(GameObject targetPlayer)
    {
        shurikenObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Assassin Shuriken"), shurikenTransform.position, Quaternion.identity);
        shurikenObject.GetComponent<Shuriken>().target = targetPlayer;
    }

}
