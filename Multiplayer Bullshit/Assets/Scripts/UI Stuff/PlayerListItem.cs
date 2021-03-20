using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    [SerializeField ]Player playerItem;
    Player[] players;
    
    public void SetUp(Player player)
    {
        playerItem = player;
        text.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(playerItem == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
    public void Update()
    {
        switch ((string)playerItem.CustomProperties["color"])
        {
            case "red":
                text.color = Color.red;
                break;
            case "orange":
                text.color = new Color(1f, 163f/255, 0.0f);
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
                text.color = new Color(0f, 77f/255f, 5f/255f);
                break;
            case "maroon":
                text.color = new Color(99f/255f, 6f/255f, 24f/255f);
                break;
        }
    }
}
