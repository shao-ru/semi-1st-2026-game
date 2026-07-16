using UnityEngine;
using System.Collections; //コルーチンを使うときなどに必要

public class playerAttack : MonoBehaviour

{
    //攻撃オブジェクト
    [SerializeField] private Collider2D attackCollider;

    //攻撃時間
    [SerializeField] private float attackTime = 0.5f;

    //攻撃中はtrue
    private bool isAttacking = false;

    //プレイヤーのAnimatorを入れる箱
    private Animator anim;


    void Start()
    {
        //最初は攻撃用のオブジェクトの当たり判定をOFFにしておく
        attackCollider.enabled = false;

        //プレイヤーのAnimatorを取得してanimに入れる
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //左クリックをおす かつ 攻撃中じゃなければ
        if (Input.GetMouseButtonDown(0) && isAttacking == false)
        {
            //コルーチンを使用するメソッドを呼び出すときはStartCoroutineが必要
            StartCoroutine(Attack());
        }
    }


    /// <summary>
    /// 攻撃するメソッド
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack()
    {
        //攻撃中であるためtrueにする
        isAttacking = true;

        //攻撃用のオブジェクトの当たり判定をONにする
        attackCollider.enabled = true;

        //プレイヤーの攻撃アニメーションON
        anim.SetBool("Attack", true);

        //指定時間待機
        yield return new WaitForSeconds(attackTime);

        //プレイヤーの攻撃アニメーションOFF
        anim.SetBool("Attack", false);

        //攻撃用のオブジェクトの当たり判定をOFFにする
        attackCollider.enabled = false;

        //攻撃が終わったためfalseにする
        isAttacking = false;
        

    }
}