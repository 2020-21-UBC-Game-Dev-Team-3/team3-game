using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseButton : MonoBehaviour
{
    [SerializeField] PlayerActionController pac;
    [SerializeField] Image useButtonImage;

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