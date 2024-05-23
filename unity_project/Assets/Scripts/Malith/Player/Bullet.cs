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
            Destroy(gameObject);
            collision.GetComponent<Golem>().GolemTakeDamage(25);
        }
    }
}
