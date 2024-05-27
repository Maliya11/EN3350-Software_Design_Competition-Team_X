using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    public int MinotaurHP = 100;
    public Animator animator;
    PlayerManager playerManager;
    // Start is called before the first frame update
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
            Debug.LogWarning("Player not found. Minotaur cannot detect player position.");
        }
    }

    void IgnoreCollisions()
    {
        Collider2D minotaurCollider = GetComponent<Collider2D>();
        Collider2D playerCollider = target.GetComponent<Collider2D>();
        if (minotaurCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, minotaurCollider);
        }
        else
        {
            Debug.LogWarning("Minotaur or player collider not found. Ignoring collision failed.");
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

    public void MinotaurTakeDamage(int damage)
    {
        MinotaurHP -= damage;
        if (MinotaurHP <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Damage1");
        }
    }

    void Die()
    {
        animator.SetTrigger("deth1");
        playerManager.numberOfPoints += 10;
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
