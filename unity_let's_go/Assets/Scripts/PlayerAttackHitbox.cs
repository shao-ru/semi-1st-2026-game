using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
     /// <summary>
    /// 接触した瞬間を検知する
    /// IsTriggerにチェックを入れる必要がある
    /// </summary>
    /// <param name="collision"></param>
private void OnTriggerEnter2D(Collider2D collision)
 {
    //接触したオブジェクトのタグがEnemyならば
    if (collision.CompareTag("Enemy"))
    {
        //そのオブジェクトを消滅させる
        Destroy(collision.gameObject);
    }
 }
}
