using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1 : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    public int Zombie1HP = 100;
    public Animator animator;

    void Start()
    {
        FindTarget(); // Call to find the target
        IgnoreCollisions(); // Call to ignore collisions with the player
    }

    void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Zombie1 cannot detect player position.");
        }
    }

    void IgnoreCollisions()
    {
        Collider2D zombie1Collider = GetComponent<Collider2D>();
        Collider2D playerCollider = target.GetComponent<Collider2D>();
        if (zombie1Collider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, zombie1Collider);
        }
        else
        {
            Debug.LogWarning("Zombie1 or player collider not found. Ignoring collision failed.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Update golem's scale based on player position
            transform.localScale = new Vector2(target.position.x > transform.position.x ? 1.5f : -1.5f, 1.5f);
        }
    }

    public void Zombie1TakeDamage(int damage)
    {
        Zombie1HP -= damage;
        if (Zombie1HP <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Damage4");
        }
    }

    void Die()
    {
        animator.SetTrigger("deth4");
        PlayerManager.numberOfPoints += 10;
        GetComponent<CapsuleCollider2D>().enabled = false;
        this.enabled = false;
    }

    public void PlayerDamage()
    {
        if (target != null)
        {
            PlayerCollision playerCollision = target.GetComponent<PlayerCollision>();
            if (HealthManager.health > 0)
            {
                playerCollision.PlayerTakeDamage();
            }
        }
        else
        {
            Debug.LogWarning("Player not found. Cannot damage player.");
        }
    }
}
