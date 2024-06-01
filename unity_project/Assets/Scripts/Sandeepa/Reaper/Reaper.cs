using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    public int GolemHP = 100;
    public Animator animator;
    PlayerManager playerManager;
    // Start is called before the first frame update
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
                Debug.LogError("Animator component not found on the Reaper.");
            }
        }
    }

    // Update is called once per frame
    void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Reaper cannot detect player position.");
        }
    }

    void IgnoreCollisions()
    {
        Collider2D ReaperCollider = GetComponent<Collider2D>();
        if(target != null)
        {
            Collider2D playerCollider = target.GetComponent<Collider2D>();
            if (ReaperCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, ReaperCollider);
            }
            else
            {
                Debug.LogWarning("Reaper or player collider not found. Ignoring collision failed.");
            }
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Update Reaper's scale based on player position
            transform.localScale = new Vector2(target.position.x > transform.position.x ? 1.5f : -1.5f, 1.5f);
        }
    }

    public void ReaperTakeDamage(int damage)
    {
        GolemHP -= damage;
        AudioEnemy.instance.Play("Hurt");
        if (GolemHP <= 0)
        {
            Die();
        }
        else
        {
            if(animator != null)
            {
                animator.SetTrigger("Damage5");
            }
        }
    }

    void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("deth5");
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
            Debug.LogWarning("Collider2D not found on the Reaper.");
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
                playerCollision.PlayerTakeDamage();
            }
        }
        else
        {
            Debug.LogWarning("Player not found. Cannot damage player.");
        }
    }
}
