using System;
using UnityEngine;

public abstract class PlayerProceduralAnim : MonoBehaviour
{
        [SerializeField] private PlayerProceduralAnimManager playerProceduralAnimManager;

        private void StopOtherProceduralAnimations()
        {
                playerProceduralAnimManager.StopOtherProceduralAnimations(this);
        }
        protected void StartAnimation(Action onReadyToPlayCallback)
        {
                StopOtherProceduralAnimations();
                onReadyToPlayCallback?.Invoke();
        }

        internal abstract void StopAnimation();
}