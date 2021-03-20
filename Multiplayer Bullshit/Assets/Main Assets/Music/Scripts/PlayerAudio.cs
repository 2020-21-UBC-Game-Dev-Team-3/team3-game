using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAudio : MonoBehaviour
{
    public AudioListener audioListener;
    public AudioSource Footsteps;
    public AudioSource Music;
    public AudioSource BarMusic;
    private PhotonView pv;
    private float threshold;
    private bool inBar;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            Destroy(audioListener);
            Destroy(Music);
            Destroy(BarMusic);
        }
        StartCoroutine(LateStart(0.1f));
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("MusicZone"))
        {
            Music.mute = true;
            threshold = 0;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BarMusicCollider")
        {
            BarMusic.mute = false;
            inBar = true;
            threshold = 0;
            Music.mute = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MusicZone"))
        {
            threshold = 0;
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("MusicZone"))
        {
            threshold = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BarMusicCollider")
        {
            inBar = false;
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
        threshold++;
        if(threshold > 20)
        {
            Music.mute = false;
        }
        if (!inBar && threshold > 20)
        {
            BarMusic.mute = true;
        }
    }
    [PunRPC]
    private void MuteFoot(bool boolean)
    {
        Footsteps.mute = boolean;
    }
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Music.Play();
    }
}
