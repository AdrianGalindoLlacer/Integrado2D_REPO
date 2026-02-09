using UnityEngine;
using System.Collections;
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
    bool canDash = true;
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

    private void Start()
    {
        canDash = true;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        aimDirection = (mousePosition - playerRb.position).normalized;

        if (moveInput.x > 0 && !isFacingRight) Flip();
        if (moveInput.x < 0 && isFacingRight) Flip();

        if (Input.GetMouseButton(0) && weapon != null)
        {
            weapon.Fire();
        }

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Dash hacia la dirección en la que caminas
        playerRb.linearVelocity = new Vector2(
            moveDirection.x * dashSpeed,
            moveDirection.y * dashSpeed
        );

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        
        moveDirection = moveInput.normalized;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    #endregion
}
