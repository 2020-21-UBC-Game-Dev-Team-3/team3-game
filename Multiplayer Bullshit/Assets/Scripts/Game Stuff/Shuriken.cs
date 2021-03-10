using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shuriken : MonoBehaviour
{
    public bool shurikenLaunched;

    public Vector3 target;

    // Update is called once per frame
    void Update()
    {
        if(shurikenLaunched)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 3f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeHit();
        }
        Destroy(gameObject);
    }

}
