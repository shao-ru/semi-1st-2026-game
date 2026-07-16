using UnityEngine;

public class SlowArea : MonoBehaviour
{
    public float slowSpeed = 2f;
    private float originalSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove player = other.GetComponent<PlayerMove>();

            if (player != null)
            {
                originalSpeed = player.moveSpeed;
                player.moveSpeed = slowSpeed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove player = other.GetComponent<PlayerMove>();

            if (player != null)
            {
                player.moveSpeed = originalSpeed;
            }
        }
    }
}