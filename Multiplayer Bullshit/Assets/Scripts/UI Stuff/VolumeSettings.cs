using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Slider))]

public class VolumeSettings : MonoBehaviour
{
    public delegate void EndSliderDragEventHandler(float val);
    // Start is called before the first frame update
[SerializeField] string nameofkey;
    void Start()
    {
        
    }

    public event EndSliderDragEventHandler EndDrag;

    private float SliderValue
    {
        get
        {
            return gameObject.GetComponent<Slider>().value;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (EndDrag != null)
        {
            EndDrag(SliderValue);
            PlayerPrefs.SetFloat(nameofkey, SliderValue);
        }
    }
}
