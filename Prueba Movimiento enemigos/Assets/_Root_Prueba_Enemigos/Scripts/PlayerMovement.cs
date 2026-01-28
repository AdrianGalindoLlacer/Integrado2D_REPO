using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.8f;

    [Header("Weapon")]
    public Weapon weapon;

    Rigidbody2D rb;
    Vector2 moveDirection;
    Vector2 mousePosition;

    bool isDashing;
    float dashTime;
    float dashCooldownTimer;
    Vector2 dashDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        moveDirection = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z; // Distancia al plano Z=0
        mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        
        
        
        Vector2 aimDir = mousePosition - rb.position;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;



        if (Input.GetMouseButton(0) && weapon != null)
        {
            weapon.Fire();
        }

      
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;
            dashDirection = moveDirection == Vector2.zero ? aimDir.normalized : moveDirection;
        }

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
       
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTime -= Time.fixedDeltaTime;

            if (dashTime <= 0)
            {
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        
        rb.linearVelocity = moveDirection * moveSpeed;
    }
}
