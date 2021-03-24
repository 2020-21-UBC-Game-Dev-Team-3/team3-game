using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VotingManager : MonoBehaviour {
  private bool isTiedInVotes;
  //private int numOfPlayersNeededToVote;
  private int numOfSkipVotes;
  private int numOfPlayersVotedSoFar;
  private int numOfPlayersVotingForYou;
  private Dictionary<GameObject, bool> playerVotingSections = new Dictionary<GameObject, bool>();
  private Player playerWithHighestVotes;
  private int currNumOfHighestVotes;
  PhotonView pv;
  //[SerializeField] private PlayerMovementController playerMovementController;
  [SerializeField] private GameObject skipBoxInteractButton;
  [SerializeField] private GameObject skipBoxIconArea;
  [SerializeField] private GameObject playerBoxes;
  //    [HideInInspector] public List<Player> playersAllowedToVote;
  //  private List<Player> playersNotAllowedToVote;
  public List<AudioSource> meetingAudios;
  public List<AudioSource> bgmAudios;
  private GameObject[] players;
  private GameManager gameManager;

  private void Awake() {
    // playersAllowedToVote = new List<Player>(PhotonNetwork.PlayerList);
    //Debug.Log("Vote count: " + playersAllowedToVote.Count);
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    pv = GetComponent<PhotonView>();

  }

  void Start() {
    players = GameObject.FindGameObjectsWithTag("Player");
    foreach (GameObject p in players) {
      meetingAudios.Add(p.transform.Find("MeetingAudio").GetComponent<AudioSource>());
    }
    foreach (GameObject p in players) {
      bgmAudios.Add(p.transform.Find("PlayerMusic").GetComponent<AudioSource>());
    }
  }

  public void SetupVoting() {
    ClearBodies();
    for (int i = 0; i < playerBoxes.transform.childCount; i++) {
      playerVotingSections.Add(playerBoxes.transform.GetChild(i).gameObject, false);
    }
    foreach (Player player in PhotonNetwork.PlayerList) {
      bool isPlayerAllowedToVote = gameManager.playersAllowedToVote.Contains(player);
      Debug.Log(player.NickName + " is allowed to vote: " + isPlayerAllowedToVote);
      foreach (KeyValuePair<GameObject, bool> section in playerVotingSections) {
        if (!section.Value) {
          playerVotingSections[section.Key] = true;
          section.Key.GetComponent<VoteSection>().SetupVoteSection(player);
          if (!isPlayerAllowedToVote) {
            section.Key.transform.Find("InteractButton").gameObject.SetActive(false);
            section.Key.transform.Find("DeadMan").gameObject.SetActive(true);
            pv.RPC("UserHasVoted", player);

          }
          section.Key.SetActive(true);
          break;
        } else Debug.Log("COULD NOT SETUP SECTION FOR: " + player.NickName);
      }
    }
  }

  public void ClearBodies() {
    DeadBody[] bodies = FindObjectsOfType<DeadBody>();
    foreach (var body in bodies) {
      PhotonNetwork.Destroy(body.gameObject);
    }
  }

  public void ConfirmVoteForPlayer() {
    GameObject votingBox = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
    pv.RPC("UserHasVoted", PhotonNetwork.LocalPlayer);
    votingBox.SetActive(false);
    if (votingBox.transform.parent.parent.name == "SkipBox") pv.RPC("SkipVoteSelected", RpcTarget.All);
    else {
      Player player = PhotonNetwork.PlayerList[votingBox.transform.parent.transform.parent.gameObject.GetComponent<VoteSection>().playerInSectionIndex];
      // int playerNum = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
      pv.RPC("SetIconsForVoting", player);
    }
    pv.RPC("IsVotingFinished", RpcTarget.MasterClient);
  }

  public void ExitVoteForPlayer() {
    EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
  }

  public void ShowVotingIcons() {
    EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.SetActive(true);
  }

  [PunRPC]
  public void UserHasVoted() {
    for (int i = 0; i < playerBoxes.transform.childCount; i++)
      playerBoxes.transform.GetChild(i).Find("InteractButton").gameObject.SetActive(false);
    skipBoxInteractButton.SetActive(false);
  }

  [PunRPC]
  private void SetIconsForVoting() {
    numOfPlayersVotingForYou++;

  }

  [PunRPC]
  private void SkipVoteSelected() {
    numOfSkipVotes++;

  }

  [PunRPC]
  private void CompareVotes() {
    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
      pv.RPC("CompareTwoPlayers", PhotonNetwork.PlayerList[i], i);
    }
  }

  [PunRPC]
  private void CompareTwoPlayers(int playerIndex) {
    if (numOfPlayersVotingForYou > currNumOfHighestVotes)
      pv.RPC("SetToLargestList", RpcTarget.All, numOfPlayersVotingForYou, playerIndex, false);
    else if (numOfPlayersVotingForYou == currNumOfHighestVotes)
      pv.RPC("SetToLargestList", RpcTarget.All, currNumOfHighestVotes, playerIndex, true);
  }

  [PunRPC]
  private void SetToLargestList(int highestVoteSoFar, int playerIndex, bool isTied) {
    currNumOfHighestVotes = highestVoteSoFar;
    isTiedInVotes = isTied;
    if (!isTiedInVotes) playerWithHighestVotes = PhotonNetwork.PlayerList[playerIndex];
  }

  [PunRPC]
  private void IsVotingFinished() {
    numOfPlayersVotedSoFar++;
    if (numOfPlayersVotedSoFar >= gameManager.playersAllowedToVote.Count) {
      Debug.Log("Compare votes");
      pv.RPC("CompareVotes", RpcTarget.MasterClient);
      Debug.Log("Compare votes has finished");
      pv.RPC("EndVoting", RpcTarget.All);
      Debug.Log("Ending voting has finished");
    }
  }

  [PunRPC]
  private void EndVoting() {
    StartCoroutine("VotingResults");

  }

  private IEnumerator VotingResults() {
    pv.RPC("ShowSkipResults", RpcTarget.All, numOfSkipVotes, true);
    pv.RPC("ShowVotingResults", RpcTarget.All, numOfPlayersVotingForYou, System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer), true);

    yield return new WaitForSeconds(4f);
    PlayMakerFSM.BroadcastEvent("GlobalTurnMovementOn");
    yield return new WaitForSeconds(1f);
    foreach (AudioSource audio in meetingAudios) {
            if (audio != null)
            {
                audio.Stop();
            }
    }
    foreach (AudioSource audio in bgmAudios) {
            if (audio != null)
            {
                audio.Play();
            }
    }
    if (!isTiedInVotes && currNumOfHighestVotes > numOfSkipVotes)
      pv.RPC("KillPlayerWithHighestVotes", RpcTarget.All, playerWithHighestVotes);
    gameObject.SetActive(false);
  }

  [PunRPC]
  private void ShowVotingResults(int numOfVotes, int playerIndex, bool isVotingEnabled) {
    GameObject playerIcons = playerBoxes.transform.GetChild(playerIndex).Find("PlayerIconsArea").gameObject;
    for (int i = 0; i < numOfVotes; i++)
      playerIcons.transform.GetChild(i).gameObject.SetActive(isVotingEnabled);
  }

  [PunRPC]
  private void ShowSkipResults(int numOfVotes, bool isVotingEnabled) {
    for (int i = 0; i < numOfVotes; i++)
      skipBoxIconArea.transform.GetChild(i).gameObject.SetActive(isVotingEnabled);
  }

  [PunRPC]
  private void KillPlayerWithHighestVotes(Player playerToKill) {
    //Use the kill mechanic here to kill player while the scene plays out
    if (PhotonNetwork.LocalPlayer == playerToKill) pv.RPC("KillCurrPlayer", PhotonNetwork.LocalPlayer);

  }

  [PunRPC]
  private void KillCurrPlayer() {
    //  pv.RPC("RemovePlayerFromVoting", RpcTarget.All, PhotonNetwork.LocalPlayer);
    transform.parent.GetComponentInParent<MapManager>().InitiateMapReset();
    foreach (PlayerManager playerManager in FindObjectsOfType<PlayerManager>())
      if (playerManager.GetComponent<PhotonView>().IsMine) playerManager.GetVotedOff();
    /*PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>().GetVotedOff();*/
  }

  [PunRPC]
  public void RemovePlayerFromVoting(Player player) {
    // Debug.Log("Before removing player: " + playersAllowedToVote.Count);
    // PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>().playersAllowedToVote.Remove(player);
    //  Debug.Log("After removing player: " + playersAllowedToVote.Count);
    // playersNotAllowedToVote.Add(PhotonNetwork.LocalPlayer);
  }

  public void OnEnable() {
    //numOfPlayersNeededToVote = playersAllowedToVote.Count;
    SetupVoting();
  }

  public void OnDisable() {
    pv.RPC("ShowSkipResults", RpcTarget.All, numOfSkipVotes, false);
    pv.RPC("ShowVotingResults", RpcTarget.All, numOfPlayersVotingForYou, System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer), false);
    for (int i = 0; i < playerBoxes.transform.childCount; i++)
      playerBoxes.transform.GetChild(i).Find("InteractButton").gameObject.SetActive(true);
    skipBoxInteractButton.SetActive(true);
    playerVotingSections.Clear();
    isTiedInVotes = false;
    //numOfPlayersNeededToVote = 0;
    numOfPlayersVotedSoFar = 0;
    numOfPlayersVotingForYou = 0;
    currNumOfHighestVotes = 0;
    numOfSkipVotes = 0;
  }
}
