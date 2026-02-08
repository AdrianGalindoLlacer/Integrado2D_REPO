using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    [SerializeField] bool isFacingRight = false;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.8f;
    Vector2 aimDirection;


    [Header("Weapon")]
    public Weapon weapon;

    Rigidbody2D playerRb;
    Vector2 moveDirection;
    Vector2 mousePosition;

    bool isDashing;
    float dashTime;
    float dashCooldownTimer;
    Vector2 dashDirection;

    Vector2 moveInput;
    private Animator anim;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
        

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z; // Distancia al plano Z=0
        mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        aimDirection = (mousePosition - playerRb.position).normalized;

        if (moveInput.x > 0 && !isFacingRight) Flip();
        if (moveInput.x < 0 && isFacingRight) Flip();






        if (Input.GetMouseButton(0) && weapon != null)
        {
            weapon.Fire();
        }

      
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;
            dashDirection = moveDirection == Vector2.zero ? aimDirection : moveDirection;

        }

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
       
        if (isDashing)
        {
            playerRb.linearVelocity = dashDirection * dashSpeed;
            dashTime -= Time.fixedDeltaTime;

            if (dashTime <= 0)
            {
                isDashing = false;
                playerRb.linearVelocity = Vector2.zero;
            }
            return;
        }

       

        Movement();
    }

    void Movement()
    {
        playerRb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 actualScale = transform.localScale;
        actualScale.x *= -1;
        transform.localScale = actualScale;
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {

    }

    public void OnDash(InputAction.CallbackContext context)
    {

    }

    #endregion
}
