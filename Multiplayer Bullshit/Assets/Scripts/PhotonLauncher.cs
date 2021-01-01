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
    public PhotonView photonView2;
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

    void Awake()
    {
        photonView2 = GetComponent<PhotonView>();
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
    [PunRPC]
    public void OnJoinRoomColor()
    {
        randomColor = Random.Range(0, allowedColors.Count);
        PlayerCustomProperties.Add("color", allowedColors[randomColor]);
        takenColors.Add(allowedColors[randomColor]);
        allowedColors.Remove(allowedColors[randomColor]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
        Debug.Log(PlayerCustomProperties["color"]);
    }

    public override void OnJoinedRoom()
    {
        photonView2.RPC("OnJoinRoomColor", RpcTarget.All);
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
    [PunRPC]
    public void LeaveRoomColor()
    {
        allowedColors.Add(PlayerCustomProperties["color"].ToString());
        PlayerCustomProperties.Remove("color");
        takenColors.Remove(allowedColors[randomColor]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
    }

    public void LeaveRoom()
    {
        photonView2.RPC("LeaveRoomColor", RpcTarget.All);
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
    public void colorButton(string color)
    {
        allowedColors.Add(PlayerCustomProperties["color"].ToString());
        PlayerCustomProperties.Remove("color");
        takenColors.Remove(allowedColors[randomColor]);
        PlayerCustomProperties.Add("color", color);
        takenColors.Add(color);
        allowedColors.Remove(color);
        Debug.Log(PlayerCustomProperties["color"]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);
    }
    public void RedButton()
    {
        string color = "red";
        if (allowedColors.Find(x => x == color) != null)
        {
            photonView2.RPC("colorButton", RpcTarget.All, color);
            /*allowedColors.Add(PlayerCustomProperties["color"].ToString());
            PlayerCustomProperties.Remove("color");
            takenColors.Remove(allowedColors[randomColor]);
            PlayerCustomProperties.Add("color", color);
            takenColors.Add(color);
            allowedColors.Remove(color);
            Debug.Log(PlayerCustomProperties["color"]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);*/
        }
        else Debug.Log("color already chosen");
    }
    public void OrangeButton()
    {
        string color = "orange";
        if (allowedColors.Find(x => x == color) != null)
        {
            photonView2.RPC("colorButton", RpcTarget.All, color);
            /*allowedColors.Add(PlayerCustomProperties["color"].ToString());
            PlayerCustomProperties.Remove("color");
            takenColors.Remove(allowedColors[randomColor]);
            PlayerCustomProperties.Add("color", color);
            takenColors.Add(color);
            allowedColors.Remove(color);
            Debug.Log(PlayerCustomProperties["color"]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);*/
        }
        else Debug.Log("color already chosen");
    }
    public void YellowButton()
    {
        string color = "yellow";
        if (allowedColors.Find(x => x == color) != null)
        {
            photonView2.RPC("colorButton", RpcTarget.All, color);
            /*allowedColors.Add(PlayerCustomProperties["color"].ToString());
            PlayerCustomProperties.Remove("color");
            takenColors.Remove(allowedColors[randomColor]);
            PlayerCustomProperties.Add("color", color);
            takenColors.Add(color);
            allowedColors.Remove(color);
            Debug.Log(PlayerCustomProperties["color"]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);*/
        }
        else Debug.Log("color already chosen");
    }
    public void GreenButton()
    {
        string color = "green";
        if (allowedColors.Find(x => x == color) != null)
        {
            photonView2.RPC("colorButton", RpcTarget.All, color);
            /*allowedColors.Add(PlayerCustomProperties["color"].ToString());
            PlayerCustomProperties.Remove("color");
            takenColors.Remove(allowedColors[randomColor]);
            PlayerCustomProperties.Add("color", color);
            takenColors.Add(color);
            allowedColors.Remove(color);
            Debug.Log(PlayerCustomProperties["color"]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);*/
        }
        else Debug.Log("color already chosen");
    }

    public void BlueButton()
    {
        string color = "blue";
        if (allowedColors.Find(x => x == color) != null)
        {
            photonView2.RPC("colorButton", RpcTarget.All, color);
            /*allowedColors.Add(PlayerCustomProperties["color"].ToString());
            PlayerCustomProperties.Remove("color");
            takenColors.Remove(allowedColors[randomColor]);
            PlayerCustomProperties.Add("color", color);
            takenColors.Add(color);
            allowedColors.Remove(color);
            Debug.Log(PlayerCustomProperties["color"]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);*/
        }
        else Debug.Log("color already chosen");
    }
    public void IndigoButton()
    {
        string color = "indigo";
        if (allowedColors.Find(x => x == color) != null)
        {
            photonView2.RPC("colorButton", RpcTarget.All, color);
            /*
            allowedColors.Add(PlayerCustomProperties["color"].ToString());
            PlayerCustomProperties.Remove("color");
            takenColors.Remove(allowedColors[randomColor]);
            PlayerCustomProperties.Add("color", color);
            takenColors.Add(color);
            allowedColors.Remove(color);
            Debug.Log(PlayerCustomProperties["color"]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);*/
        }
        else Debug.Log("color already chosen");
    }
    public void PurpleButton()
    {
        string color = "purple";
        if (allowedColors.Find(x => x == color) != null)
        {
            photonView2.RPC("colorButton", RpcTarget.All, color);/*
            allowedColors.Add(PlayerCustomProperties["color"].ToString());
            PlayerCustomProperties.Remove("color");
            takenColors.Remove(allowedColors[randomColor]);
            PlayerCustomProperties.Add("color", color);
            takenColors.Add(color);
            allowedColors.Remove(color);
            Debug.Log(PlayerCustomProperties["color"]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProperties);*/
        }
        else Debug.Log("color already chosen");
    }
}
