using DG.Tweening;
using UnityEngine;

public class MovingPlatformTrap : TrapBehaviour
{
    [SerializeField]
    float range = 6f;
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    Vector3 selfAxis;
    [SerializeField]
    bool killOnContact = true;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Transform from global to local direction so the same traps work with any orientation.
        Vector3 selfDirection = transform.InverseTransformDirection(selfAxis);
        // Move the piece half of the desired movement, then Tween the full movement.
        rb.position -= selfDirection * range / 2f;
        rb.DOMove(selfDirection.normalized * range, moveSpeed).SetSpeedBased().SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(startTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!killOnContact)
            return;

        CharacterMovement character = other.GetComponent<CharacterMovement>();
        if (character != null)
            character.SpawnFromLastPath();
    }
}
