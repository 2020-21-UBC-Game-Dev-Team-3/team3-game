using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class TrapManager : MonoBehaviour {

  [SerializeField] PhotonView pv;

  public void InstantiateTrap(Vector3 position) {
    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), position, Quaternion.identity);
  }

  public void DestroyTrap(GameObject trap) {
    pv.RPC("DestroyTrapRPC", RpcTarget.All, trap);
  }

  [PunRPC]
  public void DestroyTrapRPC(GameObject trap) {
    PhotonNetwork.Destroy(trap);
  }
}
