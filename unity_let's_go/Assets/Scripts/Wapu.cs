using UnityEngine;

public class WarpOnCollision2D : MonoBehaviour
{
    [Header("ワープ先の座標")]
    [SerializeField] private Vector3 targetPosition = new Vector3(0f, 0f, 0f);

    // Is Trigger にチェックを入れている場合
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Warp(other.gameObject);
        }
    }

    // Is Trigger にチェックを入れていない（物理衝突する）場合
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Warp(collision.gameObject);
        }
    }

    private void Warp(GameObject target)
    {
        target.transform.position = targetPosition;
    }
}