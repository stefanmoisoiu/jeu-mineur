using UnityEngine;

public class PPipeProceduralAnim : PlayerProceduralAnim
{
    [SerializeField] private ProceduralAnimation enterPipeAnim, exitPipeAnim;
    [SerializeField] private PPipe pipe;
    [SerializeField] private PVisible visible;
    

    private void OnEnable()
    {
        pipe.OnEnterPipe += PlayEnterPipeAnim;
        pipe.OnExitPipe += PlayExitPipeAnim;
    }

    private void OnDisable()
    {
        pipe.OnEnterPipe -= PlayEnterPipeAnim;
        pipe.OnExitPipe -= PlayExitPipeAnim;
    }

    private void PlayEnterPipeAnim()
    {
        StartAnimation(() => enterPipeAnim.StartAnimation(this).Then(() => visible.SetVisible(false)));
    }
    private void PlayExitPipeAnim()
    {
        visible.SetVisible(true);
        StartAnimation(() => exitPipeAnim.StartAnimation(this));
    }

    internal override void StopAnimation()
    {
        enterPipeAnim.StopAnimation(this);
        exitPipeAnim.StopAnimation(this);
    }
}
