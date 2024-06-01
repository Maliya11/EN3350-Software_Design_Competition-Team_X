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
        if(PlayerManager.isNinja)
        {
            AudioManagerPlayer.instance.Play("NinjaDeath");
        }
        else
        {
            AudioManagerPlayer.instance.Play("RobotDeath");
        }
        
        yield return new WaitForSeconds(2);

        animator.SetTrigger("backToIdle");
        PlayerManager.isPlayerDead = true;
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
        GetComponent<Animator>().SetLayerWeight(1, 1);

        yield return new WaitForSeconds(3);

        GetComponent<Animator>().SetLayerWeight(1, 0);
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }
}

