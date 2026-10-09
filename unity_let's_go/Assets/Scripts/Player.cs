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


    void Start()
    {
        // Rigidbody2Dを取得
        rb = GetComponent<Rigidbody2D>();

        // プレイヤーの元の大きさを保存
        defaultScale = transform.localScale;
    }


    void Update()
    {
        // 左右移動
        Walk();

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
             && isGrounded)
        {
            rb.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse
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
        }
    }
}