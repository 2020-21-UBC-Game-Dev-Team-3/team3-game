using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeCullingMask : MonoBehaviour
{
    PhotonView pv;

    [SerializeField] GameObject player;

    Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        pv = player.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return;
        
        cam.cullingMask |= 1 << LayerMask.NameToLayer("Ghost");
    }
}
