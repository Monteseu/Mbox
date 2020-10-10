using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [HideInInspector]
    public CinemachinePath characterPath;

    private void Awake()
    {
        characterPath = GetComponent<CinemachinePath>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterMovement character = other.GetComponent<CharacterMovement>();
        if (character != null)
            character.OnEnterNewPath(this);
    }
}
