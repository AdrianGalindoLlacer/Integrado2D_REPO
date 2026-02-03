using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Rigidbody2D target;
    [SerializeField] Vector2 leadingFactor = new Vector2(0.25f, 0.15f);
    [SerializeField] float lerpSpeed = 5f;
    [SerializeField] float maxDistance = 3f;
    [SerializeField] float maxTargetSpeed = 5f;
    float sqrMaxDistance;
    float sqrMaxTargetSpeed;
    void Awake()
    {
        sqrMaxDistance = maxDistance * maxDistance;
        sqrMaxTargetSpeed = maxTargetSpeed * maxTargetSpeed;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = new Vector3
        (
            target.position.x + target.linearVelocity.x * leadingFactor.x,
            target.position.y + target.linearVelocity.y * leadingFactor.y,
            transform.position.z
        );
        if (target.linearVelocity.sqrMagnitude > sqrMaxTargetSpeed)
        {
            Vector2 limitedVelocity = target.linearVelocity.normalized * maxTargetSpeed;
            targetPosition = new Vector3
            (
                target.position.x + limitedVelocity.x * leadingFactor.x,
                target.position.y + limitedVelocity.y * leadingFactor.y,
                transform.position.z
            );
        }
        Vector3 desiredPosition = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        if ((targetPosition - desiredPosition).sqrMagnitude > sqrMaxDistance)
        {
            Vector3 direction = (targetPosition - desiredPosition).normalized;
            desiredPosition = targetPosition - direction * maxDistance;
        }
        transform.position = desiredPosition;
    }
}