using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPopUp : MonoBehaviour
{
    [SerializeField] GameObject map;
    
    public void Open()
    {
        map.SetActive(true);
    }

    public void Close()
    {
        map.SetActive(false);
    }
}
