using UnityEngine;

public class FallZone : MonoBehaviour
{
    public float backDistance = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position += new Vector3(-backDistance, 0f, 0f);

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}