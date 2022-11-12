using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothVector
{
    Vector3 targetValue;
    Vector3 prevTargetValue;
    Vector3 value;
    Vector3 velocity;
    Vector3 accelleration;
    private float k1;
    private float k2;
    private float k3;
    public Vector3 Value { get { return value; } }
    public Vector3 TargetValue { get { return targetValue; } set { targetValue = value; } }
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }

    public SmoothVector(Vector3 targetValue, float frequency, float damping, float reactiveness) {
        k1 = damping / (Mathf.PI * frequency);
        k2 = Mathf.Pow((2 * Mathf.PI * frequency),-2);
        k3 = reactiveness * damping / (2 * Mathf.PI * frequency);
        this.targetValue = targetValue;
        prevTargetValue = targetValue;
        value = targetValue;
        velocity = Vector3.zero;
        accelleration = Vector3.zero;
    }

    public void ChangeProperties(float frequency, float damping, float reactiveness) {
        k1 = damping / (Mathf.PI * frequency);
        k2 = Mathf.Pow((2 * Mathf.PI * frequency), -2);
        k3 = reactiveness * damping / (2 * Mathf.PI * frequency);
    }

    public void TimeStep(float deltaTime) {
        value += velocity * deltaTime;
        Vector3 targetVelocity = (targetValue - prevTargetValue) / deltaTime;
        accelleration = (targetValue + k3 * targetVelocity - value - k1 * velocity) / k2;
        velocity += accelleration * deltaTime;
        prevTargetValue = targetValue;
    }
}
