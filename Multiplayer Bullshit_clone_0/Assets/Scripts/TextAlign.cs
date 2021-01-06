using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAlign : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera mainCamera;
    void Start()
    {
        Vector3 vec = new Vector3(-0.01f, 0.01f, 0.01f);
        mainCamera = Camera.main;
        transform.localScale = (vec);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCamera.transform);
    }
}
