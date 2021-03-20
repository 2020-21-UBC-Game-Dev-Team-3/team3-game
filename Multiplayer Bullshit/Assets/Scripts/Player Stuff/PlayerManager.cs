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

    public string playerSkinName;

    //singleton instance of player manager
    public static PlayerManager instanceLocalPM;

    GameObject controller;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine) instanceLocalPM = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (pv.IsMine)
        {
            CreateController();
        }
    }

    Vector3 FindSpawnPoint()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        Vector3 position = gameManager.spawnLocations[0].position;
        return position;
    }

    void CreateController()
    {
        Vector3 position = FindSpawnPoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "player2"), position, Quaternion.identity, 0, new object[] { pv.ViewID });
        pv.RPC("IncrementPlayerNumber", RpcTarget.MasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Elevator"), vec, Quaternion.identity);
        }    
        }

    public void GiveCharacterSkinToController(string skinName)
    {
        pv.RPC("GiveCharacterSkinToControllerRPC", RpcTarget.All, skinName);
    }

    [PunRPC]
    void GiveCharacterSkinToControllerRPC(string skinName)
    {
        playerSkinName = skinName;
        controller.GetComponent<SkinSelect>().SetCharacterSkin();
    }


    public void Die()
    {
        FindObjectOfType<GameManager>().RemovePlayer(controller);
        PhotonNetwork.Destroy(controller);
        CreateDeadBody(controller);
        CreateGhostPlayer(controller);
    }

    public void GetVotedOff()
    {
        FindObjectOfType<GameManager>().RemovePlayer(controller);
        PhotonNetwork.Destroy(controller);
        CreateGhostPlayer(controller);
    }

    void CreateDeadBody(GameObject oldController)
    {
        Vector3 position = oldController.transform.position;
        GameObject deadBodyObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "DeadBody"), position, Quaternion.identity);
      //  deadBodyObject.GetComponent<SkinSelect>().SetCharacterSkin();
    }

    void CreateGhostPlayer(GameObject oldController)
    {
        Vector3 position = new Vector3(oldController.transform.position.x, oldController.transform.position.y + 1f, oldController.transform.position.z);
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GhostPlayer"), position, Quaternion.identity, 0, new object[] { pv.ViewID });
      //  controller.GetComponent<SkinSelect>().SetCharacterSkin();
    }


    [PunRPC]
    void IncrementPlayerNumber()
    {
        GameObject.FindObjectOfType<RoleRandomizer>().numberOfPlayersAddedSoFar++;
    }
}
