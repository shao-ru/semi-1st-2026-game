using UnityEngine;

/// <summary>
/// リトライボタン用。
/// ボタンなどに付けて、Button の OnClick に RetryButton.Retry を登録する。
/// 押すとタイマーを最初に戻して、今のシーンをやり直す。
/// </summary>
public class RetryButton : MonoBehaviour
{
    public void Retry()
    {
        GlobalTimer.RetryCurrentScene();
    }
}
