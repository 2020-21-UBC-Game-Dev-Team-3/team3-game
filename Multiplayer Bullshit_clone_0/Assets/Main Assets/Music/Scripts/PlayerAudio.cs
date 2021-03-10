using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAudio : MonoBehaviour
{
    public AudioListener audioListener;
    public AudioSource Footsteps;
    public AudioSource Music;
    private PhotonView pv;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            audioListener.GetComponent<AudioListener>().enabled = false;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("MusicZone"))
        {
            Music.mute = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("MusicZone"))
        {
            Music.mute = false;
        }
    }
    private void Update()
    {
        if (!pv.IsMine) return;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if(horizontal != 0 || vertical != 0)
        {
            pv.RPC("MuteFoot", RpcTarget.All, false);
        }
        else
        {
            pv.RPC("MuteFoot", RpcTarget.All, true);
        }
    }
    [PunRPC]
    private void MuteFoot(bool boolean)
    {
        Footsteps.mute = boolean;
    }
}
