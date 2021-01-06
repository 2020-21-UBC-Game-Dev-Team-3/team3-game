using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private Vector3 vec = new Vector3(-43.24f, 0f, -0.17f);
    PhotonView pv;
    public GameObject myNameObject;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(pv.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Vector3 position = new Vector3(0f, 10f, 0f);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "unitychan"), position, Quaternion.identity);
        Debug.Log(PhotonNetwork.NickName);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Elevator"), vec, Quaternion.identity);
    }
}
