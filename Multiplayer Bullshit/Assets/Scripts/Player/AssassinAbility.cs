using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            if(hit.collider.CompareTag("Player"))
            {
                pv.RPC("KnifeTravel", RpcTarget.All, hit.point);
            }
        }

        yield return StartCoroutine(InitiateCooldown());
    }

    [PunRPC]
    public void KnifeTravel(Vector3 position)
    {
        shurikenObject = Instantiate(shurikenPrefab, shurikenTransform.position, shurikenPrefab.transform.rotation);
        shurikenObject.GetComponent<Shuriken>().shurikenLaunched = true;
        shurikenObject.GetComponent<Shuriken>().target = position;

        
        //Debug.Log("knife sent successfully");
    }

}
