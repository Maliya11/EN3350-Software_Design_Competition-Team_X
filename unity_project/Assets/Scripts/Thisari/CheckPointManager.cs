using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    PlayerManager playerManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Update playerSafePosition of Player Manager to the current position of the checkpoint
            playerManager.playerSafePosition = transform.position;
            Debug.Log("Player safe position updated to: " + playerManager.playerSafePosition);        
        }
    }
}
