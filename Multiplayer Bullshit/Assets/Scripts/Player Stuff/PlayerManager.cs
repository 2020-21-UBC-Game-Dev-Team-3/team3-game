﻿using System.Collections;
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

    GameObject controller;

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
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "player"), position, Quaternion.identity, 0, new object[] { pv.ViewID });
        Debug.Log(PhotonNetwork.NickName);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Elevator"), vec, Quaternion.identity);
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateDeadBody(controller);
        CreateGhostPlayer(controller);
    }

    void CreateDeadBody(GameObject oldController)
    {
        Vector3 position = oldController.transform.position;
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "DeadBody"), position, Quaternion.identity);
    }

    void CreateGhostPlayer(GameObject oldController)
    {
        Vector3 position = new Vector3(oldController.transform.position.x, oldController.transform.position.y + 1f, oldController.transform.position.z);
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GhostPlayer"), position, Quaternion.identity, 0, new object[] { pv.ViewID });
    }
}
