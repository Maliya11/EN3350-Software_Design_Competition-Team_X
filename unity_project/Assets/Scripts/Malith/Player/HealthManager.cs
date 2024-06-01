using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    /*
    This script is used to update the health container in the canvas to show the updated health status of the player
    */
    public static int health = 3;  //this is the max health
    private int previousHealth;
    public Image[] hearts;  //assign the heart images to this array from the inspector
    public Sprite fullHeart;  //red full heart image
    public Sprite emptyHeart;  //empty heart image

    void Awake()
    {
        //At the starting of the game health is full and the heart container in the canvas is updated
        health = 3;
        previousHealth = health;
        UpdateHearts();  //update the canvas according to the "health" count
    }
    void Update()
    {
        //if the previos heart count is not equal to the current change the canvas
        if (health != previousHealth)
        {
            UpdateHearts();
            previousHealth = health;
        }
    }

    void UpdateHearts()
    {
        // Set all hearts to empty
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }

        // Set the hearts according to the current health
        for (int i = 0; i < health; i++)
        {
            if (i < hearts.Length) // Ensure index is within bounds
            {
                hearts[i].sprite = fullHeart;
            }
        }
    }
}
