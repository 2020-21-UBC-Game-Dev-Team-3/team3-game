using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UseButton : MonoBehaviour
{
    [SerializeField] PlayerActionController pac;
    [SerializeField] Image useButtonImage;
    [SerializeField] GameObject useButtonObject;

    private void Start()
    {
        if (!GetComponentInParent<PhotonView>().IsMine)
        {
            Destroy(useButtonObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pac.interactable != null)
        {
            useButtonImage.color = Color.white;
        }
        else
        {
            useButtonImage.color = Color.gray;
        }
    }
}