using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;
    
    private Coroutine _currentEffect;
    

    [Header("Effects")]
    
    [SerializeField] private ButtonEffect normal;
    [SerializeField] private ButtonEffect highlighted;
    [SerializeField] private ButtonEffect pressed;
    
    private void PlayButtonEffect(ButtonEffect effect)
    {
        if (_currentEffect != null) StopCoroutine(_currentEffect);
        _currentEffect = StartCoroutine(effect.ApplyEffect(button, text));
    }

    [System.Serializable]
    public class ButtonEffect
    {
        public float length = 0.1f;
        public AnimationCurve advancementCurve = AnimationCurve.EaseInOut(0,0,1,1);
        public float scale = 1;
        public float characterSpacing;
        
        public IEnumerator ApplyEffect(Button button, TMP_Text text)
        {
            Vector3 originalScale = button.transform.localScale;
            Vector3 targetScale = Vector3.one * scale;
            
            float originalCharacterSpacing = text.characterSpacing;
            float targetCharacterSpacing = characterSpacing;
            
            float t = 0f;
            while (t < length)
            {
                t += Time.deltaTime;
                float advancement = t / length;
                float curveAdvancement = advancementCurve.Evaluate(advancement);
                
                
                button.transform.localScale = Vector3.Lerp(originalScale, targetScale, curveAdvancement);
                text.characterSpacing = Mathf.Lerp(originalCharacterSpacing, targetCharacterSpacing, curveAdvancement);
                
                yield return null;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayButtonEffect(highlighted);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayButtonEffect(normal);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayButtonEffect(pressed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayButtonEffect(highlighted);
    }
}
