using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    /*
    This script is for the bat enemy. The bat enemy will move left and right within a certain range.
    */

    // Variables
    public float speed = 0.8f;
    public float range = 3;
    float startingX;
    int dir =1;

    // Start is called before the first frame update
    void Start()
    {
        // Get the starting x position of the bat
        startingX = transform.position.x;
        transform.localScale = new Vector2(4f,3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move the bat left and right
        transform.Translate(Vector2.right * speed * Time.deltaTime*dir);
        if(transform.position.x >= startingX + range)
        {
            // If the bat reaches the right end of the range, change direction
            dir *= -1;
            transform.localScale = new Vector2(-4f,3f);
        }
        else if(transform.position.x <= startingX - range)
        {
            // If the bat reaches the left end of the range, change direction
            dir *= -1;
            transform.localScale = new Vector2(4f,3f);
        }
    }

    public void BatTakeDamage(int damage)
    {
        // Player does not take damage
    }
}
