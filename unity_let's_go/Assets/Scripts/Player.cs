
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    [SerializeField] private float jumpForce = 15.0f;

    // スピードアップ設定
    public float speedUp = 15f;
    public float speedUpTime = 5f;

    private float normalSpeed;
    private Coroutine speedUpCoroutine;

    [SerializeField] private Transform groundChecker;
    [SerializeField] private float checkerRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;
    private Rigidbody2D rb;
    private Vector2 defaultScale;
    private float moveX;

    public float respawnY = 7f;

    [SerializeField] private float knockbackPower = 15.0f;
    [SerializeField] private float knockbackTime = 0.5f;

    private bool isKnockback;
    private float knockbackTimer;
    private bool isFallen = false;

    private float defaultGravityScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultScale = transform.localScale;
        defaultGravityScale = rb.gravityScale;

        normalSpeed = moveSpeed;
    }

    void Update()
    {
        if (isFallen)
        {
            return;
        }

        if (transform.position.y < -10f)
        {
            CheckRespawn();
            return;
        }

        moveX = Input.GetAxisRaw("Horizontal");

        if (isKnockback)
        {
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0f)
            {
                isKnockback = false;
            }
        }

        if (!isKnockback)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (isFallen)
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );
            return;
        }

        if (isKnockback)
        {
            return;
        }

        rb.linearVelocity = new Vector2(
            moveX * moveSpeed,
            rb.linearVelocity.y
        );

        if (moveX > 0)
        {
            transform.localScale = defaultScale;
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector2(
                -defaultScale.x,
                defaultScale.y
            );
        }
    }

    private void Jump()
    {
        if (groundChecker == null)
        {
            return;
        }

        isGrounded = Physics2D.OverlapCircle(
            groundChecker.position,
            checkerRadius,
            groundLayer
        );

        if ((Input.GetKeyDown(KeyCode.W) ||
             Input.GetKeyDown(KeyCode.Space))
             && isGrounded)
        {
            rb.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse
            );
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFallen)
        {
            return;
        }

        if (!collision.gameObject.CompareTag("Car"))
        {
            return;
        }

        isKnockback = true;
        knockbackTimer = knockbackTime;

        if (collision.transform.position.x > transform.position.x)
        {
            rb.linearVelocity = new Vector2(
                -knockbackPower, 3f
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(
                knockbackPower, 3f
            );
        }
    }

    // スピードアップ
    public void SpeedUp()
    {
        if (speedUpCoroutine != null)
        {
            StopCoroutine(speedUpCoroutine);
        }

        speedUpCoroutine = StartCoroutine(SpeedUpCoroutine());
    }

    IEnumerator SpeedUpCoroutine()
    {
        moveSpeed = speedUp;

        yield return new WaitForSeconds(speedUpTime);

        moveSpeed = normalSpeed;
        speedUpCoroutine = null;
    }

    private void CheckRespawn()
    {
        isFallen = true;
        moveX = 0f;
        isKnockback = false;
        knockbackTimer = 0f;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;

        Vector2 respawnPosition = new Vector2(
            rb.position.x - 5f,
            respawnY
        );

        rb.position = respawnPosition;

        transform.position = new Vector3(
            respawnPosition.x,
            respawnPosition.y,
            transform.position.z
        );

        Physics2D.SyncTransforms();

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = defaultGravityScale;

        CancelInvoke(nameof(EnableControl));
        Invoke(nameof(EnableControl), 3f);
    }

    private void EnableControl()
    {
        isFallen = false;
    }
}
