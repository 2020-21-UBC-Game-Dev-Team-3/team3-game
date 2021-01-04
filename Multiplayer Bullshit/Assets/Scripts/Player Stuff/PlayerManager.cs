using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;


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
    }

}
