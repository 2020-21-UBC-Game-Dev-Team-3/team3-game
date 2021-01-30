using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    public static PhotonLauncher Instance;
    [SerializeField] Button red, orange, yellow, green, blue, indigo, purple;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] GameObject playerListPrefab;

    [SerializeField] GameObject startGameButton;
    [SerializeField] RoomManager roomManager;

    [HideInInspector] public PhotonView pv;

    [SerializeField] GameObject nameObject;
    public Menu roomMenu;
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

    private void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to master.");
            PhotonNetwork.ConnectUsingSettings();
        } else
        {
            RemoveDupes();
            if (roomMenu.gameObject.activeSelf == true) { UpdateList(); }
        }
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
        if (PhotonNetwork.NickName == "") PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
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
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
        pv.RPC("RemoveColor", RpcTarget.AllViaServer, PlayerCustomProperties["color"]);

        for (int i = 1; i <= 6; i++)
        {
            if (FixColor(i)) break;
            i++;
        }
    }

    private bool FixColor(int i)
    {
        string colorStr = "takenColor" + i;
        if (PhotonNetwork.MasterClient.CustomProperties[colorStr] == null)
        {
            RemoveColorsTakenMaster();
            AddColorsTakenMaster();
            ColorsTaken.Remove(colorStr);
            ColorsTaken.Add(colorStr, (string)PlayerCustomProperties["color"]);

            PhotonNetwork.MasterClient.CustomProperties.Add(colorStr, (string)PlayerCustomProperties["color"]);
            PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
            return true;
        }
        return false;
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

    public void SetUpLists()
    {
        for (int i = 0; i <= 6; i++) AddTakenColorsToList(i);
        for (int x = 0; x < takenColors.Count; x++)
        {
            for (int y = 0; y < allowedColors.Count; y++)
            {
                if (takenColors[x] == allowedColors[y]) allowedColors.Remove(allowedColors[y]);
            }
        }
        RemoveDupes();
    }

    private void AddTakenColorsToList(int i)
    {
        string colorStr = "takenColor" + i;
        if (PhotonNetwork.MasterClient.CustomProperties[colorStr] != null)
            takenColors.Add((string)PhotonNetwork.MasterClient.CustomProperties[colorStr]);

    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text)) return;
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        InitializeLists();
        MenuManager.Instance.OpenMenu("room");
        TurnOnColorButtons();
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent) 
            Destroy(child.gameObject);

        for (int i = 0; i < players.Count(); i++)
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);

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
        for (int i = 0; i <= 6; i++) RemoveTakenColor(i);
    }

    public void RemoveTakenColor(int i)
    {
        string colorStr = "takenColor" + i;
        if (PhotonNetwork.MasterClient.CustomProperties[colorStr] != null)
            pv.RPC("RemoveColor", RpcTarget.Others, (string)PhotonNetwork.MasterClient.CustomProperties[colorStr]);

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
        for (int i = 0; i <= 6; i++) LeaveRoomAddColor(i, leavingColor);
        PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
    }

    public void LeaveRoomAddColor(int i, string leavingColor)
    {
        string colorStr = "takenColor" + i;
        if ((string)PhotonNetwork.MasterClient.CustomProperties[colorStr] != leavingColor)
        {
            ColorsTaken.Remove(colorStr);
            ColorsTaken.Add(colorStr, PhotonNetwork.MasterClient.CustomProperties[colorStr]);
        } else ColorsTaken.Add(colorStr, null);
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
            for (int i = 0; i <= 6; i++) LeaveRoomRemoveColor(i);
        }
        LeaveRoom();
    }

    private void LeaveRoomRemoveColor(int i)
    {
        string colorStr = "takenColor" + i;
        PhotonNetwork.MasterClient.CustomProperties.Remove(colorStr);
    }

    public void LeaveRoom()
    {
        RemoveColorsTakenMaster();
        for(int i = 0; i <= 6; i++) LeaveRoomAddTakenColor(i);
        InitializeLists();
        PhotonNetwork.LocalPlayer.SetCustomProperties(ColorsTaken);
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
        TurnOffColorButtons();
    }

    private void LeaveRoomAddTakenColor(int i)
    {
        string colorStr = "takenColor" + i;
        ColorsTaken.Add(colorStr, null);
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
    }

    public void StartGame()
    {
        roomManager.maxNumberOfPlayers = PhotonNetwork.PlayerList.Length;
        PhotonNetwork.LoadLevel(1);
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

        for (int i = 0; i <= 6; i++)
            if (colorSelect == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor" + i]) token = false;
        if (token == true)
        {
            pv.RPC("RemoveColor", RpcTarget.AllViaServer, colorSelect);
            pv.RPC("AddColor", RpcTarget.AllViaServer, colorOld);
            for (int i = 0; i <= 6; i++)
                if (colorOld == (string)PhotonNetwork.MasterClient.CustomProperties["takenColor" + i]) PhotonNetwork.MasterClient.CustomProperties.Remove("takenColor" + i);
            for (int i = 0; i <= 6; i++) {
                if (PhotonNetwork.MasterClient.CustomProperties["takenColor" + i] == null)
                {
                    RemoveColorsTakenMaster();
                    AddColorsTakenMaster();
                    ColorsTaken.Remove("takenColor" + i);
                    ColorsTaken.Add("takenColor" + i, colorSelect);
                    PhotonNetwork.MasterClient.CustomProperties.Add("takenColor" + i, colorSelect);
                    PhotonNetwork.MasterClient.SetCustomProperties(ColorsTaken);
                    break;
                }
            }
            RemoveDupes();
            UpdateList();
            PlayerCustomProperties.Remove("color");
            PlayerCustomProperties.Add("color", colorSelect);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
            pv.RPC("KeyisTrue", RpcTarget.AllViaServer);
        }
        else {
            pv.RPC("KeyisTrue", RpcTarget.AllViaServer); 
            RemoveDupes(); 
            UpdateList(); 
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
                    if (allowedColors[f] == allowedColors[g]) allowedColors.RemoveAt(g);
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
    }
    public void GreenButton()
    {

        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "green"));
        }
    }

    public void BlueButton()
    {

        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "blue"));
        }
    }
    public void IndigoButton()
    {

        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "indigo"));
        }
    }
    public void PurpleButton()
    {

        if (key == true)
        {
            pv.RPC("OnClickCoroutine", RpcTarget.Others);
            float random = 0.5f;
            StartCoroutine(RandomSendTime(Random.Range(0, random), "purple"));
        }
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
            if (PhotonNetwork.MasterClient.CustomProperties["takenColor1"] == null)
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
    public IEnumerator RandomSendTime(float time, string _color)
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
            else pv.RPC("KeyisTrue", RpcTarget.AllViaServer);
        }
        else pv.RPC("KeyisTrue", RpcTarget.AllViaServer);

    }
    [PunRPC]
    public void KeyisTrue()
    {
        key = true;
    }
}
