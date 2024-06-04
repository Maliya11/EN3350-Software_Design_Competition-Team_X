using JetBrains.Annotations;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /*
    This script is used to identify whether the bullet objects hits an enemy
    */
    public float maxTravelDistance = 10f; // Maximum travel distance of the bullet
    private Vector2 initialPosition; // Initial position of the bullet

    private void Start()
    {
        initialPosition = transform.position; // Gets the starting position of the bullet
    }

    private void Update()
    {
        // Monitor the distance tarveled by the bullet
        // and destroy it if it reaches the maximum travel distance
        float distanceTraveled = Vector2.Distance(initialPosition, transform.position);
        if(distanceTraveled > maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // In each code block checks the tag of the object that the bullet collided with, 
        // and give damage to that object
        if(collision.tag == "Golem")
        {
            Golem golem = collision.GetComponent<Golem>();
            if(golem != null)
            {
                golem.GolemTakeDamage(25);
            }
            Destroy(gameObject);
        }

        if(collision.tag == "Minotaur")
        {
            Minotaur Minotaur = collision.GetComponent<Minotaur>();
            if(Minotaur != null)
            {
                Minotaur.MinotaurTakeDamage(25);
            }
            Destroy(gameObject);
        }

        if(collision.tag == "Goblin")
        {
            Goblin Goblin = collision.GetComponent<Goblin>();
            if(Goblin != null)
            {
                Goblin.GoblinTakeDamage(25);
            }
            Destroy(gameObject);
        }

        if(collision.tag == "Zombie")
        {
            Zombie Zombie = collision.GetComponent<Zombie>();
            if(Zombie != null)
            {
                Zombie.ZombieTakeDamage(25);
            }
            Destroy(gameObject);
        }

        if(collision.tag == "Zombie1")
        {
            Zombie1 Zombie1 = collision.GetComponent<Zombie1>();
            if(Zombie1 != null)
            {
                Zombie1.Zombie1TakeDamage(25);
            }
            Destroy(gameObject);
        }

        if(collision.tag == "Reaper")
        {
            Reaper Reaper = collision.GetComponent<Reaper>();
            if(Reaper != null)
            {
                Reaper.ReaperTakeDamage(25);
            }
            Destroy(gameObject);
        }

        if(collision.tag == "Bat")
        {
            Bat bat = collision.GetComponent<Bat>();
            if(bat != null)
            {
                bat.BatTakeDamage(25);
            }
            Destroy(gameObject);
        }  
    }
}
