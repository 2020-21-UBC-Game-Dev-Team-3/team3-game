using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChameleonAbility : RoleAbility
{
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseAbility() => StartCoroutine(StartChameleon());

    IEnumerator StartChameleon()
    {

        cam.cullingMask |= 1 << LayerMask.NameToLayer("Chameleon");
        pv.RPC("OnChameleon", RpcTarget.All);
        yield return new WaitForSeconds(5);
        pv.RPC("OffChameleon", RpcTarget.All);

    }

    [PunRPC]
    public void OnChameleon()
    {
        gameObject.layer = 15;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "Minimap Indicator")
                child.gameObject.layer = 15;
        }
    }
    [PunRPC]
    public void OffChameleon()
    {
        gameObject.layer = 9;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "Minimap Indicator")
                child.gameObject.layer = 9;
        }
    }

}
