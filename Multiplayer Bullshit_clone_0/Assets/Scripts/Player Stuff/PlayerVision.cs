using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerVision : MonoBehaviour
{
    public float visionRadius;
    private PhotonView pv;

  //NOTE: LOTS OF DUPLICATION DUE TO UNITY CHAN. ONCE YOU REPLACE OBJECT SOME OF THIS CODE MIGHT NOT WORK PROPERLY
  //SO ADJUST UNITY CHAN APPROPIATELY

  private void Awake() {
    pv = GetComponent<PhotonView>();
  }

  // Start is called before the first frame update
  void Start()
    {
        pv = GetComponent<PhotonView>();
        GetComponent<SphereCollider>().radius = visionRadius;
        if (pv.IsMine) EnableChildren(gameObject);
    }

    void EnableChildren(GameObject c)
    {
        for (int i = 0; i < c.transform.childCount; i++)
            c.transform.GetChild(i).gameObject.SetActive(true);
    }

    void DisableChildren(GameObject c)
    {
        for (int i = 0; i < c.transform.childCount; i++)
            c.transform.GetChild(i).gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (pv.IsMine && collider.gameObject != gameObject && collider.gameObject.tag == "Player") EnableChildren(collider.gameObject);
        Debug.Log(collider.gameObject);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (pv.IsMine && collider.gameObject != gameObject && collider.gameObject.tag == "Player") DisableChildren(collider.gameObject);
    }
}
