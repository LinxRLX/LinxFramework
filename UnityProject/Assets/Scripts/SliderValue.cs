using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SliderValue : MonoBehaviour
{
    public Text text;
    public Slider slider;
    void Update()
    {
        text.text = slider.value.ToString(CultureInfo.CurrentCulture);
    }
}
