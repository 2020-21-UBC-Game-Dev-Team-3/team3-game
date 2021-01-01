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
    public int randomColor;
    public bool buttonInUse = false;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        {
         //   PlayerCustomProperties.Add("color", "none");
          //  PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
        }
        Instance = this;

            allowedColors.Add("red");
            allowedColors.Add("orange");
            allowedColors.Add("yellow");
            allowedColors.Add("green");
            allowedColors.Add("blue");
            allowedColors.Add("indigo");
            allowedColors.Add("purple");

            randomColor = Random.Range(0, allowedColors.Count);
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
        if (PhotonNetwork.IsMasterClient)
        {
            if ((PhotonNetwork.MasterClient.CustomProperties["takenColor1"] == null)) { PlayerCustomProperties.Add("takenColor1", allowedColors[randomColor]); }
            else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor2"] == null)) { PlayerCustomProperties.Add("takenColor2", allowedColors[randomColor]); }
            else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor3"] == null)) { PlayerCustomProperties.Add("takenColor3", allowedColors[randomColor]); }
            else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor4"] == null)) { PlayerCustomProperties.Add("takenColor4", allowedColors[randomColor]); }
            else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor5"] == null)) { PlayerCustomProperties.Add("takenColor5", allowedColors[randomColor]); }
            else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor6"] == null)) { PlayerCustomProperties.Add("takenColor6", allowedColors[randomColor]); }
        }
            pv.RPC("RemoveColor", RpcTarget.All, allowedColors[randomColor]);
            //takenColors.Add(allowedColors[randomColor]);
            //allowedColors.Remove(allowedColors[randomColor]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
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
        for(int x = 0;  x < takenColors.Count; x++)
        {
            for (int y = 0; y < allowedColors.Count; y++)
            {
                if (takenColors[x] == allowedColors[y])
                {
                    allowedColors.Remove(allowedColors[y]);
                }
            }
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            SetUpLists();
            Debug.Log("bruh");
            OnJoinRoomColor();

        }
        else
        {
            OnJoinRoomColor();
        }
        //OnJoinRoomColor();

        /*randomColor = Random.Range(0, allowedColors.Count);
        PlayerCustomProperties.Add("color", allowedColors[randomColor]);
        takenColors.Add(allowedColors[randomColor]);
        allowedColors.Remove(allowedColors[randomColor]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
        Debug.Log(PlayerCustomProperties["color"]);*/
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
    public void LeaveRoomColor()
    {
        pv.RPC("RemoveColor", RpcTarget.All, allowedColors[randomColor]);
        //allowedColors.Add(PlayerCustomProperties["color"].ToString());
        //PlayerCustomProperties.Remove("color");
        takenColors.Remove(allowedColors[randomColor]);
        PlayerCustomProperties.Remove("color");
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
    }

    public void LeaveRoom()
    {
        LeaveRoomColor();
        /*allowedColors.Add(PlayerCustomProperties["color"].ToString());
        PlayerCustomProperties.Remove("color");
        takenColors.Remove(allowedColors[randomColor]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);*/
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
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void Update()
    {
        nameObject.name = PhotonNetwork.NickName;
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
        if (token == true)
        {
            pv.RPC("RemoveColor", RpcTarget.All, colorSelect);
            pv.RPC("AddColor", RpcTarget.All, colorOld);
            PlayerCustomProperties.Remove("color");
            PlayerCustomProperties.Add("color", colorSelect);
            if (PhotonNetwork.IsMasterClient)
            {
                if ((PhotonNetwork.MasterClient.CustomProperties["takenColor1"] == null)) { PlayerCustomProperties.Add("takenColor1", colorSelect); }
                else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor2"] == null)) { PlayerCustomProperties.Add("takenColor2", colorSelect); }
                else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor3"] == null)) { PlayerCustomProperties.Add("takenColor3", colorSelect); }
                else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor4"] == null)) { PlayerCustomProperties.Add("takenColor4", colorSelect); }
                else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor5"] == null)) { PlayerCustomProperties.Add("takenColor5", colorSelect); }
                else if ((PhotonNetwork.MasterClient.CustomProperties["takenColor6"] == null)) { PlayerCustomProperties.Add("takenColor6", colorSelect); }
                    if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor1"] == colorOld) && (string)PhotonNetwork.MasterClient.CustomProperties["takenColor1"] != null) { PlayerCustomProperties.Remove("takenColor1");}
                    if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor2"] == colorOld) && (string)PhotonNetwork.MasterClient.CustomProperties["takenColor2"] != null) { PlayerCustomProperties.Remove("takenColor2");}
                    if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor3"] == colorOld) && (string)PhotonNetwork.MasterClient.CustomProperties["takenColor3"] != null) { PlayerCustomProperties.Remove("takenColor3");}
                    if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor4"] == colorOld) && (string)PhotonNetwork.MasterClient.CustomProperties["takenColor4"] != null) { PlayerCustomProperties.Remove("takenColor4");}
                    if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor5"] == colorOld) && (string)PhotonNetwork.MasterClient.CustomProperties["takenColor5"] != null) { PlayerCustomProperties.Remove("takenColor5");}
                    if (((string)PhotonNetwork.MasterClient.CustomProperties["takenColor6"] == colorOld) && (string)PhotonNetwork.MasterClient.CustomProperties["takenColor6"] != null) { PlayerCustomProperties.Remove("takenColor6");}
            }
            Debug.Log(PlayerCustomProperties["color"]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
           /* for (int x = 0; x < takenColors.Count; x++)
            {
                for (int y = 0; y < allowedColors.Count; y++)
                {
                    if (takenColors[x] == allowedColors[y])
                    {
                        allowedColors.Remove(allowedColors[y]);
                        x = 0;
                    }
                }
            }*/
        }
        else { Debug.Log("Color already chosen"); }
    }
    public void RedButton()
    {
        string color = "red";
        if (allowedColors.Find(x => x == color) != null && !buttonInUse)
        {
            pv.RPC("ButtonInUse", RpcTarget.All, true);
            colorButton(color, (string)PlayerCustomProperties["color"]);
            pv.RPC("ButtonInUse", RpcTarget.All, false);
        }
        else Debug.Log("color already chosen");
    }
    public void OrangeButton()
    {
        string color = "orange";
        if (allowedColors.Find(x => x == color) != null && !buttonInUse)
        {
            pv.RPC("ButtonInUse", RpcTarget.All, true);
            colorButton(color, (string)PlayerCustomProperties["color"]);
            pv.RPC("ButtonInUse", RpcTarget.All, false);
        }
        else Debug.Log("color already chosen");
    }
    public void YellowButton()
    {
        string color = "yellow";
        if (allowedColors.Find(x => x == color) != null && !buttonInUse)
        {
            pv.RPC("ButtonInUse", RpcTarget.All, true);
            colorButton(color, (string)PlayerCustomProperties["color"]);
            pv.RPC("ButtonInUse", RpcTarget.All, false);
        }
        else Debug.Log("color already chosen");
    }
    public void GreenButton()
    {
        string color = "green";
        if (allowedColors.Find(x => x == color) != null && !buttonInUse)
        {
            pv.RPC("ButtonInUse", RpcTarget.All, true);
            colorButton(color, (string)PlayerCustomProperties["color"]);
            pv.RPC("ButtonInUse", RpcTarget.All, false);
        }
        else Debug.Log("color already chosen");
    }

    public void BlueButton()
    {
        string color = "blue";
        if (allowedColors.Find(x => x == color) != null && !buttonInUse)
        {
            pv.RPC("ButtonInUse", RpcTarget.All, true);
            colorButton(color, (string)PlayerCustomProperties["color"]);
            pv.RPC("ButtonInUse", RpcTarget.All, false);
        }
        else Debug.Log("color already chosen");
    }
    public void IndigoButton()
    {
        string color = "indigo";
        if (allowedColors.Find(x => x == color) != null&& !buttonInUse)
        {
            pv.RPC("ButtonInUse", RpcTarget.All, true);
            colorButton(color, (string)PlayerCustomProperties["color"]);
            pv.RPC("ButtonInUse", RpcTarget.All, false);
        }
        else Debug.Log("color already chosen");
    }
    public void PurpleButton()
    {
        string color = "purple";
        if (allowedColors.Find(x => x == color) != null && !buttonInUse)
        {
            pv.RPC("ButtonInUse", RpcTarget.All, true);
            colorButton(color, (string)PlayerCustomProperties["color"]);
            pv.RPC("ButtonInUse", RpcTarget.All, false);
        }
        else Debug.Log("color already chosen");
    }
}
