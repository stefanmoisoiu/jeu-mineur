using UnityEngine;

public class PAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PDebug debug;
    private PDebug.DebugText _debugText;
    
    private string _currentAnimation;

    private void Start()
    {
        _debugText = () => $"Current animation: {_currentAnimation}";
        debug.AddDebugText(_debugText);
        
        _currentAnimation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    public void PlayAnimation(string animationName)
    {
        if (_currentAnimation == animationName) return;
        _currentAnimation = animationName;
        animator.Play(animationName);
    }
}
