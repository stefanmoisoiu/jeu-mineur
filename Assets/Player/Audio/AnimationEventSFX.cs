using UnityEngine;
public class AnimationEventSFX : MonoBehaviour
{
    [SerializeField] private ScriptableSFX[] sfxes;
    
    public void Play(int sfxIndex)
    {
        sfxes[sfxIndex].Play();
    }
}
