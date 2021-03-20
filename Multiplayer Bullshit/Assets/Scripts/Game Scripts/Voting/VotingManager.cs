using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VotingManager : MonoBehaviour
{
    private bool isTiedInVotes;
    private int numOfPlayersNeededToVote;
    private int numOfSkipVotes;
    private int numOfPlayersVotedSoFar;
    private int numOfPlayersVotingForYou;
    private Dictionary<GameObject, bool> playerVotingSections = new Dictionary<GameObject, bool>();
    private Player playerWithHighestVotes;
    private int currNumOfHighestVotes;
    PhotonView pv;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private GameObject skipBoxIconArea;
    [SerializeField] private GameObject playerBoxes;
    [SerializeField] private List<Player> playersAllowedToVote;
    private List<AudioSource> meetingAudios;
    private GameObject[] players;
    private void Awake()
    {
        playersAllowedToVote = new List<Player>(PhotonNetwork.PlayerList);
        Debug.Log("Vote count: " + playersAllowedToVote);
    }

    void Start()
    {
        pv = GetComponent<PhotonView>();
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            meetingAudios.Add(p.transform.Find("MeetingAudio").GetComponent<AudioSource>());
        }
    }

    public void SetupVoting()
    {
        for (int i = 0; i < playerBoxes.transform.childCount; i++)
        {
            playerVotingSections.Add(playerBoxes.transform.GetChild(i).gameObject, false);
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            bool isPlayerAllowedToVote = playersAllowedToVote.Contains(player);
            Debug.Log("Player ID: " + player.UserId);
            Debug.Log("Player allowed to vote nickname: " + playersAllowedToVote[0].UserId);
            foreach (KeyValuePair<GameObject, bool> section in playerVotingSections)
            {
                if (!section.Value)
                {
                    playerVotingSections[section.Key] = true;
                    section.Key.GetComponent<VoteSection>().SetupVoteSection(player);
                    if (!isPlayerAllowedToVote)
                    {
                        section.Key.transform.Find("InteractButton").gameObject.SetActive(false);
                        section.Key.transform.Find("DeadMan").gameObject.SetActive(true);
                    }
                    section.Key.SetActive(true);
                    break;
                }
                else Debug.Log("COULD NOT SETUP SECTION FOR: " + player.NickName);
            }
        }
    }

    public void ConfirmVoteForPlayer()
    {
        GameObject votingBox = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        pv.RPC("UserHasVoted", PhotonNetwork.LocalPlayer);
        votingBox.SetActive(false);
        if (votingBox.transform.parent.parent.name == "SkipBox") pv.RPC("SkipVoteSelected", RpcTarget.All);
        else
        {
            Player player = PhotonNetwork.PlayerList[votingBox.transform.parent.transform.parent.gameObject.GetComponent<VoteSection>().playerInSectionIndex];
           // int playerNum = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
            pv.RPC("SetIconsForVoting", player);
        }
        pv.RPC("IsVotingFinished", RpcTarget.MasterClient);
    }

    public void ExitVoteForPlayer()
    {
        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
    }

    public void ShowVotingIcons()
    {
        EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    [PunRPC]
    public void UserHasVoted()
    {
        for (int i = 0; i < playerBoxes.transform.childCount; i++)
            playerBoxes.transform.GetChild(i).Find("InteractButton").gameObject.SetActive(false);
    }

    [PunRPC]
    private void SetIconsForVoting()
    {
        numOfPlayersVotingForYou++;

    }

    [PunRPC]
    private void SkipVoteSelected()
    {
        numOfSkipVotes++;

    }

    [PunRPC]
    private void CompareVotes()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            pv.RPC("CompareTwoPlayers", PhotonNetwork.PlayerList[i], i);
        }
    }

    [PunRPC]
    private void CompareTwoPlayers(int playerIndex)
    {
        if (numOfPlayersVotingForYou > currNumOfHighestVotes)
            pv.RPC("SetToLargestList", RpcTarget.All, numOfPlayersVotingForYou, playerIndex, false);
        else if (numOfPlayersVotingForYou == currNumOfHighestVotes)
            pv.RPC("SetToLargestList", RpcTarget.All, currNumOfHighestVotes, playerIndex, true);
    }

    [PunRPC]
    private void SetToLargestList(int highestVoteSoFar, int playerIndex, bool isTied)
    {
        currNumOfHighestVotes = highestVoteSoFar;
        isTiedInVotes = isTied;
        if (!isTiedInVotes) playerWithHighestVotes = PhotonNetwork.PlayerList[playerIndex];
    }

    [PunRPC]
    private void IsVotingFinished()
    {
        numOfPlayersVotedSoFar++;
        if (numOfPlayersVotedSoFar >= playersAllowedToVote.Count)
        {
            Debug.Log("Compare votes");
            pv.RPC("CompareVotes", RpcTarget.MasterClient);
            Debug.Log("Compare votes has finished");
            pv.RPC("EndVoting", RpcTarget.All);
            Debug.Log("Ending voting has finished");
        }
    }

    [PunRPC]
    private void EndVoting()
    {
        StartCoroutine("VotingResults");

    }

    private IEnumerator VotingResults()
    {
        pv.RPC("ShowSkipResults", RpcTarget.All, numOfSkipVotes);
        pv.RPC("ShowVotingResults", RpcTarget.All, numOfPlayersVotingForYou, System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer));

        yield return new WaitForSeconds(4f);
        PlayMakerFSM.BroadcastEvent("GlobalTurnMovementOn");
        yield return new WaitForSeconds(1f);
        if (!isTiedInVotes && currNumOfHighestVotes > numOfSkipVotes)
            pv.RPC("KillPlayerWithHighestVotes", RpcTarget.All, playerWithHighestVotes);
        this.gameObject.SetActive(false);
    }

    [PunRPC]
    private void ShowVotingResults(int numOfVotes, int playerIndex)
    {
        GameObject playerIcons = playerBoxes.transform.GetChild(playerIndex).Find("PlayerIconsArea").gameObject;
        for (int i = 0; i < numOfVotes; i++)
            playerIcons.transform.GetChild(i).gameObject.SetActive(true);
    }

    [PunRPC]
    private void ShowSkipResults(int numOfVotes)
    {
       for (int i = 0; i < numOfVotes; i++)
            skipBoxIconArea.transform.GetChild(i).gameObject.SetActive(true);
    }

    [PunRPC]
    private void KillPlayerWithHighestVotes(Player playerToKill)
    {
        //Use the kill mechanic here to kill player while the scene plays out
        if (PhotonNetwork.LocalPlayer == playerToKill) pv.RPC("KillCurrPlayer", PhotonNetwork.LocalPlayer);

    }

    [PunRPC]
    private void KillCurrPlayer()
    {
        transform.parent.GetComponentInParent<MapManager>().ResetMap();
        PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>().GetVotedOff();
        playersAllowedToVote.Remove(PhotonNetwork.LocalPlayer);
        foreach(AudioSource audio in meetingAudios)
        {
            audio.Stop();
        }
    }

    public void OnEnable()
    {
        numOfPlayersNeededToVote = playersAllowedToVote.Count;
        SetupVoting();
    }
}
