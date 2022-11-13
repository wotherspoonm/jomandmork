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
    private readonly float gravity;
    private float k1;
    private float k2;
    private float k3;
    public Vector3 Value { get { return value; } }
    public Vector3 TargetValue { get { return targetValue; } set { targetValue = value; } }
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
    /// <summary>
    /// </summary>
    /// <param name="targetValue">The value that the differential equation will try to reach</param>
    /// <param name="frequency">The frequency at which the undamped system will oscillate</param>
    /// <param name="damping">The amount of viscous damping that the system experiences. A value of 1 makes the system critically damped, providing the fastest non-oscillatory response. A value less than 1 makes the system underdamped and the response will be oscillatory. A value greater than one will provide a slow, non-oscillatory response.</param>
    /// <param name="reactiveness">How quickly the system responds to a change in target value</param>
    /// <param name="gravity">Accelleration due to gravity on the system</param>
    public SmoothVector(Vector3 targetValue, float frequency, float damping, float reactiveness, float gravity = 0) {
        k1 = damping / (Mathf.PI * frequency);
        k2 = Mathf.Pow((2 * Mathf.PI * frequency),-2);
        k3 = reactiveness * damping / (2 * Mathf.PI * frequency);
        this.targetValue = targetValue;
        this.gravity = gravity;
        prevTargetValue = targetValue;
        value = targetValue;
        velocity = Vector3.zero;
        accelleration = Vector3.zero;
    }
    /// <summary>
    /// Changes the response characteristics of the differential system.
    /// </summary>
    /// <param name="frequency">The frequency at which the undamped system will oscillate</param>
    /// <param name="damping">The amount of viscous damping that the system experiences. A value of 1 makes the system critically damped, providing the fastest non-oscillatory response. A value less than 1 makes the system underdamped and the response will be oscillatory. A value greater than one will provide a slow, non-oscillatory response.</param>
    /// <param name="reactiveness">How quickly the system responds to a change in target value</param>
    public void ChangeProperties(float frequency, float damping, float reactiveness) {
        k1 = damping / (Mathf.PI * frequency);
        k2 = Mathf.Pow((2 * Mathf.PI * frequency), -2);
        k3 = reactiveness * damping / (2 * Mathf.PI * frequency);
    }

    public void TimeStep(float deltaTime) {
        value += velocity * deltaTime;
        Vector3 targetVelocity = (targetValue - prevTargetValue) / deltaTime;
        accelleration = gravity * Mathf.Sign(value.y) * Vector3.down + (targetValue + k3 * targetVelocity - value - k1 * velocity) / k2;
        velocity += accelleration * deltaTime;
        prevTargetValue = targetValue;
    }
}
