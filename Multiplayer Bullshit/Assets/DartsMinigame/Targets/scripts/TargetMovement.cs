using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public GameObject target;
    public Camera MainCamera;
    private Vector2 screenBounds;
    private Vector2 randomPosition;
    public float timer = 0f;
    [SerializeField] public float speed;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        // Debug.Log(screenBounds);
        randomPosition = new Vector2(Random.Range(-670, 1070), Random.Range(-1050, -100));
        speed = Random.Range(50, 200);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, randomPosition, speed * Time.deltaTime);
        if (timer > 2.0f){
            randomPosition = new Vector2(Random.Range(-670,1070), Random.Range(-1050,-100));
           // Debug.Log(randomPosition);
            timer = 0f;
            Debug.Log(Random.Range(-670,1070));
            Debug.Log(Random.Range(-1050,-100));

        }
      
    }
}
