using UnityEngine;

public class EnemyCliffChecker : MonoBehaviour
{
   //敵キャラ
    [SerializeField] private Transform enemy;

    /// <summary>
    /// Exitは離れた瞬間を検知する
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //enemyがnullだったらこのメソッドを中断する
        if (enemy == null) return;

        //地面を検知できなくなったら
        if (collision.CompareTag("Ground"))
        {
            //今とは逆向きにする
            enemy.localScale = new Vector2(-enemy.localScale.x, enemy.localScale.y);
        }
    }

}
