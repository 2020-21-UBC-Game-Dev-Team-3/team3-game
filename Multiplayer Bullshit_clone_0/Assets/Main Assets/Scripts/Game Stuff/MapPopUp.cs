using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPopUp : MonoBehaviour
{
    [SerializeField] GameObject map;

    bool mapOpen;

    void Update()
    {
        if (Input.GetKeyDown("tab") && !mapOpen) OpenMap();
        else if (Input.GetKeyDown("tab") && mapOpen) CloseMap();
    }

    public void OpenMap()
    {
        map.SetActive(true);
        mapOpen = true;
    }

    public void CloseMap()
    {
        map.SetActive(false);
        mapOpen = false;
    }
}
