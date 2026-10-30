using UnityEngine;

public class Crow : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float patrolDistance = 10f;

    private Rigidbody2D rb;
    private float startX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startX = transform.position.x;
    }

    private void FixedUpdate()
    {
        Walk();
        CheckTurn();
    }

    private void Walk()
    {
        if (transform.localScale.x > 0)
        {
            rb.linearVelocity = new Vector2(-moveSpeed, 0f);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveSpeed, 0f);
        }
    }

   private void CheckTurn()
    {
       if (transform.localScale.x > 0 &&
         transform.position.x <= startX - patrolDistance)
      {
           Flip();
       }

       if (transform.localScale.x < 0 &&
           transform.position.x >= startX + patrolDistance)
       {
           Flip();
       }
    }
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = Application.isPlaying
            ? new Vector3(startX, transform.position.y)
            : transform.position;

        Gizmos.DrawLine(
            center + Vector3.left * patrolDistance,
            center + Vector3.right * patrolDistance);
    }
}
///Happy Halloween! Trick or treat! カボチャよこせや 