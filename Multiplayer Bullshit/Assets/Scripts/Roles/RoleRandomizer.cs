using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleRandomizer : MonoBehaviour
{
    private bool hasLoadedGame;

    private int maxImposterNum = 1;
    private int maxCrewmateNum = 1;
    private int currImposterNum;
    private int currCrewmateNum;
    private List<int> playersLeftWithRole = new List<int>();
    private PhotonView pv;
    private RoomManager roomManager;

    public float imposterPercentScaler;
    [HideInInspector] public int numberOfPlayersAddedSoFar;
    [HideInInspector] public List<Player> playerList;

    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>().GetComponent<RoomManager>();
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name == "Gaming" && !hasLoadedGame && numberOfPlayersAddedSoFar == roomManager.maxNumberOfPlayers)
        {
            hasLoadedGame = true;
            LoadGame();
        }
    }
    private void LoadGame()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (currCrewmateNum < maxCrewmateNum) pv.RPC("RandomizePlayerRole", PhotonNetwork.PlayerList[i], i);
            else pv.RPC("FillInImposters", PhotonNetwork.PlayerList[i], i);
        }
    }

    [PunRPC]
    private void RandomizePlayerRole(int index)
    {
        Role role = GameObject.FindGameObjectWithTag("Player").GetComponent<Role>();
        if (currImposterNum < maxImposterNum)
        {
            if (Random.Range(0, 100) >= (100 - roomManager.currPercentForImposter))
                pv.RPC("FillInImposters", PhotonNetwork.PlayerList[index], index);
            else 
                pv.RPC("FillInCrewmates", PhotonNetwork.PlayerList[index], index);
        } else pv.RPC("FillInCrewmates", PhotonNetwork.PlayerList[index], index);

    }

    private void SetupListOfPlayers(Player[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            playersLeftWithRole.Add(i);
        }
    }

    [PunRPC]
    private void FillInImposters(int index)
    {
        Role role = GameObject.FindGameObjectWithTag("Player").GetComponent<Role>();
        role.currRole = Role.Roles.Imposter;
        //role.currPercentForImposter -= imposterPercentScaler;
        roomManager.currPercentForImposter -= imposterPercentScaler;
        Debug.Log("Player imposter percentage with imposter: " + roomManager.currPercentForImposter);
        pv.RPC("IncrementRoleNumbers", RpcTarget.All, true);
        pv.RPC("AdjustRoleText", PhotonNetwork.PlayerList[index], true);
    }

    [PunRPC]
    private void FillInCrewmates(int index)
    {
        Role role = GameObject.FindGameObjectWithTag("Player").GetComponent<Role>();
        role.currRole = Role.Roles.Crewmate;
        //role.currPercentForImposter += imposterPercentScaler;
        roomManager.currPercentForImposter += imposterPercentScaler;
        Debug.Log("Player imposter percentage with crewmate: " + roomManager.currPercentForImposter);
        pv.RPC("IncrementRoleNumbers", RpcTarget.All, false);
        pv.RPC("AdjustRoleText", PhotonNetwork.PlayerList[index], false);
    }

    [PunRPC]
    private void AdjustRoleText(bool isImposter)
    {
        TextMeshProUGUI roleUI = Camera.main.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (isImposter)
        {
            roleUI.text = "Imposter";
            roleUI.color = Color.red;
        } else
        {
            roleUI.text = "Crewmate";
            roleUI.color = Color.cyan;
        }
    }


    [PunRPC]
    private void IncrementRoleNumbers(bool isImposter)
    {
        if (isImposter) currImposterNum++;
        else currCrewmateNum++;
    }
}
