using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ButtonEffect", menuName = "UI/ButtonEffect", order = 1)]
public class ScriptableButtonEffect : ScriptableObject
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
            t += Time.unscaledDeltaTime;
            float advancement = t / length;
            float curveAdvancement = advancementCurve.Evaluate(advancement);
                
                
            button.transform.localScale = Vector3.Lerp(originalScale, targetScale, curveAdvancement);
            text.characterSpacing = Mathf.Lerp(originalCharacterSpacing, targetCharacterSpacing, curveAdvancement);
                
            yield return null;
        }
    }
}