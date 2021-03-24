using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkinSet : MonoBehaviour
{
    [SerializeField] List<Sprite> skinImages;
    [SerializeField] GameObject uiSkinImage;

    public void ChangeActiveUISkinImage(int index)
    {
        uiSkinImage.GetComponent<Image>().sprite = skinImages[index];
    }
}
