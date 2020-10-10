using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteorTrap : TrapBehaviour
{
    [SerializeField]
    Transform alertObjectTransform;
    [SerializeField]
    SpriteRenderer crackSprite;
    [SerializeField]
    GameObject meteorPrefab;

    [SerializeField]
    float indicatorScaleTime = 1f;
    [SerializeField]
    float indicatorRotationSpeed = 1f;
    [SerializeField]
    float meteorDelayAfterIndicator = 0.5f;
    [SerializeField]
    float meteorSpawnHeight = 5f;
    [SerializeField]
    float meteorFlyTime = 1f;
    float currentTime = 0f;


    private void Start()
    {
        alertObjectTransform.gameObject.SetActive(false);
        currentTime = startTime;
    }
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= activationInterval)
        {
            currentTime = 0;
            StartCoroutine(ActivateTrapSequence());
        }
    }

    IEnumerator ActivateTrapSequence()
    {
        yield return null;
        alertObjectTransform.gameObject.SetActive(true);
        alertObjectTransform.DORotate(Vector3.up * 360, indicatorRotationSpeed).SetSpeedBased().SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        alertObjectTransform.DOScale(0f, indicatorScaleTime).From();

        yield return new WaitForSeconds(indicatorScaleTime + meteorDelayAfterIndicator);

        Transform meteor = Instantiate(meteorPrefab, transform.position + Vector3.up * meteorSpawnHeight, meteorPrefab.transform.rotation, transform).transform;

        meteor.DOMoveY(transform.position.y, meteorFlyTime).SetEase(Ease.InCubic).OnComplete(() =>
        {
            alertObjectTransform.gameObject.SetActive(false);
            GameManager.get.fxManager.SpawnExplosion(transform.position);

            crackSprite.DOFade(0.75f, 0.15f).OnComplete(() =>
            {
                crackSprite.DOFade(0f, 0.5f);
            });
            Destroy(meteor.gameObject);
        });

    }

}
