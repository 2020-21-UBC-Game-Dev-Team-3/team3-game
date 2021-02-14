using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmTrapScript : MonoBehaviour
{
    public GameObject wires;
    int[] wireOrder;
    int count;
    int index;
    Ray ray;
    RaycastHit clickHit;
    public bool waiting;
    public GameObject endCanvas;

    // Start is called before the first frame update
    void Start()
    {
        endCanvas.SetActive(false);
        System.Random rnd = new System.Random();

        wireOrder = new int[6];
        
        for (int i = 0; i < wireOrder.Length; i++)
        {
            wireOrder[i] = rnd.Next(0, 4);
        }

        index = 0;
        StartCoroutine(Wait(0.3f));
    }

    // Update is called once per frame
    void Update()
    {

        
        if (Input.GetMouseButtonDown(0) )
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out clickHit, 100.0f))
            {
                if (clickHit.transform.GetSiblingIndex() == wireOrder[count])
                {
                    print(count);
                    count++;
                } else
                {
                    count = 0;
                    StartCoroutine(Wait(0.3f));
                }
            }
        }



        if (count == wireOrder.Length)
        {
            print("yay");
            endCanvas.SetActive(true);
        }
    }


    IEnumerator Wait(float time)
    {
        waiting = true;
        yield return new WaitForSeconds(time);
        for (int i = 0; i < wireOrder.Length; i++)
        {
            wires.transform.GetChild(wireOrder[i]).transform.gameObject.SetActive(true);

            yield return new WaitForSeconds(time);
            wires.transform.GetChild(wireOrder[i]).transform.gameObject.SetActive(false);
            yield return new WaitForSeconds(time);

        }
        waiting = false;

    }
}