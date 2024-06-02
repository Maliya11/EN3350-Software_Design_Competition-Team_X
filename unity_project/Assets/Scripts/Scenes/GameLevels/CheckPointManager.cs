using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Update playerSafePosition of Player Manager to the current position of the checkpoint
            PlayerManager.playerSafePosition = transform.position;
            Debug.Log("Player safe position updated to: " + PlayerManager.playerSafePosition);        
        }
    }
}
