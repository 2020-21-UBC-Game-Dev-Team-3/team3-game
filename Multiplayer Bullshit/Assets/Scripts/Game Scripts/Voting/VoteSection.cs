using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoteSection : MonoBehaviour
{
/*    private float redTint;
    private float blueTint;
    private float greenTint;*/

/*    private int currNumOfTints;
    private int maxNumOfTints = 3;*/
    [HideInInspector] public int playerInSectionIndex;

/*    private const byte SetColors = 0;
    private const byte SetRBGColors = 1;*/
    public TextMeshProUGUI playerName;

    public void SetupVoteSection(Player player)
    {
        playerInSectionIndex = System.Array.IndexOf(PhotonNetwork.PlayerList, player);
        playerName.text = player.NickName;
        switch ((string)player.CustomProperties["color"])
        {
            case "red":
                playerName.color = Color.red;
                break;
            case "orange":
                playerName.color = new Color(1f, 163f / 255, 0.0f);
                break;
            case "yellow":
                playerName.color = Color.yellow;
                break;
            case "green":
                playerName.color = Color.green;
                break;
            case "blue":
                playerName.color = Color.blue;
                break;
            case "indigo":
                playerName.color = Color.cyan;
                break;
            case "purple":
                playerName.color = Color.magenta;
                break;
            case "white":
                playerName.color = Color.white;
                break;
            case "black":
                playerName.color = Color.black;
                break;
            case "dgreen":
                playerName.color = new Color(0f, 77f / 255f, 5f / 255f);
                break;
            case "maroon":
                playerName.color = new Color(99f / 255f, 6f / 255f, 24f / 255f);
                break;
        }

        /*        Player localPlayer = PhotonNetwork.LocalPlayer;
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] { player.ActorNumber } };
                PhotonNetwork.RaiseEvent(SetColors, localPlayer, raiseEventOptions, SendOptions.SendReliable);*/
    }

/*    public void SetColor(Player localPlayer)
    {
        Color voteSectionPlayerColor = GetComponentInParent<Canvas>().transform.Find("Name Text (TMP)").GetComponent<TextMeshProUGUI>().color;
        Debug.Log("Player's color: " + voteSectionPlayerColor);
        Debug.Log("LOCAL PLAYER NAME: " + PhotonNetwork.LocalPlayer.NickName);
        Debug.Log("Is other player a local player? " + (localPlayer == PhotonNetwork.LocalPlayer) + " Here is their name: " + localPlayer.NickName);
        StartSetupOfRBGColors(localPlayer, voteSectionPlayerColor.r, "red");
        StartSetupOfRBGColors(localPlayer, voteSectionPlayerColor.b, "blue");
        StartSetupOfRBGColors(localPlayer, voteSectionPlayerColor.g, "green");
    }

    private void StartSetupOfRBGColors(Player localPlayer, float colorTint, string rbgColor)
    {
        Debug.Log(rbgColor + " Color tint: " + colorTint);
        Debug.Log("Check Player name: " + localPlayer.NickName);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] { localPlayer.ActorNumber } };
        object[] content = new object[] { colorTint, rbgColor };
        PhotonNetwork.RaiseEvent(SetRBGColors, content, raiseEventOptions, SendOptions.SendReliable);

    }
    public void SetRBGColor(float colorTint, string rbgColor)
    {
        switch(rbgColor)
        {
            case "red": redTint = colorTint;
                break;
            case "blue": blueTint = colorTint;
                break;
            case "green": greenTint = colorTint;
                break;
        }
        currNumOfTints++;
        if (currNumOfTints >= maxNumOfTints)
        {
            currNumOfTints = 0;
            playerProfile.color = new Color(redTint, blueTint, greenTint);
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonData)
    {
        switch(photonData.Code)
        {
            case SetColors: SetColor((Player) photonData.CustomData);
                break;
            case SetRBGColors:
                object[] data = (object[])photonData.CustomData;
                SetRBGColor((float)data[0], (string)data[1]);
                break;
        }
    }*/

}
