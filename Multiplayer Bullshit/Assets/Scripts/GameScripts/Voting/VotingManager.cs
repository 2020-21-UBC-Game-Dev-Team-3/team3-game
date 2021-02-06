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

    private int numOfPlayersVotedSoFar;
    private Dictionary<GameObject, bool> playerVotingSections = new Dictionary<GameObject, bool>();
    private List<Color> playersVotingForYou = new List<Color>();
    private Player playerWithHighestVotes;
    private int currNumOfHighestVotes;
    PhotonView pv;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private GameObject playerBoxes;
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void SetupVoting()
    {

        for (int i = 0; i < playerBoxes.transform.childCount; i++)
        {
            playerVotingSections.Add(playerBoxes.transform.GetChild(i).gameObject, false);
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            foreach(KeyValuePair<GameObject, bool> section in playerVotingSections)
            {
                if (!section.Value)
                {
                    playerVotingSections[section.Key] = true;
                    Debug.Log("SETUP SECTION");
                    section.Key.GetComponent<VoteSection>().SetupVoteSection(player);
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
        Player player = PhotonNetwork.PlayerList[votingBox.transform.parent.transform.parent.gameObject.GetComponent<VoteSection>().playerInSectionIndex];
        int playerNum = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
        votingBox.SetActive(false);
        pv.RPC("UserHasVoted", PhotonNetwork.LocalPlayer);
        pv.RPC("SetIconsForVoting", player, playerNum);
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
            playerBoxes.transform.GetChild(i).GetComponentInChildren<Button>().gameObject.SetActive(false);
    }

    [PunRPC]
    private void SetIconsForVoting(int playerNum)
    {
        playersVotingForYou.Add(Color.white);

    }

    [PunRPC]
    private void CompareVotes()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            pv.RPC("CompareTwoPlayers", PhotonNetwork.PlayerList[i], i);
        }
    }

    [PunRPC]
    private void CompareTwoPlayers(int playerIndex)
    {
        if (playersVotingForYou.Count > currNumOfHighestVotes)
            pv.RPC("SetToLargestList", RpcTarget.All, playersVotingForYou.Count, playerIndex, false);
        else if (playersVotingForYou.Count == currNumOfHighestVotes)
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
        if (numOfPlayersVotedSoFar >= PhotonNetwork.PlayerList.Length)
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
        int playerNum = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
        GameObject playerIcons = playerBoxes.transform.GetChild(playerNum).FindChild("PlayerIconsArea").gameObject;
        for (int i = 0; i < playersVotingForYou.Count; i++)
        {
            playerIcons.transform.GetChild(i).GetComponent<Image>().color = playersVotingForYou[i];
            playerIcons.transform.GetChild(i).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(5f);

        for (int i = 0; i < playersVotingForYou.Count; i++) playerIcons.transform.GetChild(i).gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        if (!isTiedInVotes) pv.RPC("KillPlayerWithHighestVotes", RpcTarget.All, playerWithHighestVotes);
        cameraController.enabled = true;
        playerMovementController.enabled = true;
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
        //Replace this with ghost mechanic
        transform.parent.Find("DeadScreen").gameObject.SetActive(true);
    }

    public void OnEnable()
    {
        SetupVoting();
    }
}
