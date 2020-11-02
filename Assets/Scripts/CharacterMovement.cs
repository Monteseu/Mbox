using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Animator animator;
    SkinnedMeshRenderer charSkin;

    [SerializeField]
    float topSpeed = 12f;
    [SerializeField]
    float accelerationDamp = 0.75f;
    [SerializeField]
    float decelerationDamp = 0.1f;

    float currentSpeed = 0f;
    float currentVelocity;
    bool pressingDown;

    CharacterState currentState = CharacterState.FREE_FORWARD;

    PathController currentPath;

    float currentPathPosition = 0;
    float blendProgress = 0;
    [SerializeField]
    float blendSpeed = 1;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        charSkin = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void SetPositionAlongPath(float newDistancePosition)
    {
        // Update the currentPosition after adding the step.
        currentPathPosition = newDistancePosition;

        // If we reach the end of the path we switch to Free mode.
        if (newDistancePosition >= currentPath.characterPath.PathLength)
            currentState = CharacterState.FREE_FORWARD;

        // Get the world space position from the distance on the path.
        Vector3 desiredPosition = currentPath.characterPath.EvaluatePositionAtUnit(newDistancePosition, CinemachinePathBase.PositionUnits.Distance);

        // Ignore Y Constraint, let gravity do the job.
        desiredPosition.y = transform.position.y;

        // We might enter to a new path from a random position and we don't want to teleport to the init of the path so we lerp our position until we're 1-1 with the path position.
        if (currentState.Equals(CharacterState.BLENDING))
        {
            blendProgress = Mathf.Clamp01(blendProgress + blendSpeed * Time.deltaTime);

            // Get direction to use it for a LookRotation so we'll look forwards.
            Vector3 direction = (desiredPosition - transform.position).normalized;

            // Avoid the case where direction == 0.
            if (direction.magnitude > 0)
                transform.rotation = Quaternion.LookRotation(direction);

            // Move the character
            transform.position = Vector3.Lerp(transform.position, desiredPosition, blendProgress);

            // When blend is complete we can smoothly switch to the rail contrained movement.
            if (blendProgress == 1)
            {
                currentState = CharacterState.FOLLOW_RAIL;
                blendProgress = 0;
            }
        }
        else
        {
            // Just move the character to the path position after adding the step.
            transform.position = desiredPosition;
            transform.rotation = currentPath.characterPath.EvaluateOrientationAtUnit(newDistancePosition, CinemachinePathBase.PositionUnits.Distance);
        }
    }

    void Update()
    {
        /* I prefer to use a bool instead of stabbing the logic inside the "GetMouse...". Mainly because in physic-depending games like this we move the character
         * in the FixedUpdate, while we process the input in the Update. Here we are directly Translating the transform but also depending of the gravity, which is not cool
         * and will probably make jittery stuff when falling or being pushed/moved by platforms. In version(1) there was no falling/Interactions so no inmediate need for Physics.
         * */

        // The reason why I don't just use pressingDown = Input.GetMouseButton(0); is because I usually give more functionality inside those two events (Sounds, Particles...)
        // But in this specific case we could be using the method above.

        if (Input.GetMouseButtonDown(0))
            pressingDown = true;
        if (Input.GetMouseButtonUp(0))
            pressingDown = Input.GetMouseButton(0);

        if (pressingDown)
            currentSpeed = Mathf.SmoothDamp(currentSpeed, topSpeed, ref currentVelocity, accelerationDamp);
        else
            currentSpeed = Mathf.SmoothDamp(currentSpeed, 0f, ref currentVelocity, decelerationDamp);

        // If we're on a path or blending
        if (!currentState.Equals(CharacterState.FREE_FORWARD))
            SetPositionAlongPath(currentPathPosition + currentSpeed * Time.deltaTime);
        else
            transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // Normalize speed (0-1) from 0 to our TopSpeed to let the animator know which animation to apply.
        float normalizedSpeed = Mathf.InverseLerp(0, topSpeed, Mathf.Abs(currentSpeed)) * Mathf.Sign(currentSpeed);

        animator.SetFloat("Speed", normalizedSpeed);
    }

    // Called by the path trigger when we enter a new path
    public void OnEnterNewPath(PathController path)
    {
        blendProgress = 0;
        currentPath = path;
        currentPathPosition = 0f;
        currentState = CharacterState.BLENDING;
    }

    // Called when something kills us. We just start over from the beggining of our last path.
    public void SpawnFromLastPath()
    {
        currentPathPosition = 0f;
        SetPositionAlongPath(currentPathPosition);
        charSkin.material.DOColor(Color.red, 0.1f).SetLoops(4, LoopType.Yoyo);
    }
}

enum CharacterState
{
    FOLLOW_RAIL,
    FREE_FORWARD,
    BLENDING
}