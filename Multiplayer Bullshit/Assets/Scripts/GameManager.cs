using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks {

  [SerializeField] Transform[] teleportLocations = new Transform[10]; // assuming this is the max players in a game

    List<string> availableImposterRoles = new List<string>();
    List<string> availableCrewmateRoles = new List<string>();

    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        availableImposterRoles.AddRange(new string[] { "Assassin", "Chameleon" });
    }

    public void TeleportPlayers() {
    GameObject[] alivePlayers = GameObject.FindGameObjectsWithTag("Player");
    for (int i = 0; i < alivePlayers.Length; i++) {
      alivePlayers[i].gameObject.transform.position = teleportLocations[i].transform.position;
    }
  }


    public void InitiateRoleAbilityAssignment()
    {
        pv.RPC("SetUpRoleAbilityAssignment", RpcTarget.All);
    }

    [PunRPC]
    void SetUpRoleAbilityAssignment()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var x in players) Debug.Log(x.name + " " + x.GetComponent<PhotonView>().ViewID);

        List<GameObject> crewmates = new List<GameObject>();
        List<GameObject> imposters = new List<GameObject>();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<Role>().currRole == Role.Roles.Crewmate)
            {
                crewmates.Add(players[i]);
            }
            else
            {
                imposters.Add(players[i]);
            }
        }
        foreach (var x in imposters) Debug.Log(x.name);


        //RandomizeImposterSubRoles(imposters);

    }

    void RandomizeImposterSubRoles(List<GameObject> imposterList)
    {
        //foreach (var imposter in imposterList)
        //{
        //    if(imposter.GetComponent<PhotonView>().IsMine)
        //    {
        //        switch (availableImposterRoles[0])
        //        {
        //            case "Assassin":
        //                imposter.GetComponent<Role>().subRole = Role.Roles.Assassin;
        //                imposter.GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
        //                pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Assassin");
        //                break;                    

        //            case "Chameleon":
        //                imposter.GetComponent<Role>().subRole = Role.Roles.Chameleon;
        //                imposter.GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
        //                pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Chameleon");
        //                break;

        //            default:
        //                Debug.Log("You're a dingus bingus");
        //                break;
        //        }
        //    }

        //}


        for (int i = 0; i < imposterList.Count; i++)
        {
            Debug.Log(imposterList[i].name + " " + imposterList[i].GetComponent<PhotonView>().ViewID);
            Role imposterRole = imposterList[i].GetComponent<Role>();
            switch (availableImposterRoles[Random.Range(0, availableImposterRoles.Count - 1)])
            {
                case "Assassin":
                    if (!availableImposterRoles.Contains("Assassin")) return;
                    imposterRole.subRole = Role.Roles.Assassin;
                    Debug.Log("You're an assassin");
                    imposterList[i].GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
                    pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Assassin");
                    Debug.Log(availableImposterRoles.Contains("Assassin"));
                    break;

                case "Chameleon":
                    if (!availableImposterRoles.Contains("Chameleon")) return;
                    imposterRole.subRole = Role.Roles.Chameleon;
                    Debug.Log("You're a chameleon");
                    imposterList[i].GetComponent<PlayerActionController>().InitiateRoleAbilityAssignment();
                    pv.RPC("RemoveImposterRoleFromList", RpcTarget.All, "Chameleon");
                    Debug.Log(availableImposterRoles.Contains("Chameleon"));
                    break;

                default:
                    Debug.Log("You're a dingus bingus");
                    break;
            }
        }
    }

    //[PunRPC]
    //void RemoveImposterRoleFromList(string imposterRole)
    //{
    //    availableImposterRoles.Remove(imposterRole);
    //    foreach (var x in availableImposterRoles) Debug.Log(x);
    //}



    /*  [HideInInspector] public bool isConnectedToMaster;
      // Start is called before the first frame update

      public IEnumerator EndGame() {
        yield return new WaitForSeconds(20f);
        *//* PhotonNetwork.LeaveRoom();*//* //FOR TESTING
      }

      private void OnLevelWasLoaded(int level) {
        if (SceneManager.GetActiveScene().name == "Gaming")
          StartCoroutine("EndGame");
      }

      public override void OnLeftRoom() {
        if (SceneManager.GetActiveScene().name == "Gaming") {
          PhotonNetwork.Disconnect();
          PhotonNetwork.LoadLevel(0);
        }
      }*/
}
