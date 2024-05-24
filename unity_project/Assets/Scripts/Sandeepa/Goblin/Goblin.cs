using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    public int GoblinHP = 100;
    public Animator animator;
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
            Debug.LogWarning("Player not found. Goblin cannot detect player position.");
        }
    }

    void IgnoreCollisions()
    {
        Collider2D goblinCollider = GetComponent<Collider2D>();
        Collider2D playerCollider = target.GetComponent<Collider2D>();
        if (goblinCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, goblinCollider);
        }
        else
        {
            Debug.LogWarning("Golem or player collider not found. Ignoring collision failed.");
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

    public void GoblinTakeDamage(int damage)
    {
        GoblinHP -= damage;
        if (GoblinHP <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Damage2");
        }
    }

    void Die()
    {
        animator.SetTrigger("deth2");
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
