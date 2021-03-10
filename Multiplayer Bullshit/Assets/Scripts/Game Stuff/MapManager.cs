using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapManager : MonoBehaviour
{
    public GameObject player;

    GameObject topFloorPlane;
    GameObject middleFloorPlane;
    GameObject bottomFloorPlane;

    PhotonView pv;

    void Start()
    {
        topFloorPlane = GameObject.Find("Top Floor Plane");
        middleFloorPlane = GameObject.Find("Middle Floor Plane");
        bottomFloorPlane = GameObject.Find("Bottom Floor Plane");

        pv = player.GetComponent<PhotonView>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return;

        float playerPlanePos = Mathf.Round(player.transform.position.y);
        topFloorPlane.SetActive((Mathf.Round(topFloorPlane.transform.position.y)) == playerPlanePos);
        middleFloorPlane.SetActive((Mathf.Round(middleFloorPlane.transform.position.y)) == playerPlanePos);
        bottomFloorPlane.SetActive((Mathf.Round(bottomFloorPlane.transform.position.y)) == playerPlanePos);
    }

    public void ResetMap()
    {
        topFloorPlane.SetActive(true);
        middleFloorPlane.SetActive(true);
        bottomFloorPlane.SetActive(true);
    }
}
