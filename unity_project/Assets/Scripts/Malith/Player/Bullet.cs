using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if(collision.tag == "Golem")
        {
            //collision.GetComponent<Golem>().TakeDamage(25);
        }
    } 

    
}
