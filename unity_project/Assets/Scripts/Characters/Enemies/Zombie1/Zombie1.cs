using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1 : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    public int Zombie1HP = 100;
    public Animator animator;
    PlayerManager playerManager;

    void Start()
    {
        FindTarget(); // Call to find the target
        IgnoreCollisions(); // Call to ignore collisions with the player
    
        // Find the PlayerManager instance
        playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("PlayerManager not found in the scene.");
        }

        // Ensure the animator is assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the Zombie.");
            }
        }
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
        if(target != null)
        {
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
        AudioEnemy.instance.Play("Hurt");
        if (Zombie1HP <= 0)
        {
            Die();
        }
        else
        {
            if(animator != null)
            {
                animator.SetTrigger("Damage4");
            }
        }
    }

    void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("deth4");
            StartCoroutine(DisableGameObject());
        }
        if (playerManager != null)
        {
            playerManager.AddPoints(10);
            playerManager.enemyKills++;
        }
        else
        {
            Debug.LogWarning("PlayerManager not found. Points not added.");
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        else
        {
            Debug.LogWarning("Collider2D not found on the Zombie1.");
        }

         
    }

    private IEnumerator DisableGameObject()
    {
        yield return new WaitForSeconds(2.0f); // Adjust the wait time if needed
        //gameObject.SetActive(false);
        this.enabled = false;
    }

    public void PlayerDamage()
    {
        if (target != null)
        {
            PlayerCollision playerCollision = target.GetComponent<PlayerCollision>();
            if (playerCollision != null && HealthManager.health > 0)
            {
                AudioEnemy.instance.Play("Punch");
                playerCollision.PlayerTakeDamage();
            }
        }
        else
        {
            Debug.LogWarning("Player not found. Cannot damage player.");
        }
    }
}
