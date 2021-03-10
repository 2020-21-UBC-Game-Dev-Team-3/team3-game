using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NameTextScript : MonoBehaviour
{
    public ExitGames.Client.Photon.Hashtable PlayerCustomProperties = new ExitGames.Client.Photon.Hashtable();
    PhotonView pv;
    private Camera mainCamera;
    private Transform mainCameraTransform;
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCameraTransform = Camera.main.transform;
        if (pv.IsMine)
        {
            SetName();
        }
        else SetOwnerName();
    }
    public void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("MyColor : " + PhotonNetwork.LocalPlayer.CustomProperties["color"]);
            Debug.Log("MyColor : " + pv.Owner.CustomProperties["color"]);
            Debug.Log("Taken Colors 1-6");
            Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor1"]);
            Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor2"]);
            Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor3"]);
            Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor4"]);
            Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor5"]);
            Debug.Log(PhotonNetwork.MasterClient.CustomProperties["takenColor6"]);
            Debug.Log("TakenColorList");
        }
    }
    void Awake()
    {
        pv = GetComponent<PhotonView>();

        Debug.Log((string)pv.Owner.CustomProperties["color"]);
        if (!pv.IsMine) {
            switch ((string)pv.Owner.CustomProperties["color"])
            {
                case "red":
                    text.color = Color.red;
                    break;
                case "orange":
                    text.color = new Color(1.0f, 0.64f, 0.0f);
                    break;
                case "yellow":
                    text.color = Color.yellow;
                    break;
                case "green":
                    text.color = Color.green;
                    break;
                case "blue":
                    text.color = Color.blue;
                    break;
                case "indigo":
                    text.color = Color.cyan;
                    break;
                case "purple":
                    text.color = Color.magenta;
                    break;
            }
        }
        else
        {
            switch ((string)pv.Owner.CustomProperties["color"])
            {
                case "red":
                    text.color = Color.red;
                break;
                case "orange":
                    text.color = new Color(0.2f, 0.3f, 0.4f);
                break;
                case "yellow":
                    text.color = Color.yellow;
                break;
                case "green":
                    text.color = Color.green;
                break;
                case "blue":
                    text.color = Color.blue;
                break;
                case "indigo":
                    text.color = Color.cyan;
                break;
                case "purple":
                    text.color = Color.magenta;
                break;
            }
        }
        
        
    }
    [PunRPC]
    private void SetOwnerName() => text.text = pv.Owner.NickName;
    [PunRPC]
    private void SetName() => text.text = PhotonNetwork.NickName;
}
