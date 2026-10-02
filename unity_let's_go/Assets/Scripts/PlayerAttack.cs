using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private float attackTime = 0.5f;
    private bool isAttacking = false;

    void Start()
    {
        attackCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isAttacking == false)
        {
            StartCoroutine(Attack());
        }
    }

    /// <summary>
    /// 攻撃するメソッド
    /// </summary>
    private IEnumerator Attack()
    {
        isAttacking = true;
        attackCollider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        attackCollider.enabled = false;
        isAttacking = false;
    }
}