using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(gameObject);
        }

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
                Minotaur.GolemTakeDamage(25);
            }
            Destroy(gameObject);
        }

        
    }
}
