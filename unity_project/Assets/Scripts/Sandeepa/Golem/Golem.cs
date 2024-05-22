using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
}
