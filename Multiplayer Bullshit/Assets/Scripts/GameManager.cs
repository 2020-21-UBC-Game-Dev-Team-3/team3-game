using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [HideInInspector] public bool isConnectedToMaster;
    [HideInInspector] public Color playerColor;
    // Start is called before the first frame update

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(20f);
       /* PhotonNetwork.LeaveRoom();*/ //FOR TESTING
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "Gaming")
            StartCoroutine("EndGame");
    }

   public override void OnLeftRoom()
    {
        if (SceneManager.GetActiveScene().name == "Gaming")
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel(0);
        }
    }
}
