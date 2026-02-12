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

    [Header("Weapon")]
    public Weapon weapon;

    Rigidbody2D playerRb;
    Vector2 moveDirection;
    Vector2 mousePosition;

    bool isDashing;
    Vector2 moveInput;
    private Animator anim;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDashing)
            return;

        // Flip
        if (moveInput.x > 0 && !isFacingRight) Flip();
        if (moveInput.x < 0 && isFacingRight) Flip();

        // Disparo
        if (Input.GetMouseButton(0) && weapon != null)
        {
            weapon.Fire();
        }

        
        bool isMoving = moveInput.magnitude > 0.1f;
        anim.SetBool("Run", isMoving);
    }

    void FixedUpdate()
    {
        if (isDashing)
            return;

        playerRb.linearVelocity = moveInput * moveSpeed;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        
        anim.SetBool("Dash", true);

        AudioManager.Instance.PlaySFX(4);
        playerRb.linearVelocity = moveDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        
        anim.SetBool("Dash", false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    #region Input

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
