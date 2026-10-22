using UnityEngine;

public class SignalController : MonoBehaviour
{
    [Header("信号の時間")]
    [SerializeField] private float greenTime = 5f;
    [SerializeField] private float redTime = 5f;

    // 青信号ならtrue
    // 赤信号ならfalse
    private bool isGreen = true;

    // 現在の状態になってから経過した時間
    private float timer = 0f;

    // SignalについているSpriteRenderer
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // SpriteRendererを取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 最初は青
        SetGreen();
    }

    private void Update()
    {
        // 経過時間を加える
        timer += Time.deltaTime;

        // 現在が青信号の場合
        if (isGreen)
        {
            // 青信号の時間が終わったら赤にする
            if (timer >= greenTime)
            {
                SetRed();
            }
        }
        // 現在が赤信号の場合
        else
        {
            // 赤信号の時間が終わったら青にする
            if (timer >= redTime)
            {
                SetGreen();
            }
        }
    }

    private void SetGreen()
    {
        // 青信号にする
        isGreen = true;

        // タイマーを0に戻す
        timer = 0f;

        // 見た目を青にする
        spriteRenderer.color = Color.blue;
    }

    private void SetRed()
    {
        // 赤信号にする
        isGreen = false;

        // タイマーを0に戻す
        timer = 0f;

        // 見た目を赤にする
        spriteRenderer.color = Color.red;
    }

    // 他のスクリプトから
    // 「今、青なの？」を確認するための関数
    public bool IsGreen()
    {
        return isGreen;
    }
}