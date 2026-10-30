using UnityEngine;
using System.Collections;

public class Poop2 : MonoBehaviour
{
    [Header("移動速度低下")]
    [SerializeField] private float slowSpeed = 2f;

    [Header("速度低下の時間")]
    [SerializeField] private float slowDuration = 3f;

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
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                // 元の移動速度を保存
                float originalSpeed = player.moveSpeed;

                // 移動速度を遅くする
                player.moveSpeed = slowSpeed;

                // プレイヤー側で元の速度に戻す処理を実行
                player.StartCoroutine(
                    RestoreSpeed(player, originalSpeed)
                );
            }

            // 糞を消す
            Destroy(gameObject);
        }
    }

    private IEnumerator RestoreSpeed(Player player, float originalSpeed)
    {
        // 指定時間待つ
        yield return new WaitForSeconds(slowDuration);

        // 元の速度に戻す
        if (player != null)
        {
            player.moveSpeed = originalSpeed;
        }
    }
}