using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //移動スピード(floatなので整数、整数を扱える)
    [SerializeField] private float moveSpeed = 10.0f;

    //ジャンプ力
    [SerializeField] private float jumpForce = 15.0f;

    // プレイヤーの足元にある子オブジェクト
    [SerializeField] private Transform groundChecker;

    // 地面をチェックする円の半径
    [SerializeField] private float checkerRadius = 0.1f;

    // 地面のレイヤー
    [SerializeField] private LayerMask groundLayer;

    // 地面に着地しているときはtrue、離れているときはfalse
   private bool isGrounded = false;

    //プレイヤーのRigidBody2Dを入れる箱
    private Rigidbody2D rb;

    //元々の大きさ
    private Vector2 defaulttScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //プレイヤーのRigidBody2Dを取得してrbに入れる
        rb = GetComponent<Rigidbody2D>();

        //プレイヤーの元の大きさを代入
        defaulttScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //自分で作ったWalkメソッドを呼び出す
        Walk();

        Jump();
    }

    private void Walk()
    {
        //入力された方向を調べる(右なら1/左なら-1/何も押してなければ0)
        float direction = Input.GetAxisRaw("Horizontal");

        //RigidBody2D(物理演算)の「速度(velocity)」を使って左右に動かす
        rb.linearVelocityX = direction * moveSpeed;

        // 右に進んでいたら
        if (direction > 0)
        {
           //デフォルトのまま
           transform.localScale = defaulttScale;
        }
        // 左に進んでいたら
        else if (direction < 0)
        {
          //
          transform.localScale = new Vector2(-defaulttScale.x, defaulttScale.y);
        }
    }

    /// <summary>
    /// ジャンプメソッド
    /// WキーまたはSpaceキーを押すとジャンプする
    /// </summary>   
    private void Jump()
   {    
        // OverlapCircle(円の中心, 円の半径, 検知するレイヤー);

        // 足元のチェッカーが地面を検知したら
        if (Physics2D.OverlapCircle(groundChecker.position, checkerRadius, groundLayer))
        {
            isGrounded = true;
        }
        // 検知してないときは
        else
        {
            isGrounded = false;
        }

       // (WキーまたはSpaceキーを押した瞬間)かつisGroundがtrueならば
       if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded == true)
       {
        // 上方向に一気に力を加える
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
       }
   }


}