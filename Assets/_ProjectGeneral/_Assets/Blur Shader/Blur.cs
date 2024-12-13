using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[AddComponentMenu("UI/Blur Panel")]
[RequireComponent(typeof(CanvasGroup))]
public class BlurPanel : Image
{

    CanvasGroup canvasGroup;
    [Range(0,2)]
    public float blurValue = 1f;
    private float oldBlurValue = 100f;

    private Tween tween;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        color = Color.black * 0.1f;
    }
#endif

    protected override void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected override void OnEnable()
    {
        Blur(blurValue);
    }

    void Update()
    {
        Blur(blurValue);
    }

    private void Blur(float value)
    {        
        if (oldBlurValue != value)
        {
            oldBlurValue = value;
            base.UpdateMaterial();
        }
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();        
        canvasGroup.alpha = value;        
    }

    public void DOBlur(float value, float duration)
    {
        if (tween != null)
            tween.Kill();   
        tween = DOTween.To(() => blurValue, x => blurValue = x, value, duration);
    }
}