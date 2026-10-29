using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("アニメーション画像")]
    [SerializeField] private Sprite idleSprite;      // 待機時の画像
    [SerializeField] private Sprite walkSpriteA;     // 移動中の画像1
    [SerializeField] private Sprite walkSpriteB;     // 移動中の画像2
    [SerializeField] private Sprite jumpSprite;      // ジャンプ・空中時の画像
    [SerializeField] private float blinkInterval = 0.15f; // 切り替え速度

    [Header("SE（効果音）設定")]
    [SerializeField] private AudioSource audioSource; // AudioSourceコンポーネント
    [SerializeField] private AudioClip jumpSE;        // ジャンプ音
    [SerializeField] private float jumpSeThreshold = 1.0f; // ★SEが鳴る最小の上昇速度

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float animTimer = 0f;
    private bool useSpriteA = true;
    private bool wasInAir = false; // 空中状態を記憶するフラグ

    void Start()
    {
        // 自分についているコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // 未割り当ての場合は自動取得
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (spriteRenderer == null || rb == null) return;

        // Y軸の移動速度が一定以上（上昇中・落下中）なら「空中」と判定（画像切り替え用）
        bool isInAir = Mathf.Abs(rb.linearVelocityY) > 0.1f;

        // 1. 空中にいる間
        if (isInAir)
        {
            // ★段差対策：上方向にしっかり跳び上がった瞬間（速度が閾値以上）のみSEを再生
            if (!wasInAir && rb.linearVelocityY > jumpSeThreshold)
            {
                if (audioSource != null && jumpSE != null)
                {
                    audioSource.PlayOneShot(jumpSE);
                }
                wasInAir = true; // SEが鳴ったら空中フラグを立てる
            }

            if (jumpSprite != null)
            {
                spriteRenderer.sprite = jumpSprite;
            }
            return;
        }

        // 地上に着地したら空中フラグをリセット
        wasInAir = false;

        // 2. 地上にいる間（歩き・待機）
        float direction = Input.GetAxisRaw("Horizontal");

        if (direction != 0)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= blinkInterval)
            {
                animTimer = 0f;
                useSpriteA = !useSpriteA;
                spriteRenderer.sprite = useSpriteA ? walkSpriteA : walkSpriteB;
            }
        }
        else
        {
            animTimer = 0f;
            useSpriteA = true;
            if (idleSprite != null)
            {
                spriteRenderer.sprite = idleSprite;
            }
        }
    }
}