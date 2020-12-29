using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private int currChance;
    private float minX = 1f;
    private float maxX = 8f;
    private int[] speeds = { 1, 1, 10, 10, 20 };
    private bool movingLeft;


    // Start is called before the first frame update
    void Start()
    {
        movingLeft = true;
        movingLeft = Random.Range(0.0f, 1.0f) <= 0.5;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x >= maxX)
            {
                movingLeft = false;
            }
        }
        else
        {
            if (transform.position.x <= minX)
            {
                movingLeft = true;
            }
        }


        if (movingLeft)
        {
            transform.Translate(Vector2.left * speeds[Random.Range(0, speeds.Length)] * Time.deltaTime);
        } else
        {
            transform.Translate(Vector2.right * speeds[Random.Range(0, speeds.Length)] * Time.deltaTime);
        }
    }
}
