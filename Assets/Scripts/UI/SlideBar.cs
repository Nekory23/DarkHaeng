using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(int value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void SetCurrentValue(int value)
    {
        slider.value = value;
    }
}
