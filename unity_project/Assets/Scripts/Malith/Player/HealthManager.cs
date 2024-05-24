using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static int health = 3;
    private int previousHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Awake()
    {
        health = 3;
        previousHealth = health;
        UpdateHearts();
    }
    void Update()
    {
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
