using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D rb;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;

    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f); // 如果我们不这样做，游戏开始，玩家不移动，抛射武器将没有动量
    }

    void Update()
    {
        InputManagement();

    }


    void FixedUpdate()
    {

        Move();
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0 || moveDir.y != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(moveDir.x, moveDir.y);
        }
    }

    void Move()
    {

        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }
}
