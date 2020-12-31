using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapManager : MonoBehaviour
{
    public GameObject player;

    public GameObject topFloorPlane;
    public GameObject middleFloorPlane;
    public GameObject bottomFloorPlane;

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

        if(Math.Round(player.transform.position.y) == Math.Round(middleFloorPlane.transform.position.y))
        {
            topFloorPlane.SetActive(false);
            middleFloorPlane.SetActive(true);
            bottomFloorPlane.SetActive(false);
        } else if(Math.Round(player.transform.position.y) == Math.Round(topFloorPlane.transform.position.y))
        {
            topFloorPlane.SetActive(true);
            middleFloorPlane.SetActive(false);
            bottomFloorPlane.SetActive(false);
        } else if(Math.Round(player.transform.position.y) == Math.Round(bottomFloorPlane.transform.position.y))
        {
            topFloorPlane.SetActive(false);
            middleFloorPlane.SetActive(false);
            bottomFloorPlane.SetActive(true);
        }
    }
}
