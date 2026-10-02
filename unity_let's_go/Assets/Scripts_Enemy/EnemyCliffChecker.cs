using UnityEngine;

public class EnemyCliffChecker : MonoBehaviour
{
     // 敵キャラ
 [SerializeField] private Transform enemy;

 /// <summary>
 /// Enterは接触した瞬間を検知する
 /// </summary>
 /// <param name="collision"></param>
 private void OnTriggerExit2D(Collider2D collision)
 {
    // enemyがnullだったらこのメソッドを中断する
    if (enemy == null) return;

    // 壁を検知したら
    if (collision.CompareTag("Ground"))
    {
        // 今とは逆向きにする
        enemy.localScale = new Vector2(
            -enemy.localScale.x,
            enemy.localScale.y
        );
    }
 }
}
