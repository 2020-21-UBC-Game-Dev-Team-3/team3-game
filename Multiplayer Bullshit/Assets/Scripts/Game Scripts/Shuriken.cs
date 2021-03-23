using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shuriken : MonoBehaviour
{
    public bool shurikenLaunched;

    public Transform target;

    PhotonView pv;

    private void Awake() => pv = GetComponent<PhotonView>();

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 3f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Role>().currRole == Role.Roles.Crewmate)
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeHit();
            pv.RPC("DestroyShuriken", RpcTarget.All);
        }
    }

    [PunRPC]
    void DestroyShuriken()
    {
        Debug.Log("nani the fuck");
        Destroy(gameObject);
    }
}