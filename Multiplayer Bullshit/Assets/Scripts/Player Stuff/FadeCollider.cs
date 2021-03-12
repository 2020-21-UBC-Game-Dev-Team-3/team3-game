using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeCollider : MonoBehaviour
{
    [SerializeField] private float fadeInAmount;
    [SerializeField] private PhotonView pv;
    [SerializeField] private GameObject mainPlayer;
    [SerializeField] private TextMeshProUGUI nameText;

    void Start()
    {
        if (pv.IsMine)
        {
            Renderer[] playerMaterials = mainPlayer.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in playerMaterials)
            {
                Color color = renderer.material.color;
                if (color.a != 1f)
                    color.a = 1f;
                renderer.material.color = color;
            }
            Color nameColor = nameText.color;
            nameColor.a = 1f;
            nameText.color = nameColor;
        }
    }
    void Fade(GameObject c, Func<float, float, float> method)
    {
        Renderer[] playerMaterials = c.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in playerMaterials)
        {
            Color color = renderer.material.color;
            color.a = method(color.a, fadeInAmount / 255f);
            renderer.material.color = color;
        }
    }

    float Add(float a, float b)
    {
        return a + b;
    }

    float Subtract(float a, float b)
    {
        return Mathf.Abs(b - a);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (pv.IsMine && collider.gameObject != mainPlayer && collider.gameObject.tag == "Player")
        {
            Fade(collider.gameObject, Add);/*
            Color nameColor = nameText.color;
            nameColor.a = 1f;
            nameText.color = nameColor;*/
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (pv.IsMine && collider.gameObject != mainPlayer && collider.gameObject.tag == "Player")
        {
            Fade(collider.gameObject, Subtract);
/*            Color nameColor = nameText.color;
            nameColor.a = 0f;
            nameText.color = nameColor;*/
        }
    }
}
