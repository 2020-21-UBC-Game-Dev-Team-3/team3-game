using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    private Vector3 vec = new Vector3(-43.24f, 0f, -0.17f);
    public List<GameObject> mapFloorPlanes = new List<GameObject>();
    PhotonView pv;
    public GameObject myNameObject;

    public List<string> playerTasksRemaining = new List<string>();
    public List<string> playerTasksCompleted = new List<string>();

    public string playerSkinName = "SChef";

    public Role.Roles assignedRole;

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
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "Elevator"), vec, Quaternion.identity);
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

    public void UpdateTasksOnPlayerManager(string minigame)
    {
        playerTasksRemaining.Remove(minigame);
        playerTasksCompleted.Add(minigame);
    }

    public void Die()
    {
        FindObjectOfType<GameManager>().RemovePlayer(controller);
        pv.RPC("RemovedDeadPlayerFromVoting", RpcTarget.All, PhotonNetwork.LocalPlayer);
        Vector3 oldPosition = controller.transform.position;
        PhotonNetwork.Destroy(controller);
        CreateDeadBody(oldPosition);
        CreateGhostPlayer(oldPosition);
    }

    public void GetVotedOff()
    {
        FindObjectOfType<GameManager>().RemovePlayer(controller);
        pv.RPC("RemovedDeadPlayerFromVoting", RpcTarget.All, PhotonNetwork.LocalPlayer);
        Vector3 oldPosition = controller.transform.position;
        PhotonNetwork.Destroy(controller);
        CreateGhostPlayer(oldPosition);
    }

    void CreateDeadBody(Vector3 position)
    {
        GameObject deadBodyObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "DeadBody"), position, Quaternion.identity);
        //  deadBodyObject.GetComponent<SkinSelect>().SetCharacterSkin();
    }

    void CreateGhostPlayer(Vector3 position)
    {
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GhostPlayer"), position, Quaternion.identity, 0, new object[] { pv.ViewID });
        //controller.GetComponent<SkinSelect>().SetCharacterSkin();
        controller.GetComponent<Role>().currRole = assignedRole;
        controller.GetComponent<PlayerActionController>().OnStartGame();
    }


    [PunRPC]
    void IncrementPlayerNumber()
    {
        GameObject.FindObjectOfType<RoleRandomizer>().numberOfPlayersAddedSoFar++;
    }

    [PunRPC]
    public void RemovedDeadPlayerFromVoting(Player player)
    {
        Debug.Log("Begin removing player now");
        GameObject.Find("GameManager").GetComponent<GameManager>().playersAllowedToVote.Remove(player);
    }
}
