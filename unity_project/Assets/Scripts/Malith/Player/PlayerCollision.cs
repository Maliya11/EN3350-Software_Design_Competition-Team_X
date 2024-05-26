using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Animator animator;
    private int playerLayer;
    private int enemyLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("player");
        enemyLayer = LayerMask.NameToLayer("enemy");
    }

    public void PlayerTakeDamage()
    {
        HealthManager.health--;
        if(HealthManager.health <= 0)
        {
            StartCoroutine(Dead());
        }
        else
        {
            StartCoroutine(GetHurt());
        }   
    }

    IEnumerator Dead()
    {
        if(animator == null) yield break;

        if(gameObject.activeSelf)
        {
        animator.SetTrigger("isDead");
        animator.SetTrigger("dead");
        yield return new WaitForSeconds(1);

        gameObject.SetActive(false);
        PlayerManager.isGameOver = true;
        }

        else
        {
            Debug.LogWarning("Cannot start coroutine: GameObject is inactive.");
        }
    }

    IEnumerator GetHurt()
    {
        if(animator == null) yield break;

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        // Debug.Log("one");
        // GetComponent<Animator>().SetLayerWeight(1, 1);

        yield return new WaitForSeconds(3);

        // GetComponent<Animator>().SetLayerWeight(1, 0);
        // Debug.Log("two");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }
}

