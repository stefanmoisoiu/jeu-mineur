using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;
    
    private Coroutine _currentEffect;
    

    [Header("Effects")]
    
    [SerializeField] private ScriptableButtonEffect normal;
    [SerializeField] private ScriptableButtonEffect hovered;
    [SerializeField] private ScriptableButtonEffect pressed;
    
    private void PlayButtonEffect(ScriptableButtonEffect effect)
    {
        if (_currentEffect != null) StopCoroutine(_currentEffect);
        _currentEffect = StartCoroutine(effect.ApplyEffect(button, text));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayButtonEffect(hovered);
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
        PlayButtonEffect(hovered);
    }
}
