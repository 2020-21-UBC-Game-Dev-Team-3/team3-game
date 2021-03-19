using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleRandomizer : MonoBehaviour {
  private bool hasLoadedGame;

  private int maxImposterNum = 2;
  private int maxCrewmateNum = 8;
  private int currImposterNum;
  private int currCrewmateNum;
  private List<int> playersLeftWithRole = new List<int>();
  private PhotonView pv;
  private RoomManager roomManager;

  public float imposterPercentScaler;
  [HideInInspector] public int numberOfPlayersAddedSoFar;
  [HideInInspector] public List<Player> playerList;

  List<string> availableImposterRoles = new List<string>();
  List<string> availableCrewmateRoles = new List<string>();

  void Start() {
    roomManager = FindObjectOfType<RoomManager>().GetComponent<RoomManager>();
    pv = GetComponent<PhotonView>();
    availableImposterRoles.AddRange(new string[] { "Assassin", "Chameleon", "Trapper" });
  }

  //void Update() {
  //  if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name == "Gaming" && !hasLoadedGame && numberOfPlayersAddedSoFar == roomManager.maxNumberOfPlayers) {
  //    hasLoadedGame = true;
  //    LoadGame();
  //  }
  //}

  public void LoadGame() {
    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
      if (currCrewmateNum < maxCrewmateNum) pv.RPC("RandomizePlayerRole", PhotonNetwork.PlayerList[i], i);
      else pv.RPC("FillInImposters", PhotonNetwork.PlayerList[i], i);
    }
  }

  [PunRPC]
  private void RandomizePlayerRole(int index) {
    Role role = GameObject.FindGameObjectWithTag("Player").GetComponent<Role>();
    if (currImposterNum < maxImposterNum) {
      if (Random.Range(0, 100) >= (100 - roomManager.currPercentForImposter))
        pv.RPC("FillInImposters", PhotonNetwork.PlayerList[index], index);
      else
        pv.RPC("FillInCrewmates", PhotonNetwork.PlayerList[index], index);
    } else pv.RPC("FillInCrewmates", PhotonNetwork.PlayerList[index], index);

  }

  private void SetupListOfPlayers(Player[] players) {
    for (int i = 0; i < players.Length; i++) {
      playersLeftWithRole.Add(i);
    }
  }

  [PunRPC]
  private void FillInImposters(int index) {
    Role role = GameObject.FindGameObjectWithTag("Player").GetComponent<Role>();
    role.currRole = Role.Roles.Imposter;
    //role.currPercentForImposter -= imposterPercentScaler;
    roomManager.currPercentForImposter -= imposterPercentScaler;
    Debug.Log("Player imposter percentage with imposter: " + roomManager.currPercentForImposter);
    pv.RPC("IncrementRoleNumbers", RpcTarget.All, true);
    pv.RPC("AdjustRoleText", PhotonNetwork.PlayerList[index], true);

    switch (availableImposterRoles[Random.Range(0, availableImposterRoles.Count)]) {
      case "Assassin":
        if (!availableImposterRoles.Contains("Assassin")) return;
        role.subRole = Role.Roles.Assassin;
        Debug.Log("You're an assassin");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
        pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Assassin");
        Debug.Log(availableImposterRoles.Contains("Assassin"));
        break;

      case "Chameleon":
        if (!availableImposterRoles.Contains("Chameleon")) return;
        role.subRole = Role.Roles.Chameleon;
        Debug.Log("You're a chameleon");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
        pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Chameleon");
        Debug.Log(availableImposterRoles.Contains("Chameleon"));
        break;

      case "Trapper":
        if (!availableImposterRoles.Contains("Trapper")) return;
        role.subRole = Role.Roles.Trapper;
        Debug.Log("You're a trapper");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
        pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Trapper");
        Debug.Log(availableImposterRoles.Contains("Trapper"));
        break;

      default:
        Debug.Log("You're a dingus bingus");
        break;
    }
  }

  [PunRPC]
  private void FillInCrewmates(int index) {
    Role role = GameObject.FindGameObjectWithTag("Player").GetComponent<Role>();
    role.currRole = Role.Roles.Crewmate;
    //role.currPercentForImposter += imposterPercentScaler;
    roomManager.currPercentForImposter += imposterPercentScaler;
    Debug.Log("Player imposter percentage with crewmate: " + roomManager.currPercentForImposter);
    pv.RPC("IncrementRoleNumbers", RpcTarget.All, false);
    pv.RPC("AdjustRoleText", PhotonNetwork.PlayerList[index], false);
  }

  [PunRPC]
  void RemoveImposterRoleFromList(string imposterRole) {
    availableImposterRoles.Remove(imposterRole);
    foreach (var x in availableImposterRoles) Debug.Log(x);
  }

  [PunRPC]
  private void AdjustRoleText(bool isImposter) {
    TextMeshProUGUI roleUI = Camera.main.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    if (isImposter) {
      roleUI.text = "Imposter";
      roleUI.color = Color.red;
    } else {
      roleUI.text = "Crewmate";
      roleUI.color = Color.cyan;
    }
  }


  [PunRPC]
  private void IncrementRoleNumbers(bool isImposter) {
    if (isImposter) {
      currImposterNum++;
      GetComponent<GameManager>().imposters++;
    } else {
      currCrewmateNum++;
      GetComponent<GameManager>().crewmates++;
    }

/*    if ((currCrewmateNum + currImposterNum) == (maxCrewmateNum + maxImposterNum)) {
      GetComponent<GameManager>().InitiateRoleAbilityAssignment();
    }*/
  }
}
