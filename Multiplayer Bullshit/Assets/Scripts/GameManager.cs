using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks {
  int numOfPlayersReady;

  //Spawn room boundaries
  [SerializeField] GameObject invisWall1;
  [SerializeField] GameObject invisWall2;

  //UI that needs to be turned off in beginning
  [SerializeField] GameObject roleTextCanvas;
  [SerializeField] GameObject taskbarCanvas;
  [SerializeField] GameObject minimapCanvas;
  [SerializeField] GameObject taskListCanvas;
  [SerializeField] GameObject readyCanvas;

  // FADE STUFF
  [SerializeField] GameObject crewmateWinScreen;
  [SerializeField] GameObject imposterWinScreen;
  [SerializeField] Image fadeImage;
  [SerializeField] GameObject fadeCanvas;
  public bool isFade = false;
  public bool isBlack = false;
  public bool isCrewmateWin = false;
  public Color color;

  //Character select
  [SerializeField] GameObject characterSelectCanvas;
  [SerializeField] GameObject characterSelectInteractable;
  [SerializeField] GameObject characterSelectTable;
  public List<string> characterSkinNames;

  [SerializeField] Transform[] teleportLocations = new Transform[10]; // assuming this is the max players in a game
  public List<Transform> spawnLocations;
  [HideInInspector] public List<Player> playersAllowedToVote;

    //List<string> availableImposterRoles = new List<string>();
    //List<string> availableCrewmateRoles = new List<string>();

  public int crewmates;
  public int imposters;

  RoomManager roomMan;
  PhotonView pv;

  private void Awake() {
    playersAllowedToVote = new List<Player>(PhotonNetwork.PlayerList);
  }

  void Start() {
    color = fadeCanvas.GetComponentInChildren<Image>().color;
    roomMan = FindObjectOfType<RoomManager>();
    pv = GetComponent<PhotonView>();
    ToggleUI(false);
    //availableImposterRoles.AddRange(new string[] { "Assassin", "Chameleon" });
  }

  private void Update() {
    if (!isFade) return;
    Fade();
/*    Debug.Log(fadeCanvas.GetComponentInChildren<Image>().color);*/
  }

  private void Fade() {
    fadeCanvas.SetActive(true);
    if (isBlack) {
      FadeFromBlack();
    } else {
      FadeUntilBlack();
    }
  }

  private void FadeUntilBlack() {
    if (fadeCanvas.GetComponentInChildren<Image>().color.a < 1) {
      color.a += Time.deltaTime * 0.5f;
      fadeCanvas.GetComponentInChildren<Image>().color = color;
    } else {
      isBlack = true;
      if (isCrewmateWin == true) {
        crewmateWinScreen.SetActive(true);
      } else {
        imposterWinScreen.SetActive(true);
      }
    }
  }

  private void FadeFromBlack() {
    if (fadeCanvas.GetComponentInChildren<Image>().color.a > 0.01f) {
      color.a -= Time.deltaTime * 0.5f;
      fadeCanvas.GetComponentInChildren<Image>().color = color;
    } else {
      isFade = false;
    }
  }

  public void UpdateSpawnLocationList() => pv.RPC("RemoveSpawnLocation", RpcTarget.All);

  [PunRPC]
  void RemoveSpawnLocation() => spawnLocations.RemoveAt(0);

  public void OpenCharacterSelectCanvas() => characterSelectCanvas.SetActive(true);

  public void CloseCharacterSelectCanvas() => characterSelectCanvas.SetActive(false);

  public void AssignCharacterSkin(int index) {
    PlayerManager.instanceLocalPM.GiveCharacterSkinToController(characterSkinNames[index]);
  }

  public void AddToNumberOfPlayersReady() {
    readyCanvas.SetActive(false);
    pv.RPC("IncrementNumberOfPlayersReady", RpcTarget.All);
  }

  [PunRPC]
  void IncrementNumberOfPlayersReady() {
    numOfPlayersReady++;
    if (numOfPlayersReady == roomMan.maxNumberOfPlayers) {
      BeginGame();
    }
  }

  void BeginGame() => pv.RPC("StartGame", RpcTarget.All);

  [PunRPC]
  void StartGame() {
    ToggleUI(true);

    RoleRandomizer roleRan = gameObject.GetComponent<RoleRandomizer>();
    roleRan.LoadGame();
    PlayMakerFSM.BroadcastEvent("LightSabotageFsmStart");
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    foreach (var player in players) {
      if (player.GetComponent<PhotonView>().IsMine) {
        player.GetComponent<MinigameManager>().SetUpMinigameAssignment();
        player.GetComponent<PlayerActionController>().OnStartGame();
      }
    }

    invisWall1.SetActive(false);
    invisWall2.SetActive(false);
    characterSelectInteractable.SetActive(false);
    characterSelectTable.SetActive(false);
  }

  void ToggleUI(bool change) {
    roleTextCanvas.SetActive(change);
    taskbarCanvas.SetActive(change);
    minimapCanvas.SetActive(change);
    taskListCanvas.SetActive(change);
  }

  public void RemovePlayer(GameObject player) {
    if (player.GetComponent<Role>().currRole == Role.Roles.Crewmate) {
      DecrementCrewmates();
    } else
      DecrementImposters();
  }

  public void DecrementCrewmates() {
    crewmates--;
    if (crewmates == 0) {
      Debug.Log("IMPOSTER WIN");
      ImposterWin();
    }
  }

  public void DecrementImposters() {
    imposters--;
    if (imposters == 0) {
      Debug.Log("CREWMATE WIN");
      CrewmateWin();
    }
  }

  public void CrewmateWin() {
    pv.RPC("RPC_CrewmateWin", RpcTarget.All);
  }

  public void ImposterWin() {
    pv.RPC("RPC_ImposterWin", RpcTarget.All);
  }

  public void TeleportPlayers() {
    int count = 0;
    GameObject[] alivePlayers = GameObject.FindGameObjectsWithTag("Player");
    for (int i = 0; i < alivePlayers.Length; i++) {
      alivePlayers[i].gameObject.transform.position = teleportLocations[i].transform.position;
      count++;
    }
    GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
    for (int i = count; i < ghosts.Length; i++) {
      ghosts[i].gameObject.transform.position = teleportLocations[i].transform.position;
    }
  }

  [PunRPC]
  public void RPC_CrewmateWin() {
    StartCoroutine(CrewmateWinCoroutine());
  }

  [PunRPC]
  public void RPC_ImposterWin() {
    StartCoroutine(ImposterWinCoroutine());
  }

  IEnumerator ImposterWinCoroutine() {
    isFade = true;
    yield return new WaitForSeconds(1f);
  }

  IEnumerator CrewmateWinCoroutine() {
    isFade = true;
    isCrewmateWin = true;
    yield return new WaitForSeconds(1f);
  }


  //public void InitiateRoleAbilityAssignment()
  //{
  //    pv.RPC("SetUpRoleAbilityAssignment", RpcTarget.All);
  //}

  //[PunRPC]
  //void SetUpRoleAbilityAssignment()
  //{
  //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

  //    foreach (var x in players) Debug.Log(x.name + " " + x.GetComponent<PhotonView>().ViewID);

  //    List<GameObject> crewmates = new List<GameObject>();
  //    List<GameObject> imposters = new List<GameObject>();

  //    for (int i = 0; i < players.Length; i++)
  //    {
  //        if (players[i].GetComponent<Role>().currRole == Role.Roles.Crewmate)
  //        {
  //            crewmates.Add(players[i]);
  //        }
  //        else
  //        {
  //            imposters.Add(players[i]);
  //        }
  //    }
  //    foreach (var x in imposters) Debug.Log(x.name);

  //    RandomizeImposterSubRoles(imposters);

  //}

  //void RandomizeImposterSubRoles(List<GameObject> imposterList)
  //{
  //    foreach (var imposter in imposterList)
  //    {
  //        if (imposter.GetComponent<PhotonView>().IsMine)
  //        {
  //            switch (availableImposterRoles[0])
  //            {
  //                case "Assassin":
  //                    imposter.GetComponent<Role>().subRole = Role.Roles.Assassin;
  //                    imposter.GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
  //                    pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Assassin");
  //                    break;

  //                case "Chameleon":
  //                    imposter.GetComponent<Role>().subRole = Role.Roles.Chameleon;
  //                    imposter.GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
  //                    pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Chameleon");
  //                    break;

  //                default:
  //                    Debug.Log("You're a dingus bingus");
  //                    break;
  //            }
  //        }

  //    }


  //    for (int i = 0; i < imposterList.Count; i++)
  //    {
  //        Debug.Log(imposterList[i].name + " " + imposterList[i].GetComponent<PhotonView>().ViewID);
  //        Role imposterRole = imposterList[i].GetComponent<Role>();
  //        switch (availableImposterRoles[Random.Range(0, availableImposterRoles.Count - 1)])
  //        {
  //            case "Assassin":
  //                if (!availableImposterRoles.Contains("Assassin")) return;
  //                imposterRole.subRole = Role.Roles.Assassin;
  //                Debug.Log("You're an assassin");
  //                imposterList[i].GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
  //                pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Assassin");
  //                Debug.Log(availableImposterRoles.Contains("Assassin"));
  //                break;

  //            case "Chameleon":
  //                if (!availableImposterRoles.Contains("Chameleon")) return;
  //                imposterRole.subRole = Role.Roles.Chameleon;
  //                Debug.Log("You're a chameleon");
  //                imposterList[i].GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
  //                pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Chameleon");
  //                Debug.Log(availableImposterRoles.Contains("Chameleon"));
  //                break;

  //            default:
  //                Debug.Log("You're a dingus bingus");
  //                break;
  //        }
  //    }
  //}

  //[PunRPC]
  //void RemoveImposterRoleFromList(string imposterRole)
  //{
  //    availableImposterRoles.Remove(imposterRole);
  //    foreach (var x in availableImposterRoles) Debug.Log(x);
  //}



  /*  [HideInInspector] public bool isConnectedToMaster;
    // Start is called before the first frame update

    public IEnumerator EndGame() {
      yield return new WaitForSeconds(20f);
      *//* PhotonNetwork.LeaveRoom();*//* //FOR TESTING
    }

    private void OnLevelWasLoaded(int level) {
      if (SceneManager.GetActiveScene().name == "Gaming")
        StartCoroutine("EndGame");
    }

    public override void OnLeftRoom() {
      if (SceneManager.GetActiveScene().name == "Gaming") {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
      }
    }*/
}
