using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Animator animator;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            PlayerTakeDamage();  
        }

        if(collision.transform.tag == "Water")
        {
            HealthManager.health = 0;
            animator.SetTrigger("isDead");
            PlayerManager.isGameOver = true;
            gameObject.SetActive(false);
        }
        
    }

    public void PlayerTakeDamage()
    {
        HealthManager.health--;
        if(HealthManager.health <= 0)
        {
            animator.SetTrigger("isDead");
            PlayerManager.isGameOver = true;
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(GetHurt());
        }   
    }

    IEnumerator GetHurt()
    {
        Physics2D.IgnoreLayerCollision(7, 8);
        GetComponent<Animator>().SetLayerWeight(1, 1);
        yield return new WaitForSeconds(3);
        GetComponent<Animator>().SetLayerWeight(1, 0);
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

}

