using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTweener : MonoBehaviour, IObjectTweener
{
    [SerializeField] private float movementSpeed;

    public void MoveTo(Transform transform, Vector3 targetPosition)
    {
        float distance = Vector3.Distance(targetPosition, transform.position);
        transform.DOMove(targetPosition, distance / movementSpeed).OnComplete(() => PlayAudio());
    }

    private void PlayAudio()
    {
        AudioManager.Instance.Play("ChessFinalPosition");
    }
}