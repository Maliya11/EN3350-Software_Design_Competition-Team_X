using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LightChange : MonoBehaviour
{
    public Slider slider;     // Slider for the light intensity
    public Light2D lights;    // Light to change the intensity of
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lights.intensity = slider.value;        // Set the intensity of the light to the value of the slider
    }
}
