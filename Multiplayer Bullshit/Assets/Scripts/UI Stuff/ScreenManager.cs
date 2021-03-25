using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    private void OnEnable()
    {
        if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
        if (Cursor.visible == false) Cursor.visible = true;
        PhotonNetwork.Destroy(GameObject.Find("RoomManager"));
    }

    private void OnDisable()
    {
        //if (FindObjectOfType<RoomManager>().isActiveAndEnabled) PhotonNetwork.Destroy(GameObject.Find("RoomManager"));
        Destroy(GameObject.Find("PlayMaker Photon Proxy"));
        Destroy(GameObject.Find("DeathTrack"));
    }

    public void OnReturnButtonPressed() => StartCoroutine(ReturnToMenu());

    IEnumerator ReturnToMenu()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    

}
