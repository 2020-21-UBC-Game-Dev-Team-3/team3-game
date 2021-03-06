using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource Footsteps;
    public AudioSource Music;
    private PhotonView pv;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MusicMaker"))
        {
            Music.mute = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MusicMaker"))
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
            Footsteps.mute = false;
        }
        else
        {
            Footsteps.mute = true;
        }
    }
}
