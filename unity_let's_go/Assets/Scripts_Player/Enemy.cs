using UnityEngine;

public class Enemy : MonoBehaviour
{

    //移動スピード
    [SerializeField] private float moveSpeed = 5.0f;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    /// <summary>
    /// 現在のスケールを見て移動方向を変える
    /// 敵キャラのデフォルトの向きは左向き(プレイヤーとは逆)だから注意
    /// </summary>
    private void Walk()
    {
        //左右反転してないときは
        if (transform.localScale.x > 0)
        {
            //左方向(マイナス方向)
            rb.linearVelocityX = -moveSpeed;
        }
        //左右反転してるときは
        else if (transform.localScale.x < 0)
        {
            //右方向(プラス方向)
            rb.linearVelocityX = moveSpeed;
        }
    }
}

