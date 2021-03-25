using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shuriken : MonoBehaviour
{
    public Transform target;
    PhotonView pv;
    public AudioSource audioSource;

    private void Awake() { 
        pv = GetComponent<PhotonView>();
        audioSource.volume = PlayerPrefs.GetFloat("main volume");
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Role>().currRole == Role.Roles.Crewmate)
        {
            pv.RPC("DestroyShuriken", RpcTarget.All);
            other.gameObject.GetComponent<IDamageable>()?.TakeHit();
        }
    }

    [PunRPC]
    void DestroyShuriken() => Destroy(gameObject);
}