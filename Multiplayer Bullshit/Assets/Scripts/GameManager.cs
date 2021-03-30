using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
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

    private void Awake()
    {
        playersAllowedToVote = new List<Player>(PhotonNetwork.PlayerList);
    }

    void Start()
    {
        color = fadeCanvas.GetComponentInChildren<Image>().color;
        roomMan = FindObjectOfType<RoomManager>();
        pv = GetComponent<PhotonView>();
        ToggleUI(false);
        //availableImposterRoles.AddRange(new string[] { "Assassin", "Chameleon" });
    }

    private void Update()
    {
        if (!isFade) return;
        Fade();
        /*    Debug.Log(fadeCanvas.GetComponentInChildren<Image>().color);*/
    }

    private void Fade()
    {
        fadeCanvas.SetActive(true);
        if (isBlack)
        {
            FadeFromBlack();
        }
        else
        {
            FadeUntilBlack();
        }
    }

    private void FadeUntilBlack()
    {
        if (fadeCanvas.GetComponentInChildren<Image>().color.a < 1)
        {
            color.a += Time.deltaTime * 0.5f;
            fadeCanvas.GetComponentInChildren<Image>().color = color;
        }
        else
        {
            isBlack = true;
            if (isCrewmateWin == true)
            {
                //crewmateWinScreen.SetActive(true);
                SceneManager.LoadScene("Crewmate Victory Screen", LoadSceneMode.Single);
                Debug.Log("game over; crewmates won");
            }
            else
            {
                //imposterWinScreen.SetActive(true);
                SceneManager.LoadScene("Imposter Victory Screen", LoadSceneMode.Single);
                Debug.Log("game over; imposters won");
            }
        }
    }

    private void FadeFromBlack()
    {
        if (fadeCanvas.GetComponentInChildren<Image>().color.a > 0.01f)
        {
            color.a -= Time.deltaTime * 0.5f;
            fadeCanvas.GetComponentInChildren<Image>().color = color;
        }
        else
        {
            isFade = false;
        }
    }

    public void UpdateSpawnLocationList() => pv.RPC("RemoveSpawnLocation", RpcTarget.All);

    [PunRPC]
    void RemoveSpawnLocation() => spawnLocations.RemoveAt(0);

    public void OpenCharacterSelectCanvas() => characterSelectCanvas.SetActive(true);

    public void CloseCharacterSelectCanvas() => characterSelectCanvas.SetActive(false);

    public void AssignCharacterSkin(int index)
    {
        PlayerManager.instanceLocalPM.GiveCharacterSkinToController(characterSkinNames[index]);
    }

    public void AddToNumberOfPlayersReady()
    {
        readyCanvas.SetActive(false);
        pv.RPC("IncrementNumberOfPlayersReady", RpcTarget.All);
    }

    [PunRPC]
    void IncrementNumberOfPlayersReady()
    {
        numOfPlayersReady++;
        if (numOfPlayersReady == roomMan.maxNumberOfPlayers)
        {
            BeginGame();
        }
    }

    void BeginGame() => pv.RPC("StartGame", RpcTarget.All);

    [PunRPC]
    void StartGame()
    {
        ToggleUI(true);
        RoleRandomizer roleRan = gameObject.GetComponent<RoleRandomizer>();
        roleRan.LoadGame();
        PlayMakerFSM.BroadcastEvent("LightSabotageFsmStart");

        invisWall1.SetActive(false);
        invisWall2.SetActive(false);
        characterSelectInteractable.SetActive(false);
        characterSelectTable.SetActive(false);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            player.GetComponent<NameTextScript>().started = true;
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<MinigameManager>().SetUpMinigameAssignment();
                player.GetComponent<PlayerActionController>().OnStartGame();
            }
            StartCoroutine(IncrementNumberOfCrewmatesAndImposters(player));
        }
    }

    IEnumerator IncrementNumberOfCrewmatesAndImposters(GameObject player)
    {
        yield return new WaitForSeconds(5f);
        if (player.GetComponent<Role>().updatedRole == "Crewmate" || player.GetComponent<Role>().currRole == Role.Roles.Crewmate)
        {
            crewmates++;
            FindObjectOfType<TaskBar>().totalNumOfTasks += 3;
            Debug.Log("Updated total number of tasks: " + FindObjectOfType<TaskBar>().totalNumOfTasks.ToString());
        }
        else
        {
            imposters++;
        }
    }

    public void ToggleUI(bool change)
    {
        roleTextCanvas.SetActive(change);
        taskbarCanvas.SetActive(change);
        minimapCanvas.SetActive(change);
        taskListCanvas.SetActive(change);
    }

    public void TeleportPlayers()
    {
        int count = 0;
        GameObject[] alivePlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < alivePlayers.Length; i++)
        {
            alivePlayers[i].gameObject.transform.position = teleportLocations[i].transform.position;
            count++;
        }
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.transform.position = teleportLocations[count].transform.position;
            count++;
        }
    }

    public void RemovePlayer(GameObject player)
    {
        //if (!pv.IsMine) return;
        Debug.Log("Updated Role of: " + player.GetComponent<PhotonView>().Owner.NickName + " is: " + player.GetComponent<Role>().updatedRole);
        Debug.Log("Current Role of: " + player.GetComponent<PhotonView>().Owner.NickName + " is: " + player.GetComponent<Role>().currRole);

        if (player.GetComponent<Role>().updatedRole == "Crewmate" || player.GetComponent<Role>().currRole == Role.Roles.Crewmate)
        {
            pv.RPC("DecrementCrewmates", RpcTarget.All);
        }
        else
            pv.RPC("DecrementImposters", RpcTarget.All);
    }

    [PunRPC]
    public void DecrementCrewmates()
    {
        crewmates--;
        Debug.Log("crewmate numbers have been decremented");
        if (crewmates == 0 || crewmates == imposters)
        {
            Debug.Log("IMPOSTER WIN");
            ImposterWin();
        }
    }

    [PunRPC]
    public void DecrementImposters()
    {
        imposters--;
        Debug.Log("imposter numbers have been decremented");
        if (imposters == 0)
        {
            Debug.Log("CREWMATE WIN");
            CrewmateWin();
        }
    }

    public void CrewmateWin()
    {
        pv.RPC("RPC_CrewmateWin", RpcTarget.All);
    }

    public void ImposterWin()
    {
        pv.RPC("RPC_ImposterWin", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_CrewmateWin()
    {
        StartCoroutine(CrewmateWinCoroutine());
    }

    [PunRPC]
    public void RPC_ImposterWin()
    {
        StartCoroutine(ImposterWinCoroutine());
    }

    IEnumerator ImposterWinCoroutine()
    {
        isFade = true;
        yield return new WaitForSeconds(1f);
    }

    IEnumerator CrewmateWinCoroutine()
    {
        isFade = true;
        isCrewmateWin = true;
        yield return new WaitForSeconds(1f);
    }






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