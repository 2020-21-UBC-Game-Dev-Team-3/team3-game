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
    void Awake() => pv = GetComponent<PhotonView>();

    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            topFloorPlane = GameObject.Find("Top Floor Plane");
            middleFloorPlane = GameObject.Find("Middle Floor Plane");
            bottomFloorPlane = GameObject.Find("Bottom Floor Plane");

            PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>().mapFloorPlanes.AddRange(new GameObject[] { topFloorPlane, middleFloorPlane, bottomFloorPlane });

        }
        else
        {
            List<GameObject> mapFloorPlanes = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>().mapFloorPlanes;
            foreach (var floorPlane in mapFloorPlanes)
            {
                switch (floorPlane.name)
                {
                    case "Top Floor Plane":
                        topFloorPlane = floorPlane;
                        break;

                    case "Middle Floor Plane":
                        middleFloorPlane = floorPlane;
                        break;

                    case "Bottom Floor Plane":
                        bottomFloorPlane = floorPlane;
                        break;
                }
            }
        }
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

    //public void InitiateMapReset()
    //{
    //    StartCoroutine(ResetMap());
    //}

    public void ResetMap()
    {
        topFloorPlane.SetActive(true);
        middleFloorPlane.SetActive(true);
        bottomFloorPlane.SetActive(true);
    }
}
