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
    [SerializeField]
    private float distanceToClient;
    public GameObject clientPlayer;
    [SerializeField] TMP_Text text;
    public void SetClientGhost(GameObject go)
    {
        List<GameObject> gos = new List<GameObject>();
        gos.AddRange(GameObject.FindGameObjectsWithTag("Ghost"));
        gos.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        foreach (GameObject gameo in gos)
        {
            NameTextScript nameScript = gameo.GetComponent<NameTextScript>();
            nameScript.clientPlayer = go;
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
        mainCameraTransform = Camera.main.transform;
        if (pv.IsMine)
        {
            SetName();
            if (gameObject.CompareTag("Player"))
            {
                SetClientPlayer(gameObject);
            }
            else
            {
                SetClientGhost(gameObject);
            }
        }
        else
        {
            SetOwnerName();
        }
    }

    public void Update()
    {
        if (pv.IsMine)
        {
            SetName();
            SetClientPlayer(this.gameObject);
        }
        if (!pv.IsMine)
        {

                distanceToClient = Vector3.Distance(this.transform.position, clientPlayer.transform.position);
                if (distanceToClient > 5)
                {
                    text.enabled = false;
                }
                else text.enabled = true;
        }

    }

    void Awake()
    {
        pv = GetComponent<PhotonView>();

        Debug.Log((string)pv.Owner.CustomProperties["color"]);
        if (!pv.IsMine)
        {
            switch ((string)pv.Owner.CustomProperties["color"])
            {
                case "red":
                    text.color = Color.red;
                    break;
                case "orange":
                    text.color = new Color(1f, 163f / 255, 0.0f);
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
                case "white":
                    text.color = Color.white;
                    break;
                case "black":
                    text.color = Color.black;
                    break;
                case "dgreen":
                    text.color = new Color(0f, 77f / 255f, 5f / 255f);
                    break;
                case "maroon":
                    text.color = new Color(99f / 255f, 6f / 255f, 24f / 255f);
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
                    text.color = new Color(1f, 163f / 255, 0.0f);
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
                case "white":
                    text.color = Color.white;
                    break;
                case "black":
                    text.color = Color.black;
                    break;
                case "dgreen":
                    text.color = new Color(0f, 77f / 255f, 5f / 255f);
                    break;
                case "maroon":
                    text.color = new Color(99f / 255f, 6f / 255f, 24f / 255f);
                    break;
            }
        }


    }
    [PunRPC]
    private void SetOwnerName() => text.text = pv.Owner.NickName;
    [PunRPC]
    private void SetName() => text.text = PhotonNetwork.NickName;

    public void SetClientPlayer(GameObject go)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject gameo in gos)
        {
            NameTextScript nameScript = gameo.GetComponent<NameTextScript>();
            nameScript.clientPlayer = go;
        }
    }
}
