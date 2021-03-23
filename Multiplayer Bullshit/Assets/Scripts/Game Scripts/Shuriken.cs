using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shuriken : MonoBehaviour
{
    public bool shurikenLaunched;

    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 3f);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<Role>().currRole == Role.Roles.Crewmate)
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeHit();
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
