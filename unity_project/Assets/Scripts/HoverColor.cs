using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverColor : MonoBehaviour
{
    // UI elements
    public Button button;
    public Color wantedColor;
    private Color originalColor;
    private ColorBlock cb;


    private void Start()
    {
        // Get the original color of the button
        cb = button.colors;
        originalColor = cb.selectedColor;
    }

    public void ChangeWhenHover()
    {
        // Change the color of the button to the desired color, when the mouse hovers over it
        cb.selectedColor = wantedColor;
        button.colors = cb;
    }

    public void ChangeWhenExit()
    {
        // Change the color of the button back to the original color, when the mouse exits
        cb.selectedColor = originalColor;
        button.colors = cb;
    }
}
