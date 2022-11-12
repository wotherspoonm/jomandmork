using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFloat {
    float targetValue;
    float prevTargetValue;
    float value;
    float velocity;
    float accelleration;
    private float k1;
    private float k2;
    private float k3;
    public float Value { get { return value; } }
    public float Velocity { get { return velocity; } }
    public float TargetValue { get { return targetValue; } set { targetValue = value; } }

    public SmoothFloat(float targetValue, float frequency, float damping, float reactiveness) {
        k1 = damping / (Mathf.PI * frequency);
        k2 = Mathf.Pow((2 * Mathf.PI * frequency), -2);
        k3 = reactiveness * damping / (2 * Mathf.PI * frequency);
        this.targetValue = targetValue;
        prevTargetValue = targetValue;
        value = targetValue;
        velocity = 0;
        accelleration = 0;
    }

    public void ChangeDynamics(float frequency, float damping, float reactiveness) {
        k1 = damping / (Mathf.PI * frequency);
        k2 = Mathf.Pow((2 * Mathf.PI * frequency), -2);
        k3 = reactiveness * damping / (2 * Mathf.PI * frequency);
    }

    public void TimeStep(float deltaTime) {
        value += velocity * deltaTime;
        float targetVelocity = (targetValue - prevTargetValue) / deltaTime;
        accelleration = (targetValue + k3 * targetVelocity - value - k1 * velocity) / k2;
        velocity += accelleration * deltaTime;
        prevTargetValue = targetValue;
    }
}
