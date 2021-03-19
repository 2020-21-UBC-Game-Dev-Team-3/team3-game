using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkinSelect : MonoBehaviour
{
    [SerializeField] Renderer playerMesh;
    [SerializeField] List<Material> allPlayerSkins;

    PhotonView pv;

    void Start() => pv = GetComponent<PhotonView>();

    public void SetCharacterSkin() => pv.RPC("ApplyCharacterSkin", RpcTarget.All);

    [PunRPC]
    void ApplyCharacterSkin()
    {
        //if (!pv.IsMine) return;
        string assignedSkinName = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>().playerSkinName;
        switch (assignedSkinName)
        {
            case "SBetty":
                playerMesh.material = allPlayerSkins[0];
                break;            
            
            case "SBetty2":
                playerMesh.material = allPlayerSkins[1];
                break;            
            
            case "SCaptain":
                playerMesh.material = allPlayerSkins[2];
                break;            
            
            case "SCaptain2":
                playerMesh.material = allPlayerSkins[3];
                break;            
            
            case "SChef":
                playerMesh.material = allPlayerSkins[4];
                break;            
            
            case "SChef2":
                playerMesh.material = allPlayerSkins[5];
                break;            
            
            case "SJacob":
                playerMesh.material = allPlayerSkins[6];
                break;            
            
            case "SJacob2":
                playerMesh.material = allPlayerSkins[7];
                break;            
            
            case "SLifeguard":
                playerMesh.material = allPlayerSkins[8];
                break;            
            
            case "SLifeguard2":
                playerMesh.material = allPlayerSkins[9];
                break;
        }

        //switch (skin.name)
        //{
        //    default:
        //        break;
        //}
    }



}
