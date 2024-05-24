using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {

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
