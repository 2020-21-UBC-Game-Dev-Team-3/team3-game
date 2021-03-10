using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    public static PhotonLauncher Instance;
    [SerializeField] Button red, orange, yellow, green, blue, indigo, purple;
    public Menu roomMenu;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    public PhotonView pv;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] RoomManager roomManager;
    public List<string> allowedColors;
    public List<string> takenColors;
    public ExitGames.Client.Photon.Hashtable PlayerCustomProperties = new ExitGames.Client.Photon.Hashtable();
    public ExitGames.Client.Photon.Hashtable ColorsTaken = new ExitGames.Client.Photon.Hashtable();
    public int randomColor;
    public bool buttonInUse = false;
    public bool btoken = true;
    public bool key = true;
    public bool redBool, orangeBool, yellowBool, greenBool, bllueBool, indigoBool, purpleBool;
    public int MaxPlayerCount = 6;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        Instance = this;
        InitializeLists();

    }
    public void InitializeLists()
    {
        allowedColors.Clear();
        allowedColors.Add("red");
        allowedColors.Add("orange");
        allowedColors.Add("yellow");
        allowedColors.Add("green");
        allowedColors.Add("blue");
        allowedColors.Add("indigo");
        allowedColors.Add("purple");
        takenColors.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to master.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Joined master.");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined lobby.");
        if (PhotonNetwork.NickName == "") { PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000"); };
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text)) return;
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }
    public void InputName()
    {
        if (string.IsNullOrEmpty(playerNameInputField.text)) return;
        PhotonNetwork.NickName = playerNameInputField.text;
        MenuManager.Instance.OpenMenu("title");
    }
    public void LoopForJoinRoomColor()
    {
        for (int x = 1; x <= MaxPlayerCount; x++)
        {
            if (PhotonNetwork.MasterClient.CustomProperties["takenColor" + x] == null)
            {
                RemoveColorsTakenMaster();
                AddColorsTakenMaster();
                ColorsTaken.Remove("takenColor" + x);

                ColorsTaken.Add("takenColor" + x, (string)PlayerCustomProperties["color"]);
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor" + x, (string)PlayerCustomProperties["color"]);
                PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
                break;
            }
        }
    }
    public void OnJoinRoomColor()
    {
        randomColor = Random.Range(0, allowedColors.Count);
        PlayerCustomProperties.Add("color", allowedColors[randomColor]);
        Debug.Log(PlayerCustomProperties["color"]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
        pv.RPC("RemoveColor", RpcTarget.AllViaServer, PlayerCustomProperties["color"]);
        LoopForJoinRoomColor();
    }
    [PunRPC]
    public void RemoveColor(string color)
    {
        takenColors.Add(color);
        allowedColors.Remove(color);
    }
    [PunRPC]
    public void AddColor(string color)
    {
        takenColors.Remove(color);
        allowedColors.Add(color);
    }
    public void LoopForSetUpLists()
    {
        for (int p = 1; p <= MaxPlayerCount; p++)
        {
            if (PhotonNetwork.MasterClient.CustomProperties["takenColor" + p] != null)
            {
                takenColors.Add((string)PhotonNetwork.MasterClient.CustomProperties["takenColor" + p]);
            }
        }
    }
    public void SetUpLists()
    {
        LoopForSetUpLists();
        for (int x = 0; x < takenColors.Count; x++)
        {
            for (int y = 0; y < allowedColors.Count; y++)
            {
                if (takenColors[x] == allowedColors[y])
                {
                    Debug.Log("Removing " + allowedColors[y]);
                    allowedColors.Remove(allowedColors[y]);
                }
            }
        }
        RemoveDupes();
    }

    public override void OnJoinedRoom()
    {
        InitializeLists();
        MenuManager.Instance.OpenMenu("room");
        TurnOnColorButtons();
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        SettingUpPlayerOnRoomEnter();
    }
    public void SettingUpPlayerOnRoomEnter()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            pv.RPC("ListSetHost", RpcTarget.MasterClient);
            SetUpLists();
            OnJoinRoomColor();
        }
        else
        {
            OnJoinRoomColor();
        }
        RemoveDupes();
    }
    [PunRPC]
    public void ListSetHost()
    {
        LoopForListSetHost();
        Debug.Log(PhotonNetwork.MasterClient.NickName);
        Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor1"]);
    }
    public void LoopForListSetHost()
    {
        for (int x = 1; x <= MaxPlayerCount; x++)
        {
            if (PhotonNetwork.MasterClient.CustomProperties["takenColor" + x] != null) { pv.RPC("RemoveColor", RpcTarget.Others, (string)PhotonNetwork.MasterClient.CustomProperties["takenColor" + x]); }
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room creation failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }
    [PunRPC]
    public void LeaveRoomColor(string leavingColor)
    {
        RemoveColorsTakenMaster();
        LoopForLeaveRoomColor(leavingColor);
        PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
    }
    public void LoopForLeaveRoomColor(string leavingColor)
    {
        for (int x = 1; x <= MaxPlayerCount; x++)
        {
            if ((string)PhotonNetwork.MasterClient.CustomProperties["takenColor" + x] != leavingColor)
            {
                ColorsTaken.Remove("takenColor" + x);
                ColorsTaken.Add("takenColor" + x, PhotonNetwork.MasterClient.CustomProperties["takenColor1" + x]);
            }
            else
            {
                ColorsTaken.Add("takenColor" + x, null);
            }
        }
    }
    [PunRPC]
    public void KickAll()
    {
        LeaveRoomButton();
    }
    [PunRPC]
    public void HostLeftLog()
    {
        Debug.Log("Room closed, host left the room");
    }
    public void LeaveRoomButton()
    {
        pv.RPC("AddColor", RpcTarget.AllViaServer, (string)PlayerCustomProperties["color"]);
        pv.RPC("LeaveRoomColor", RpcTarget.AllViaServer, (string)PlayerCustomProperties["color"]);
        InitializeLists();
        PlayerCustomProperties.Remove("color");
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("KickAll", RpcTarget.Others);
            pv.RPC("HostLeftLog", RpcTarget.Others);
            LoopForLeaveRoomButton();
        }
        LeaveRoom();
    }
    public void LoopForLeaveRoomButton()
    {
        for (int x = 1; x <= MaxPlayerCount; x++)
        {
            PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor" + x);
        }
    }
    public void LeaveRoom()
    {
        RemoveColorsTakenMaster();
        ColorsTakenNull();
        InitializeLists();
        PhotonNetwork.LocalPlayer.SetCustomProperties(ColorsTaken);
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
        TurnOffColorButtons();
    }
    public void ColorsTakenNull()
    {
        for (int x = 1; x <= MaxPlayerCount; x++)
        {
            ColorsTaken.Add("takenColor" + x, null);
        }
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transform in roomListContent)
        {
            Destroy(transform.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList) continue;
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        if (PhotonNetwork.IsMasterClient)
        {

        }
    }

    public void StartGame()
    {
        roomManager.maxNumberOfPlayers = PhotonNetwork.PlayerList.Length;
        PhotonNetwork.LoadLevel(1);
    }

    public void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to master.");
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            RemoveDupes();
            if (Input.GetKeyDown("space"))
            {
                Debug.Log("MyColor : " + PlayerCustomProperties["color"]);
                Debug.Log("Taken Colors 1-6");
                Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor1"]);
                Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor2"]);
                Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor3"]);
                Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor4"]);
                Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor5"]);
                Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor6"]);
                Debug.Log("TakenColorList");
                for (int x = 0; x < takenColors.Count; x++)
                {
                    Debug.Log(takenColors[x]);
                }
                Debug.Log("AllowedColorsList");
                for (int x = 0; x < allowedColors.Count; x++)
                {
                    Debug.Log(allowedColors[x]);
                }
            }
            if (roomMenu.gameObject.activeSelf == true) { UpdateList(); }
        }
    }
    public void TurnOnColorButtons()
    {
        red.gameObject.SetActive(true);
        orange.gameObject.SetActive(true);
        yellow.gameObject.SetActive(true);
        green.gameObject.SetActive(true);
        blue.gameObject.SetActive(true);
        indigo.gameObject.SetActive(true);
        purple.gameObject.SetActive(true);
    }
    public void TurnOffColorButtons()
    {
        red.gameObject.SetActive(false);
        orange.gameObject.SetActive(false);
        yellow.gameObject.SetActive(false);
        green.gameObject.SetActive(false);
        blue.gameObject.SetActive(false);
        indigo.gameObject.SetActive(false);
        purple.gameObject.SetActive(false);
    }
    [PunRPC]
    public void ButtonInUse(bool cond)
    {
        buttonInUse = cond;
    }
    public void colorButton(string colorSelect, string colorOld)
    {
        int z = 0;
        bool token = false;
        for (int x = 0; x < takenColors.Count; x++)
        {
            for (int y = 0; y < allowedColors.Count; y++)
            {
                if (takenColors[x] == allowedColors[y])
                {
                    allowedColors.Remove(allowedColors[y]);
                    z++;
                    x = 0;
                }
            }
        }
        if (z == 0)
        {
            token = true;
        }

        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor1"]) { token = false; }
        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor2"]) { token = false; }
        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor3"]) { token = false; }
        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor4"]) { token = false; }
        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor5"]) { token = false; }
        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor6"]) { token = false; }
        if (token == true)
        {
            pv.RPC("RemoveColor", RpcTarget.AllViaServer, colorSelect);
            pv.RPC("AddColor", RpcTarget.AllViaServer, colorOld);
            LoopForCB1(colorOld);
            LoopForCB2(colorSelect);
            Debug.Log(PlayerCustomProperties["color"]);
            RemoveDupes();
            UpdateList();
            PlayerCustomProperties.Remove("color");
            PlayerCustomProperties.Add("color", colorSelect);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
            pv.RPC("KeyisTrue", RpcTarget.AllViaServer);
        }
        else { Debug.Log("Color already chosen (so close)"); pv.RPC("KeyisTrue", RpcTarget.AllViaServer); RemoveDupes(); UpdateList(); }
    }
    public void LoopForCB2(string colorSelect)
    {
        for (int x = 1; x <= MaxPlayerCount; x++)
        {
            if ((PhotonNetwork.MasterClient.CustomProperties["takenColor" + x] == null))
            {
                RemoveColorsTakenMaster();
                AddColorsTakenMaster();
                ColorsTaken.Remove("takenColor" + x);
                ColorsTaken.Add("takenColor" + x, colorSelect);
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor" + x, colorSelect);
                PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
                Debug.Log("success");
                Debug.Log(("takenColor" + x) + PhotonNetwork.MasterClient.CustomProperties["takenColor" + x]);
                // PhotonNetwork.MasterClient.CustomProperties.Add("takenColor1", colorSelect);  
                break;
            }
        }
    }
    public void LoopForCB1(string colorOld)
    {
        for (int x = 1; x <= MaxPlayerCount; x++)
        {
            if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor" + x] == colorOld)) { PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor" + x); }
        }
    }
    public void RemoveColorsTakenMaster()
    {
        ColorsTaken.Remove("takenColor1");
        ColorsTaken.Remove("takenColor2");
        ColorsTaken.Remove("takenColor3");
        ColorsTaken.Remove("takenColor4");
        ColorsTaken.Remove("takenColor5");
        ColorsTaken.Remove("takenColor6");
    }
    public void AddColorsTakenMaster()
    {
        ColorsTaken.Add("takenColor1", PhotonNetwork.MasterClient.CustomProperties["takenColor1"]);
        ColorsTaken.Add("takenColor2", PhotonNetwork.MasterClient.CustomProperties["takenColor2"]);
        ColorsTaken.Add("takenColor3", PhotonNetwork.MasterClient.CustomProperties["takenColor3"]);
        ColorsTaken.Add("takenColor4", PhotonNetwork.MasterClient.CustomProperties["takenColor4"]);
        ColorsTaken.Add("takenColor5", PhotonNetwork.MasterClient.CustomProperties["takenColor5"]);
        ColorsTaken.Add("takenColor6", PhotonNetwork.MasterClient.CustomProperties["takenColor6"]);
    }
    public void RemoveDupes()
    {
        if (takenColors.Count > 0)
        {
            for (int f = 0; f < takenColors.Count; f++)
            {
                for (int g = f + 1; g < takenColors.Count; g++)
                {
                    if (takenColors[f] == takenColors[g])
                    {
                        Debug.Log("Removing duplicate " + takenColors[g]);
                        takenColors.RemoveAt(g);
                    }
                }
            }
        }

        if (allowedColors.Count > 0)
        {
            for (int f = 0; f < allowedColors.Count; f++)
            {
                for (int g = f + 1; g < allowedColors.Count; g++)
                {
                    if (allowedColors[f] == allowedColors[g])
                    {
                        Debug.Log("Removing duplicate " + allowedColors[g]);
                        allowedColors.RemoveAt(g);
                    }
                }
            }
        }
    }
    public void ButtonFunctions(string c)
    {
        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), c));
        }
        else Debug.Log("color already chosen");
    }
    public void OnClickDisable()
    {
        red.interactable = false;
        orange.interactable = false;
        yellow.interactable = false;
        green.interactable = false;
        blue.interactable = false;
        indigo.interactable = false;
        purple.interactable = false;
        OnClickCoroutine();
    }
    [PunRPC]
    public void OnClickCoroutine()
    {
        key = false;
    }
    public void UpdateList()
    {
        RemoveMCCP();
        for (int u = 0; u < takenColors.Count; u++)
        {
            for (int x = 1; x <= MaxPlayerCount; x++)
            {
                if (PhotonNetwork.MasterClient.CustomProperties["takenColor" + x] == null)
                {
                    PhotonNetwork.MasterClient.CustomProperties.Add("takenColor" + x, takenColors[u]);
                    break;
                }

            }
        }
    }
    public void RemoveMCCP()
    {
        for (int x = 1; x <= MaxPlayerCount; x++)
        {
            PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor" + x);
        }
    }
    public IEnumerator RandomSendTime(float time, string _color)
    {
        yield return new WaitForSecondsRealtime(time);
        if (allowedColors.Find(x => x == _color) != null)
        {
            LoopRandomSendTime(_color);
            if (allowedColors.Find(x => x == _color) != null && btoken)
            {
                colorButton(_color, (string)PlayerCustomProperties["color"]);
                btoken = false;
            }
            else { Debug.Log("color already chosen"); pv.RPC("KeyisTrue", RpcTarget.AllViaServer); }
        }
        else
        {
            pv.RPC("KeyisTrue", RpcTarget.AllViaServer);
        }

    }
    [PunRPC]
    public void KeyisTrue()
    {
        key = true;
        btoken = true;
    }
    public void LoopRandomSendTime(string _color)
    {
        for (int x = 1; x <= MaxPlayerCount; x++)
        {
            if (_color == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor" + x]) { btoken = false; break; }
            else { btoken = true; };
        }
    }
}


