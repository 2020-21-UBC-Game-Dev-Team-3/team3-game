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

    [SerializeField] GameObject nameObject;
    public List<string> allowedColors;
    public List<string> takenColors;
    public ExitGames.Client.Photon.Hashtable PlayerCustomProperties = new ExitGames.Client.Photon.Hashtable();
    public ExitGames.Client.Photon.Hashtable ColorsTaken = new ExitGames.Client.Photon.Hashtable();
    public int randomColor;
    public int buttonToken;
    public bool buttonInUse = false;
    public bool btoken = true;
    public bool key = true;
    public bool redBool, orangeBool, yellowBool, greenBool, bllueBool, indigoBool, purpleBool;
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
        PhotonNetwork.ConnectUsingSettings();
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
    public void OnJoinRoomColor()
    {
        randomColor = Random.Range(0, allowedColors.Count);
        PlayerCustomProperties.Add("color", allowedColors[randomColor]);
        Debug.Log(PlayerCustomProperties["color"]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
        pv.RPC("RemoveColor", RpcTarget.AllViaServer, PlayerCustomProperties["color"]);
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor1"] == null)
        {
            RemoveColorsTakenMaster();
            AddColorsTakenMaster();
            ColorsTaken.Remove("takenColor1");

            ColorsTaken.Add("takenColor1", (string)PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.CustomProperties.Add("takenColor1", (string)PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
            Debug.Log("success");
            Debug.Log("takenColor1 " + PhotonNetwork.MasterClient.CustomProperties["takenColor1"]);
        }
        else if (PhotonNetwork.MasterClient.CustomProperties["takenColor2"] == null)
        {
            RemoveColorsTakenMaster();
            AddColorsTakenMaster();
            ColorsTaken.Remove("takenColor2");
            ColorsTaken.Add("takenColor2", PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.CustomProperties.Add("takenColor2", (string)PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
            Debug.Log("success");
            Debug.Log("takenColor2 " + PhotonNetwork.MasterClient.CustomProperties["takenColor2"] );
        }
        else if (PhotonNetwork.MasterClient.CustomProperties["takenColor3"] == null)
        {

            RemoveColorsTakenMaster();
            AddColorsTakenMaster();
            ColorsTaken.Remove("takenColor3");
            ColorsTaken.Add("takenColor3", PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.CustomProperties.Add("takenColor3", (string)PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
            Debug.Log("success");
            Debug.Log("takenColor3 " + PhotonNetwork.MasterClient.CustomProperties["takenColor3"]);
        }
        else if (PhotonNetwork.MasterClient.CustomProperties["takenColor4"] == null)
        {
            RemoveColorsTakenMaster();
            AddColorsTakenMaster();
            ColorsTaken.Remove("takenColor4");
            ColorsTaken.Add("takenColor4", PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.CustomProperties.Add("takenColor4", (string)PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
            Debug.Log("success");
            Debug.Log("takenColor4 " + PhotonNetwork.MasterClient.CustomProperties["takenColor4"]);
        }
        else if (PhotonNetwork.MasterClient.CustomProperties["takenColor5"] == null)
        {
            RemoveColorsTakenMaster();
            AddColorsTakenMaster();
            ColorsTaken.Remove("takenColor5");
            ColorsTaken.Add("takenColor5", PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.CustomProperties.Add("takenColor5", (string)PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
            Debug.Log("success");
            Debug.Log("takenColor5 " + PhotonNetwork.MasterClient.CustomProperties["takenColor5"]);
        }
        else if (PhotonNetwork.MasterClient.CustomProperties["takenColor6"] == null)
        {
            RemoveColorsTakenMaster();
            AddColorsTakenMaster();
            ColorsTaken.Remove("takenColor6");
            ColorsTaken.Add("takenColor6", PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.CustomProperties.Add("takenColor6", (string)PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
            Debug.Log("success");
            Debug.Log("takenColor6 " + PhotonNetwork.MasterClient.CustomProperties["takenColor6"]);
        }
        Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor1"]);
        Debug.Log(PlayerCustomProperties["color"]);
        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["color"]);
        Debug.Log("ColorsTaken1: "+ColorsTaken["takenColor1"]);
        Debug.Log("ColorsTaken2: "+ColorsTaken["takenColor2"]);
        Debug.Log("ColorsTaken3: "+ColorsTaken["takenColor3"]);

        Debug.Log(PlayerCustomProperties["color"]);
    }
    [PunRPC] public void RemoveColor(string color)
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
    public void SetUpLists()
    {
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor1"] != null) { takenColors.Add((string)PhotonNetwork.MasterClient.CustomProperties["takenColor1"]); }
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor2"] != null) { takenColors.Add((string)PhotonNetwork.MasterClient.CustomProperties["takenColor2"]); }
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor3"] != null) { takenColors.Add((string)PhotonNetwork.MasterClient.CustomProperties["takenColor3"]); }
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor4"] != null) { takenColors.Add((string)PhotonNetwork.MasterClient.CustomProperties["takenColor4"]); }
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor5"] != null) { takenColors.Add((string)PhotonNetwork.MasterClient.CustomProperties["takenColor5"]); }
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor6"] != null) { takenColors.Add((string)PhotonNetwork.MasterClient.CustomProperties["takenColor6"]); }
        Debug.Log(takenColors.Count);
        Debug.Log(allowedColors.Count);
        for(int x = 0;  x < takenColors.Count; x++)
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

        foreach(Transform child in playerListContent)
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
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor1"] != null) { pv.RPC("RemoveColor", RpcTarget.Others, (string)PhotonNetwork.MasterClient.CustomProperties["takenColor1"]); }
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor2"] != null) { pv.RPC("RemoveColor", RpcTarget.Others, (string)PhotonNetwork.MasterClient.CustomProperties["takenColor2"]); }
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor3"] != null) { pv.RPC("RemoveColor", RpcTarget.Others, (string)PhotonNetwork.MasterClient.CustomProperties["takenColor3"]); }
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor4"] != null) { pv.RPC("RemoveColor", RpcTarget.Others, (string)PhotonNetwork.MasterClient.CustomProperties["takenColor4"]); }
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor5"] != null) { pv.RPC("RemoveColor", RpcTarget.Others, (string)PhotonNetwork.MasterClient.CustomProperties["takenColor5"]); }
        if (PhotonNetwork.MasterClient.CustomProperties["takenColor6"] != null) { pv.RPC("RemoveColor", RpcTarget.Others, (string)PhotonNetwork.MasterClient.CustomProperties["takenColor6"]); }
        Debug.Log(PhotonNetwork.MasterClient.NickName);
        Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor1"]);
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
        if ((string)PhotonNetwork.MasterClient.CustomProperties["takenColor1"] != leavingColor)
        {
            ColorsTaken.Remove("takenColor1");
            ColorsTaken.Add("takenColor1", PhotonNetwork.MasterClient.CustomProperties["takenColor1"]);
        }
        else
        {
            ColorsTaken.Add("takenColor1", null);
        }
        if ((string)PhotonNetwork.MasterClient.CustomProperties["takenColor2"] != leavingColor)
        {
            ColorsTaken.Remove("takenColor2");
            ColorsTaken.Add("takenColor2", PhotonNetwork.MasterClient.CustomProperties["takenColor2"]);
        }
        else
        {
            ColorsTaken.Add("takenColor2", null);
        }
        if ((string)PhotonNetwork.MasterClient.CustomProperties["takenColor3"] != leavingColor)
        {
            ColorsTaken.Remove("takenColor3");
            ColorsTaken.Add("takenColor3", PhotonNetwork.MasterClient.CustomProperties["takenColor3"]);
        }
        else
        {
            ColorsTaken.Add("takenColor3", null);
        }
        if ((string)PhotonNetwork.MasterClient.CustomProperties["takenColor4"] != leavingColor)
        {
            ColorsTaken.Remove("takenColor4");
            ColorsTaken.Add("takenColor4", PhotonNetwork.MasterClient.CustomProperties["takenColor4"]);
        }
        else
        {
            ColorsTaken.Add("takenColor4", null);
        }
        if ((string)PhotonNetwork.MasterClient.CustomProperties["takenColor5"] != leavingColor)
        {
            ColorsTaken.Remove("takenColor5");
            ColorsTaken.Add("takenColor5", PhotonNetwork.MasterClient.CustomProperties["takenColor5"]);
        }
        else
        {
            ColorsTaken.Add("takenColor5", null);
        }
        if ((string)PhotonNetwork.MasterClient.CustomProperties["takenColor6"] != leavingColor)
        {
            ColorsTaken.Remove("takenColor6");
            ColorsTaken.Add("takenColor6", PhotonNetwork.MasterClient.CustomProperties["takenColor6"]);
        }
        else
        {
            ColorsTaken.Add("takenColor6", null);
        }
        Debug.Log("Leaving color: " + leavingColor);
        Debug.Log("My color dude : " + PlayerCustomProperties["color"]);
        PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
        Debug.Log("My color dude : " + PlayerCustomProperties["color"]);
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
            PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor1");
            PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor2");
            PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor3");
            PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor4");
            PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor5");
            PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor6");
        }
        LeaveRoom();
    }
    public void LeaveRoom()
    {
        RemoveColorsTakenMaster();
        ColorsTaken.Add("takenColor1", null);
        ColorsTaken.Add("takenColor2", null);
        ColorsTaken.Add("takenColor3", null);
        ColorsTaken.Add("takenColor4", null);
        ColorsTaken.Add("takenColor5", null);
        ColorsTaken.Add("takenColor6", null);
        InitializeLists();
        PhotonNetwork.LocalPlayer.SetCustomProperties(ColorsTaken);
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
        TurnOffColorButtons();
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform transform in roomListContent)
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
        PhotonNetwork.LoadLevel(1);
    }

    public void Update()
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
            for(int x = 0; x < takenColors.Count; x++)
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

        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor1"]) { token = false;}
        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor2"]) { token = false; }
        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor3"]) { token = false; }
        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor4"]) { token = false; }
        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor5"]) { token = false; }
        if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor6"]) { token = false; }
        if (token == true)
        {
            pv.RPC("RemoveColor", RpcTarget.AllViaServer, colorSelect);
            pv.RPC("AddColor", RpcTarget.AllViaServer, colorOld);
            if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor1"] == colorOld)) { PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor1"); }
                if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor2"] == colorOld)) { PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor2");}
                if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor3"] == colorOld)) { PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor3");  }
                if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor4"] == colorOld)) { PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor4");  }
                if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor5"] == colorOld)) { PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor5");  }
                if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor6"] == colorOld)) { PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor6");  }
                if ((PhotonNetwork.MasterClient.CustomProperties["takenColor1"] == null))
            {
                RemoveColorsTakenMaster();
                AddColorsTakenMaster();
                ColorsTaken.Remove("takenColor1");
                ColorsTaken.Add("takenColor1", colorSelect);
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor1", colorSelect);
                PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
                Debug.Log("success");
                Debug.Log("takenColor1 " + PhotonNetwork.MasterClient.CustomProperties["takenColor1"]);
               // PhotonNetwork.MasterClient.CustomProperties.Add("takenColor1", colorSelect);  
            }
            else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor2"] == null))
            {
                RemoveColorsTakenMaster();
                AddColorsTakenMaster();
                ColorsTaken.Remove("takenColor2");
                ColorsTaken.Add("takenColor2", colorSelect);
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor2", colorSelect);
                PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
                Debug.Log("success");
                Debug.Log("takenColor2 " + PhotonNetwork.MasterClient.CustomProperties["takenColor2"]);
                // PhotonNetwork.MasterClient.CustomProperties.Add("takenColor1", colorSelect);  
            }
            else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor3"] == null))
            {
                RemoveColorsTakenMaster();
                AddColorsTakenMaster();
                ColorsTaken.Remove("takenColor3");
                ColorsTaken.Add("takenColor3", colorSelect);
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor3", colorSelect);
                PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
                Debug.Log("success");
                Debug.Log("takenColor3 " + PhotonNetwork.MasterClient.CustomProperties["takenColor3"]);
                // PhotonNetwork.MasterClient.CustomProperties.Add("takenColor1", colorSelect);  
            }
            else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor4"] == null))
            {
                RemoveColorsTakenMaster();
                AddColorsTakenMaster();
                ColorsTaken.Remove("takenColor4");
                ColorsTaken.Add("takenColor4", colorSelect);
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor4", colorSelect);
                PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
                Debug.Log("success");
                Debug.Log("takenColor4 " + PhotonNetwork.MasterClient.CustomProperties["takenColor4"]);
                // PhotonNetwork.MasterClient.CustomProperties.Add("takenColor1", colorSelect);  
            }
            else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor5"] == null))
            {
                RemoveColorsTakenMaster();
                AddColorsTakenMaster();
                ColorsTaken.Remove("takenColor5");
                ColorsTaken.Add("takenColor5", colorSelect);
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor5", colorSelect);
                PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
                Debug.Log("success");
                Debug.Log("takenColor5 " + PhotonNetwork.MasterClient.CustomProperties["takenColor5"]);
                // PhotonNetwork.MasterClient.CustomProperties.Add("takenColor1", colorSelect);  
            }
            else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor6"] == null))
            {
                RemoveColorsTakenMaster();
                AddColorsTakenMaster();
                ColorsTaken.Remove("takenColor6");
                ColorsTaken.Add("takenColor6", colorSelect);
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor6", colorSelect);
                PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
                Debug.Log("success");
                Debug.Log("takenColor6 " + PhotonNetwork.MasterClient.CustomProperties["takenColor6"]);
                // PhotonNetwork.MasterClient.CustomProperties.Add("takenColor1", colorSelect);  
            }
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

    public void RedButton()
    {

        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "red"));
        }
        else Debug.Log("color already chosen");
    }
    public void OrangeButton()
    {

        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "orange"));
        }
        else Debug.Log("color already chosen");
    }
    public void YellowButton()
    {

        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "yellow"));
        }
        else Debug.Log("color already chosen");
    }
    public void GreenButton()
    {

        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "green"));
        }
        else Debug.Log("color already chosen");
    }

    public void BlueButton()
    {

        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "blue"));
        }
        else Debug.Log("color already chosen");
    }
    public void IndigoButton()
    {

        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "indigo"));
        }
        else Debug.Log("color already chosen");
    }
    public void PurpleButton()
    {

        if (key == true)
        {       
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
        float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "purple"));
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
        PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor1");
        PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor2");
        PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor3");
        PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor4");
        PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor5");
        PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor6");
        for (int u = 0; u < takenColors.Count; u++)
        {
            if(PhotonNetwork.MasterClient.CustomProperties["takenColor1"] == null)
            {
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor1", takenColors[u]);
            }
            else if (PhotonNetwork.MasterClient.CustomProperties["takenColor2"] == null)
            {
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor2", takenColors[u]);
            }
            else if (PhotonNetwork.MasterClient.CustomProperties["takenColor3"] == null)
            {
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor3", takenColors[u]);
            }
            else if (PhotonNetwork.MasterClient.CustomProperties["takenColor4"] == null)
            {
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor4", takenColors[u]);
            }
            else if (PhotonNetwork.MasterClient.CustomProperties["takenColor5"] == null)
            {
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor5", takenColors[u]);
            }
            else if (PhotonNetwork.MasterClient.CustomProperties["takenColor6"] == null)
            {
                PhotonNetwork.MasterClient.CustomProperties.Add("takenColor6", takenColors[u]);
            }
        }
    }
    public IEnumerator OnClickDisableThis()
    {
        yield return new WaitForSeconds(0.5f);
        red.interactable = true;
        orange.interactable = true;
        yellow.interactable = true;
        green.interactable = true;
        blue.interactable = true;
        indigo.interactable = true;
        purple.interactable = true;
    }
    public IEnumerator RandomSendTime(float time , string _color)
    {
        yield return new WaitForSecondsRealtime(time);
        if (allowedColors.Find(x => x == _color) != null)
        {
            if (_color == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor1"]) { btoken = false; }
            else if (_color == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor2"]) { btoken = false; }
            else if (_color == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor3"]) { btoken = false; }
            else if (_color == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor4"]) { btoken = false; }
            else if (_color == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor5"]) { btoken = false; }
            else if (_color == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor6"]) { btoken = false; }
            else { btoken = true; }
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
    }
}
