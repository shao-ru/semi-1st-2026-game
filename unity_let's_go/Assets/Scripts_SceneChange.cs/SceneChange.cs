using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理に必要な名前空間

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // 遷移先のシーン名

    // Trigger設定されたコライダーに何かが侵入した時に呼ばれる
    private void OnTriggerEnter2D(Collider2D other) // 3Dの場合は OnTriggerEnter(Collider other)
    {
        // 侵入してきたオブジェクトが"Player"タグを持っているか確認
        if (other.CompareTag("Player"))
        {
            // 設定したシーンへ切り替え
            SceneManager.LoadScene(nextSceneName);
        }
    }
}