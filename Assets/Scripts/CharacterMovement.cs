using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    float topSpeed = 12f;
    [SerializeField]
    float accelerationDamp = 0.75f;
    [SerializeField]
    float decelerationDamp = 0.1f;
    float currentSpeed = 0f;
    float currentVelocity;
    bool pressingDown;

    SkinnedMeshRenderer charSkin;

    PathController currentPath;

    float currentPathPosition = 0;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        charSkin = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    void SetCartPosition(float distanceAlongPath)
    {
        currentPathPosition = currentPath.characterPath.StandardizeUnit(distanceAlongPath, CinemachinePathBase.PositionUnits.Distance);
        transform.position = currentPath.characterPath.EvaluatePositionAtUnit(currentPathPosition, CinemachinePathBase.PositionUnits.Distance);
        transform.rotation = currentPath.characterPath.EvaluateOrientationAtUnit(currentPathPosition, CinemachinePathBase.PositionUnits.Distance);
    }

    public void OnEnterNewPath(PathController path)
    {
        currentPath = path;
        currentPathPosition = 0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            pressingDown = true;
        if (Input.GetMouseButtonUp(0))
            pressingDown = false;

        if (pressingDown)
            currentSpeed = Mathf.SmoothDamp(currentSpeed, topSpeed, ref currentVelocity, accelerationDamp);
        else
            currentSpeed = Mathf.SmoothDamp(currentSpeed, 0f, ref currentVelocity, decelerationDamp);

        if (currentSpeed > 0)
            SetCartPosition(currentPathPosition + currentSpeed * Time.deltaTime);

        float normalizedSpeed = Mathf.InverseLerp(0, topSpeed, Mathf.Abs(currentSpeed)) * Mathf.Sign(currentSpeed);

        animator.SetFloat("Speed", normalizedSpeed);
    }

    public void SpawnFromLastPath()
    {
        currentPathPosition = 0f;
        SetCartPosition(currentPathPosition);
        charSkin.material.DOColor(Color.red, 0.1f).SetLoops(4, LoopType.Yoyo);
    }


}
