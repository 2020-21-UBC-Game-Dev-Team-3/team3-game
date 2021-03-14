using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapChanceScript : MonoBehaviour
{
    //Transform[] gears;
    int safe;
    RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        //gears = GetComponentsInChildren<Transform>();
        System.Random rnd = new System.Random();
        safe = rnd.Next(0, 5);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider.gameObject.tag == "Gear")
            {
                if (hit.transform.GetSiblingIndex() == safe)
                {
                    print("you lived");
                }
                else
                {
                    print("ded");
                }
            }
        }

    }

    
}
