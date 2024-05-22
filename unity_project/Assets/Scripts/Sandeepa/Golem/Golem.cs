using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    public int GolemHP = 100;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        if(target.position.x > transform.position.x)
       {
           transform.localScale = new Vector2(1.5f,1.5f);
       }
       else
       {
            transform.localScale = new Vector2(-1.5f,1.5f);
       }
    }

    public void TakeDamage(int damage)
    {
        GolemHP -= damage;
        Debug.Log("Golem HP: " + GolemHP);
        if(GolemHP > 0)
        {
            animator.SetTrigger("Damage");
        }
        else{
            animator.SetTrigger("deth");
            GetComponent<CapsuleCollider2D>().enabled = false;
            this.enabled = false;

        }
    }
}
