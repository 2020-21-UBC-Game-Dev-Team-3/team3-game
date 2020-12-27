using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class NameTextScript : MonoBehaviour
{
    PhotonView pv;
    private Camera mainCamera;
    private Transform mainCameraTransform;
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCameraTransform = Camera.main.transform;
        if (pv.IsMine)
        {
            SetName();
        }
        else SetOwnerName();
    }
    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    [PunRPC]
    private void SetOwnerName() => text.text = pv.Owner.NickName;
    [PunRPC]
    private void SetName() => text.text = PhotonNetwork.NickName;
}
