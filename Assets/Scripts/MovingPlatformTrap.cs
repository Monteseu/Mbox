using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTrap : TrapBehaviour
{
    [SerializeField]
    float range = 6f;
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    private void Start()
    {
        transform.position += transform.right * range / 2f;
        transform.DOMoveX(-range, moveSpeed).SetSpeedBased().SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(startTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterMovement character = other.GetComponent<CharacterMovement>();
        if (character != null)
            character.SpawnFromLastPath();
    }
}
