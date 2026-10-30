using UnityEngine;

public class Poop : MonoBehaviour
{
    private bool landed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面に当たったとき
        if (!landed && collision.gameObject.CompareTag("Ground"))
        {
            landed = true;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Static;
            }

            // 1秒後に糞を消す
            Destroy(gameObject, 1f);
        }

        // プレイヤーに当たったとき
        if (collision.gameObject.CompareTag("Player"))
        {
            // 画面を汚す
            if (PoopEffect.Instance != null)
            {
                PoopEffect.Instance.ShowDirt();
            }

            // 糞を消す
            Destroy(gameObject);
        }
    }
}
///カラスの糞が水筒の蓋にかかったことある。不運ですわ…