using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class VotingManager : MonoBehaviour
{
    public ExitGames.Client.Photon.Hashtable PlayerVoteProperty = new ExitGames.Client.Photon.Hashtable();
    private bool isTiedInVotes;
    //private bool isDead;
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
    public AudioClip[] randomSounds;
    public AudioSource thisSource;
    public List<VotingManager> votingManagers;
    public bool gotVotingManagers = false;
    private void Awake()
    {
        // playersAllowedToVote = new List<Player>(PhotonNetwork.PlayerList);
        //Debug.Log("Vote count: " + playersAllowedToVote.Count);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pv = GetComponent<PhotonView>();

    }

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            meetingAudios.Add(p.transform.Find("MeetingAudio").GetComponent<AudioSource>());
        }
        foreach (GameObject p in players)
        {
            bgmAudios.Add(p.transform.Find("PlayerMusic").GetComponent<AudioSource>());
        }
    }

    public void SetupVoting()
    {
        PlayerVoteProperty.Add("VotesR", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerVoteProperty);
        Debug.Log(pv.Owner.NickName + "This is the nickname of pv owner");
        Debug.Log(pv.Owner.ActorNumber + "This is the actornumber of pv owner");

        pv.RPC("PlayTheStupidSounds", RpcTarget.All);
        GameObject[] gameobjects = GameObject.FindGameObjectsWithTag("Player");
        if (gotVotingManagers == false)
        {
            gotVotingManagers = true;
            foreach (GameObject go in gameobjects)
            {
                if (go.transform.Find("VoteCanvas/VotingManager").GetComponent<VotingManager>() != null)
                {
                    votingManagers.Add(go.transform.Find("VoteCanvas/VotingManager").GetComponent<VotingManager>());
                }
            }
        }
        for (int i = 0; i < playerBoxes.transform.childCount; i++)
        {
            playerVotingSections.Add(playerBoxes.transform.GetChild(i).gameObject, false);
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {

            bool isPlayerAllowedToVote = gameManager.playersAllowedToVote.Contains(player);
            Debug.Log(player.NickName + " is allowed to vote: " + isPlayerAllowedToVote);
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
                        pv.RPC("UserHasVoted", player);
                    }
                    section.Key.SetActive(true);
                    break;
                }
                else Debug.Log("COULD NOT SETUP SECTION FOR: " + player.NickName);
            }
        }
    }

    public void ClearBodies()
    {
        DeadBody[] bodies = FindObjectsOfType<DeadBody>();
        foreach (var body in bodies)
        {
            PhotonNetwork.Destroy(body.gameObject);
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
        skipBoxInteractButton.SetActive(false);
    }

    [PunRPC]
    private void SetIconsForVoting()
    {
        int votesRecieved = (int)PhotonNetwork.LocalPlayer.CustomProperties["VotesR"];
        votesRecieved++;
        PlayerVoteProperty.Remove("VotesR");
        PlayerVoteProperty.Add("VotesR", votesRecieved);
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerVoteProperty);
        Debug.Log((int)PhotonNetwork.LocalPlayer.CustomProperties["VotesR"] + " name : " + PhotonNetwork.LocalPlayer.NickName);
    }

    [PunRPC]
    private void SkipVoteSelected()
    {
       // numOfSkipVotes++;

    }

    [PunRPC]
    private void CompareVotes()
    {
        currNumOfHighestVotes = 0;
        playerWithHighestVotes = null;
        foreach (VotingManager vm in votingManagers)
        {
            foreach (VotingManager dm in votingManagers)
            {

                if (vm != null && dm != null) {
                    PhotonView vmpv = vm.GetComponentInParent<PhotonView>();
                    PhotonView dmpv = dm.GetComponentInParent<PhotonView>();
                    Debug.Log(vmpv.Owner.NickName + " and " + dmpv.Owner.NickName);
                    if ((int)vmpv.Owner.CustomProperties["VotesR"] > (int)dmpv.Owner.CustomProperties["VotesR"] && dmpv.Owner.ActorNumber != vmpv.Owner.ActorNumber)
                    {
                        Debug.Log((int)vmpv.Owner.CustomProperties["VotesR"]+ " and " + (int)vmpv.Owner.CustomProperties["VotesR"]);
                        isTiedInVotes = false;
                        playerWithHighestVotes = vmpv.Owner;
                        currNumOfHighestVotes = (int)vmpv.Owner.CustomProperties["VotesR"];
                        Debug.Log(currNumOfHighestVotes + " is the amount of highest votes so far, his name is" + playerWithHighestVotes.NickName);
                    }
                    else if ((int)vmpv.Owner.CustomProperties["VotesR"] == (int)dmpv.Owner.CustomProperties["VotesR"] && dmpv.Owner.ActorNumber != vmpv.Owner.ActorNumber)
                    {
                        
                        Debug.Log("There is a tie");
                        isTiedInVotes = true;
                    }
                }
            }
        }
        pv.RPC("UpdatePlayerWithHighestVotes", RpcTarget.Others, currNumOfHighestVotes, playerWithHighestVotes, isTiedInVotes);
    }
    [PunRPC]
    void UpdatePlayerWithHighestVotes(int num, Player player, bool tied)
    {
        foreach (VotingManager vm in votingManagers)
        {
            vm.currNumOfHighestVotes = num;
            vm.playerWithHighestVotes = player;
            vm.isTiedInVotes = tied;
        }
        pv.RPC("EndVoting", RpcTarget.All);
    }

    [PunRPC]
    private void IsVotingFinished()
    {
        numOfPlayersVotedSoFar++;
        if (numOfPlayersVotedSoFar >= gameManager.playersAllowedToVote.Count)
        {
            pv.RPC("CompareVotes", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void EndVoting()
    {
        StartCoroutine("VotingResults");

    }

    private IEnumerator VotingResults()
    {
        pv.RPC("ShowSkipResults", RpcTarget.All, numOfSkipVotes, true);
        pv.RPC("ShowVotingResults", RpcTarget.All, numOfPlayersVotingForYou, System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer), true);

        yield return new WaitForSeconds(4f);
        PlayMakerFSM.BroadcastEvent("GlobalTurnMovementOn");
        yield return new WaitForSeconds(1f);
        foreach (AudioSource audio in meetingAudios)
        {
            if (audio != null)
            {
                audio.Stop();
            }
        }
        foreach (AudioSource audio in bgmAudios)
        {
            if (audio != null)
            {
                audio.Play();
            }
        }
        if (!isTiedInVotes && currNumOfHighestVotes > numOfSkipVotes && PhotonNetwork.IsMasterClient)
        {
            pv.RPC("KillPlayerWithHighestVotes", RpcTarget.All, playerWithHighestVotes);
        }
        gameObject.SetActive(false);
    }

    [PunRPC]
    private void ShowVotingResults(int numOfVotes, int playerIndex, bool isVotingEnabled)
    {
        GameObject playerIcons = playerBoxes.transform.GetChild(playerIndex).Find("PlayerIconsArea").gameObject;
        for (int i = 0; i < numOfVotes; i++)
            playerIcons.transform.GetChild(i).gameObject.SetActive(isVotingEnabled);
    }

    [PunRPC]
    private void ShowSkipResults(int numOfVotes, bool isVotingEnabled)
    {
        for (int i = 0; i < numOfVotes; i++)
            skipBoxIconArea.transform.GetChild(i).gameObject.SetActive(isVotingEnabled);
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
        Debug.Log("only one of the clients should see this");
        transform.parent.GetComponentInParent<MapManager>().ResetMap();
        transform.parent.GetComponentInParent<MinigameManager>().ResetTaskList();
        PlayerManager[] playerManagers = FindObjectsOfType<PlayerManager>();
        for (int i = 0; i < playerManagers.Length; i++)
        {
            if (playerManagers[i].GetComponent<PhotonView>().IsMine)
            {
                playerManagers[i].GetVotedOff();
                return;
            }
        }
    }

    [PunRPC]
    void PlayTheStupidSounds()
    {
        thisSource.volume = PlayerPrefs.GetFloat("main volume");
        thisSource.clip = randomSounds[Random.Range(0, 3)];
        thisSource.Play();
    }

    public void OnEnable()
    {
        /*    transform.parent.GetComponentsInParent<PlayMakerFSM>()[2].enabled = false;*/
        ClearBodies();
        SetupVoting();
        Debug.Log("Is minigame interrupt true NOW: " + transform.parent.GetComponentInParent<PlayerActionController>().minigameInterrupt);
    }

    public void OnDisable()
    {
        pv.RPC("ShowSkipResults", RpcTarget.All, numOfSkipVotes, false);
        pv.RPC("ShowVotingResults", RpcTarget.All, numOfPlayersVotingForYou, System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer), false);
        /*    transform.parent.GetComponentsInParent<PlayMakerFSM>()[2].enabled = true;*/
        for (int i = 0; i < playerBoxes.transform.childCount; i++)
            playerBoxes.transform.GetChild(i).Find("InteractButton").gameObject.SetActive(true);
        skipBoxInteractButton.SetActive(true);
        playerVotingSections.Clear();
        isTiedInVotes = false;
        numOfPlayersVotedSoFar = 0;
        numOfPlayersVotingForYou = 0;
        currNumOfHighestVotes = 0;
        numOfSkipVotes = 0;
        PhotonNetwork.LocalPlayer.CustomProperties.Remove("VotesR");
        //if (!isDead)
        //{
        //    PlayerActionController pac = transform.parent.GetComponentInParent<PlayerActionController>();
        //    pac.isCurrentlyVoting = false;
        //    pac.hasVotingCooldownRunning = true;
        //    pac.votingCooldownTimer = pac.votingCooldown;
        //}

        PlayerActionController pac = transform.parent.GetComponentInParent<PlayerActionController>();
        if (pac != null)
        {
            pac.isCurrentlyVoting = false;
            pac.hasVotingCooldownRunning = true;
            pac.votingCooldownTimer = pac.votingCooldown;
        }
        RestartAbilityCooldown();
    }

    void RestartAbilityCooldown()
    {
        Role.Roles playerSubRole = transform.parent.GetComponentInParent<Role>().subRole;
        Debug.Log("before cooldown is restarted");
        if (playerSubRole != null)
        {
            switch (playerSubRole)
            {
                case Role.Roles.Assassin:
                    transform.parent.GetComponentInParent<AssassinAbility>().RestartCooldown();
                    break;

                case Role.Roles.Chameleon:
                    transform.parent.GetComponentInParent<ChameleonAbility>().RestartCooldown();
                    break;

                case Role.Roles.Trapper:
                    transform.parent.GetComponentInParent<TrapAbility>().RestartCooldown();
                    break;
            }
        }
    }
}