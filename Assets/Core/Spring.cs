using System;
using UnityEngine;

public static class Spring
{
    public static float GetSpring(float offsetFromTarget, float velocity, float strength, float damping) => offsetFromTarget * strength - damping * velocity;
}

[Serializable]
public class FloatSpringComponent
{
    public float target;
    public float currentPosition;
    public float velocity;
    public float strength;
    public float damping;

    public float GetSpring() => Spring.GetSpring(target - currentPosition, velocity, strength, damping);
    
    public FloatSpringComponent(float target, float currentPosition, float velocity, float strength, float damping)
    {
        this.target = target;
        this.currentPosition = currentPosition;
        this.velocity = velocity;
        this.strength = strength;
        this.damping = damping;
    }
}