using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CameraManager : MonoBehaviour
{
    public bool isPlayer;

    [SerializeField] GameObject cameraSwitcher;
    [SerializeField] GameObject cameraMan;
    [SerializeField] GameObject helpMenu;

    // Start is called before the first frame update
    void Start()
    {
        isPlayer = true;

        cameraMan.GetComponent<Camera>().enabled = false;
        cameraMan.GetComponent<FreeFlyCamera>().enabled = false;

        //cameraSwitcher.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
        //cameraSwitcher.GetComponents<PlayMakerFSM>()[0].enabled = true;
        //cameraSwitcher.GetComponent<PlayerActionController>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraSwitcher == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<PhotonView>().Owner.IsMasterClient)
                {
                    cameraSwitcher = players[i];
                    break;
                }
            }
        }

        if (Input.GetKeyDown("return") && isPlayer)
        {
            cameraMan.GetComponent<Camera>().enabled = true;
            cameraMan.GetComponent<FreeFlyCamera>().enabled = true;

            helpMenu.SetActive(false);
            cameraSwitcher.GetComponent<PlayerUIToggle>().TogglePlayerUI(false);
            cameraSwitcher.GetComponentInChildren<CinemachineFreeLook>().enabled = false;
            cameraSwitcher.GetComponents<PlayMakerFSM>()[0].enabled = false;
            cameraSwitcher.GetComponent<PlayerActionController>().enabled = false;

            GameObject.Find("GameManager").GetComponent<GameManager>().ToggleUI(false);

            isPlayer = false;
        }
        else if (Input.GetKeyDown("return") && !isPlayer)
        {
            cameraMan.GetComponent<Camera>().enabled = false;
            cameraMan.GetComponent<FreeFlyCamera>().enabled = false;

            helpMenu.SetActive(true);
            cameraSwitcher.GetComponent<PlayerUIToggle>().TogglePlayerUI(true);
            cameraSwitcher.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
            cameraSwitcher.GetComponents<PlayMakerFSM>()[0].enabled = true;
            cameraSwitcher.GetComponent<PlayerActionController>().enabled = true;

            GameObject.Find("GameManager").GetComponent<GameManager>().ToggleUI(true);

            isPlayer = true;
        }
    }
}
