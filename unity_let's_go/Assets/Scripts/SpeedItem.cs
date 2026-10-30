using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.SpeedUp();
            }

            Destroy(gameObject);
        }
    }
}