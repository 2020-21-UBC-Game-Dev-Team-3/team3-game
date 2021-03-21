using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleRandomizer : MonoBehaviour {
  private bool hasLoadedGame;

/*  private int maxImposterNum;
  private int maxCrewmateNum;
  private float testingNum = 5f; // this is 5f assuming there are 10 players
  private int currImposterNum;
  private int currCrewmateNum;*/
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
    /*    availableImposterRoles.AddRange(new string[] { "Assassin", "Chameleon", "Trapper" });*/
    availableImposterRoles.AddRange(new string[] {"Trapper"});
    availableCrewmateRoles.AddRange(new string[] {"Disarmer"});
/*    maxImposterNum = (int)Mathf.Round(roomManager.maxNumberOfPlayers / testingNum);
    maxCrewmateNum = roomManager.maxNumberOfPlayers - maxImposterNum;*/
  }

  //void Update() {
  //  if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name == "Gaming" && !hasLoadedGame && numberOfPlayersAddedSoFar == roomManager.maxNumberOfPlayers) {
  //    hasLoadedGame = true;
  //    LoadGame();
  //  }
  //}

  public void LoadGame() {
    if (PhotonNetwork.IsMasterClient) {
      // intlist is a random list of integers
      List<int> intList = new List<int>();
      int tempInt;
      for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
        do {
          tempInt = Random.Range(0, PhotonNetwork.PlayerList.Length);
        } while (intList.Contains(tempInt));
        intList.Add(tempInt);
      }
      AssignRoles(intList);
    }
  }

  private void AssignRoles(List<int> randomIntList) {

    if (PhotonNetwork.PlayerList.Length == 1) {
      pv.RPC("FillInImposters", PhotonNetwork.PlayerList[randomIntList[0]], randomIntList[0]);
      return;
    }

    if (PhotonNetwork.PlayerList.Length <= 6) {
      pv.RPC("FillInImposters", PhotonNetwork.PlayerList[randomIntList[0]], randomIntList[0]); // 1st imposter
      pv.RPC("FillInDisarmers", PhotonNetwork.PlayerList[randomIntList[1]], randomIntList[1]); // 1st disarmer
      for (int i = 2; i < PhotonNetwork.PlayerList.Length; i++) {
        pv.RPC("FillInCrewmates", PhotonNetwork.PlayerList[randomIntList[i]], randomIntList[i]); // the rest of crewmates
      }
    } else {
      pv.RPC("FillInImposters", PhotonNetwork.PlayerList[randomIntList[0]], randomIntList[0]); // 1st imposter
      pv.RPC("FillInImposters", PhotonNetwork.PlayerList[randomIntList[1]], randomIntList[1]); // 2nd imposter
      pv.RPC("FillInDisarmers", PhotonNetwork.PlayerList[randomIntList[2]], randomIntList[2]); // 1st disarmer
      for (int i = 3; i < PhotonNetwork.PlayerList.Length; i++) {
        pv.RPC("FillInCrewmates", PhotonNetwork.PlayerList[randomIntList[i]], randomIntList[i]); // the rest of crewmates
      }
    }
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
/*    pv.RPC("IncrementRoleNumbers", RpcTarget.All, true);*/
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
    pv.RPC("AdjustRoleText", PhotonNetwork.PlayerList[index], false);

    role.subRole = Role.Roles.None;
    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
  }

  [PunRPC]
  private void FillInDisarmers(int index) {
    Role role = GameObject.FindGameObjectWithTag("Player").GetComponent<Role>();
    role.currRole = Role.Roles.Crewmate;
    pv.RPC("AdjustRoleText", PhotonNetwork.PlayerList[index], false);

    role.subRole = Role.Roles.Disarmer;
    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
  }

  [PunRPC]
  void RemoveImposterRoleFromList(string imposterRole) {
    availableImposterRoles.Remove(imposterRole);
  }

  [PunRPC]
  void RemoveCrewmateRoleFromList(string crewmateRole) {
    Debug.Log("SDFLKSDJFLKDSD FKLSDKJLF REMOVING DISARMER LLALALALLALALALA");
    availableCrewmateRoles.Remove(crewmateRole);
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


/*  [PunRPC]
  private void IncrementRoleNumbers(bool isImposter) {
    if (isImposter) {
      currImposterNum++;
      GetComponent<GameManager>().imposters++;
    } else {
      currCrewmateNum++;
      GetComponent<GameManager>().crewmates++;
    }

    if ((currCrewmateNum + currImposterNum) == (maxCrewmateNum + maxImposterNum)) {
      GetComponent<GameManager>().InitiateRoleAbilityAssignment();
    }
  }*/
}
