using UnityEngine;
public class Crow : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float patrolDistance = 10f;
    [Header("カラスの鳴き声")]
    [SerializeField] private AudioClip crowSound;
    [Header("音量")]
    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 1f;
    [Header("音が聞こえるX座標の範囲")]
    [SerializeField] private float soundRangeX = 5f;
    [Header("鳴き声の間隔")]
    [SerializeField] private float minSoundInterval = 3f;
    [SerializeField] private float maxSoundInterval = 7f;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Transform playerTransform;
    private float startX;
    private float nextSoundTime;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // AudioSourceを取得。なければ追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // 2Dサウンドに設定
        audioSource.spatialBlend = 0f;
        startX = transform.position.x;
        // Playerタグのオブジェクトを探す
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning(
                "Playerタグのオブジェクトが見つかりません！"
            );
        }
        // 最初の鳴き声までの時間を決める
        SetNextSoundTime();
    }
    private void FixedUpdate()
    {
        Walk();
        CheckTurn();
    }
    private void Update()
    {
        CheckCrowSound();
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
    private void CheckCrowSound()
    {
        // 音声ファイルやプレイヤーがなければ何もしない
        if (crowSound == null || playerTransform == null)
        {
            return;
        }
        // プレイヤーとのX座標の距離を計算
        float distanceX = Mathf.Abs(
            transform.position.x - playerTransform.position.x
        );
        // 範囲外なら鳴き声を停止
        if (distanceX > soundRangeX)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            // 範囲内に戻ったら鳴けるようにする
            SetNextSoundTime();
            return;
        }
        // 範囲内なら、設定した間隔で鳴く
        if (!audioSource.isPlaying &&
            Time.time >= nextSoundTime)
        {
            audioSource.PlayOneShot(crowSound, soundVolume);
            SetNextSoundTime();
        }
    }
    private void SetNextSoundTime()
    {
        nextSoundTime = Time.time + Random.Range(
            minSoundInterval,
            maxSoundInterval
        );
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = Application.isPlaying
            ? new Vector3(startX, transform.position.y, transform.position.z)
            : transform.position;
        Gizmos.DrawLine(
            center + Vector3.left * patrolDistance,
            center + Vector3.right * patrolDistance
        );
    }
}
///トリック　オア　トリート！　カボチャよこせや！
