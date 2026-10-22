using UnityEngine;

public class Player : MonoBehaviour
{
    // 移動スピード
    [SerializeField] private float moveSpeed = 10.0f;

    // ジャンプ力
    [SerializeField] private float jumpForce = 15.0f;

    // プレイヤーの足元にある子オブジェクト
    [SerializeField] private Transform groundChecker;

    // 地面をチェックする円の半径
    [SerializeField] private float checkerRadius = 0.1f;

    // 地面のレイヤー
    [SerializeField] private LayerMask groundLayer;

    // 地面に着地しているか
    private bool isGrounded = false;

    // プレイヤーのRigidbody2D
    private Rigidbody2D rb;

    // 元々の大きさ
    private Vector2 defaultScale;

    // 復帰する高さ
    public float respawnY = 7f;


    // 車に当たったときの吹っ飛び

    // 吹っ飛ぶ強さ
    [SerializeField] private float knockbackPower = 15.0f;

    // 吹っ飛んでいる時間
    [SerializeField] private float knockbackTime = 0.5f;

    // 吹っ飛んでいる最中か
    private bool isKnockback = false;

    // 吹っ飛びタイマー
    private float knockbackTimer = 0f;


    void Start()
    {
        // Rigidbody2Dを取得
        rb = GetComponent<Rigidbody2D>();

        // プレイヤーの元の大きさを保存
        defaultScale = transform.localScale;
    }


    void Update()
    {
        // 吹っ飛んでいる最中
        if (isKnockback)
        {
            knockbackTimer -= Time.deltaTime;

            // 吹っ飛び時間が終了した
            if (knockbackTimer <= 0f)
            {
                isKnockback = false;
            }
        }

        // 吹っ飛んでいないときだけ左右移動
        if (!isKnockback)
        {
            Walk();
        }

        // ジャンプ
        Jump();

        // 落下したか確認
        CheckRespawn();
    }


    private void Walk()
    {
        // 右なら1、左なら-1、何も押していなければ0
        float direction = Input.GetAxisRaw("Horizontal");

        // 左右に移動
        rb.linearVelocityX = direction * moveSpeed;

        // 右向き
        if (direction > 0)
        {
            transform.localScale = defaultScale;
        }
        // 左向き
        else if (direction < 0)
        {
            transform.localScale = new Vector2(
                -defaultScale.x,
                defaultScale.y
            );
        }
    }


    private void Jump()
    {
        // 地面を検知
        if (Physics2D.OverlapCircle(
            groundChecker.position,
            checkerRadius,
            groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // WキーまたはSpaceキーでジャンプ
        if ((Input.GetKeyDown(KeyCode.W) ||
             Input.GetKeyDown(KeyCode.Space))
             && isGrounded
             && !isKnockback)
        {
            rb.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse
            );
        }
    }


    // 車にぶつかったとき
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 車以外なら何もしない
        if (!collision.gameObject.CompareTag("Car"))
        {
            return;
        }

        // 吹っ飛び開始
        isKnockback = true;

        // 吹っ飛び時間をセット
        knockbackTimer = knockbackTime;

        // 車がプレイヤーの右側にある場合
        if (collision.transform.position.x > transform.position.x)
        {
            // 左に吹っ飛ばす
            rb.linearVelocity = new Vector2(
                -knockbackPower,
                3f
            );
        }
        else
        {
            // 右に吹っ飛ばす
            rb.linearVelocity = new Vector2(
                knockbackPower,
                3f
            );
        }
    }


    // 落下したときの復帰処理
    private void CheckRespawn()
    {
        // Y座標が-10より下に落ちたら
        if (transform.position.y < -10f)
        {
            // 3だけ左に戻して、Y=7の高さに復帰
            transform.position = new Vector3(
                transform.position.x - 3f,
                respawnY,
                transform.position.z
            );

            // 落下中の速度をリセット
            rb.linearVelocity = Vector2.zero;

            // 吹っ飛び状態を解除
            isKnockback = false;
            knockbackTimer = 0f;
        }
    }
}

//バナナの謎はまだ謎なのだぞ～