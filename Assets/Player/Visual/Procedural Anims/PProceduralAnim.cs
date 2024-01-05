using System;
using UnityEngine;

public abstract class PlayerProceduralAnim : MonoBehaviour
{
        [SerializeField] private PlayerProceduralAnimManager playerProceduralAnimManager;
        protected void StartAnimation(Action onReadyToPlayCallback)
        {
                playerProceduralAnimManager.StopAllProceduralAnimations();
                onReadyToPlayCallback?.Invoke();
        }

        internal abstract void StopAnimation();
}