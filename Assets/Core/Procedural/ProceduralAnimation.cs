using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ProceduralAnimation
{
    [Header("References")]
    [SerializeField] private Transform objectToAnimate;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Main Properties")] [SerializeField]
    private float animationLength = 1f;

    [SerializeField] private bool loop;
    [ShowIf("@loop")][SerializeField] private LoopType loopType;
    private enum LoopType
    {
        Loop,
        PingPong
    }

    [HorizontalGroup] [SerializeField] private bool scale, rotation, opacity, spriteColor, position;
    
    [FoldoutGroup("Scaling")][ShowIf("@scale")][SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [FoldoutGroup("Scaling")][ShowIf("@scale")][SerializeField] private Vector3 startValueScale = Vector3.one, endValueScale = Vector3.one;
    
    [FoldoutGroup("Rotation")][ShowIf("@rotation")][SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [FoldoutGroup("Rotation")][ShowIf("@rotation")][SerializeField] private float startValueRotation = 0, endValueRotation = 0;
    
    [FoldoutGroup("Opacity")][ShowIf("@opacity")][SerializeField] private AnimationCurve opacityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [FoldoutGroup("Opacity")][ShowIf("@opacity")][SerializeField] private float startValueOpacity = 1, endValueOpacity = 1;
    
    [FoldoutGroup("Sprite Color")][ShowIf("@spriteColor")][SerializeField] private AnimationCurve spriteColorCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [FoldoutGroup("Sprite Color")][ShowIf("@spriteColor")][SerializeField] private Color startValueSpriteColor = Color.white, endValueSpriteColor = Color.white;
    
    [FoldoutGroup("Position")][ShowIf("@position")][SerializeField] private AnimationCurve positionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [FoldoutGroup("Position")][ShowIf("@position")][SerializeField] private Vector3 startPosition = Vector3.zero, endPosition = Vector3.zero;
    
    public Action OnAnimationEnd;

    private Coroutine _animationCoroutine;

    public ProceduralAnimation StartAnimation(MonoBehaviour monoBehaviour)
    {
        SetAnimation(0);
        if (_animationCoroutine != null) monoBehaviour.StopCoroutine(_animationCoroutine);
        _animationCoroutine = monoBehaviour.StartCoroutine(AnimateCoroutine());
        return this;
    }

    public ProceduralAnimation StopAnimation(MonoBehaviour monoBehaviour)
    {
        SetAnimation(1);
        if (_animationCoroutine != null) monoBehaviour.StopCoroutine(_animationCoroutine);
        return this;
    }

    public void Reset()
    {
        OnAnimationEnd = null;
    }

    public void SetAnimation(float advancement)
    {
        if (scale) objectToAnimate.localScale = Vector3.Lerp(startValueScale, endValueScale, scaleCurve.Evaluate(advancement));
        
        if (rotation) objectToAnimate.localRotation = Quaternion.Euler(0,0,Mathf.Lerp(startValueRotation, endValueRotation, rotationCurve.Evaluate(advancement)));
        
        if (opacity) spriteRenderer.color = new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b, 
            Mathf.Lerp(startValueOpacity, endValueOpacity, opacityCurve.Evaluate(advancement)));
        
        if (spriteColor) spriteRenderer.color = new Color(
            Mathf.Lerp(startValueSpriteColor.r, endValueSpriteColor.r, spriteColorCurve.Evaluate(advancement)), 
            Mathf.Lerp(startValueSpriteColor.g, endValueSpriteColor.g, spriteColorCurve.Evaluate(advancement)),
            Mathf.Lerp(startValueSpriteColor.b, endValueSpriteColor.b, spriteColorCurve.Evaluate(advancement)), 
            spriteRenderer.color.a);
        
        if (position) objectToAnimate.localPosition = Vector3.Lerp(startPosition, endPosition, positionCurve.Evaluate(advancement));
    }

    private IEnumerator AnimateCoroutine()
    {
        int direction = 1;
        
        float advancement = 0;
        while (true)
        {
            advancement += Time.deltaTime * direction / animationLength;
            if (advancement > 1 || advancement < 0)
            {
                if (!loop)
                {
                    SetAnimation(1);
                    break;
                }

                switch (loopType)
                {
                    case LoopType.Loop:
                        advancement = 0;
                        break;
                    case LoopType.PingPong:
                        direction *= -1;
                        advancement = Mathf.Clamp01(advancement);
                        break;
                }
            }
            SetAnimation(advancement);
            yield return null;
        }
        OnAnimationEnd?.Invoke();
    }
    public void Then(Action action)
    {
        OnAnimationEnd += action;
    }
}