using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class DEAnimator : MonoBehaviour
{
    [SerializeField] 
    private bool physicsTime = false;
    private SmoothVector positionSV;
    private SmoothVector rotationSV;
    private SmoothVector bounceSV;
    private Action onUpdate;
    readonly float positionalTolerance = 0.01f; // How close to the target before simulation is stopped
    readonly float velocimetricTolerance = 0.01f;
    readonly float rotationalTolerance = 0.01f;
    readonly float angularVelocimetricTolerance = 0.01f;
    [SerializeField]
    bool useRectTransform = false;

    private Vector3 spinSpeed;
    private Vector3 snappingAngles;

    private void Awake() {
        positionSV = new(Position, 1, 1, -1f);
        rotationSV = new(transform.rotation.eulerAngles, 2, 1, 0);
        bounceSV = new(Vector3.zero,0.001f,50f,1, 10f);
        snappingAngles = transform.rotation.eulerAngles;
    }

    public void SetPositionParameters(float frequency, float damping, float reactiveness) =>
        positionSV.ChangeProperties(frequency, damping, reactiveness);

    public void SetRotationParameters(float frequency, float damping, float reactiveness) =>
        rotationSV.ChangeProperties(frequency, damping, reactiveness);

    public void MoveTo(Vector3 targetLocation) {
        positionSV.TargetValue = targetLocation;
        onUpdate -= UpdatePosition; // This is to prevent duplicate UpdatePosition calls. 
        onUpdate += UpdatePosition;
    }
    public void Bump(Vector3 velocity) {
        positionSV.Velocity += velocity;
        onUpdate -= UpdatePosition; // This is to prevent duplicate UpdatePosition calls. 
        onUpdate += UpdatePosition;
    }
    /// <summary>
    /// Bounce the object in the vertical direction
    /// </summary>
    /// <param name="speed">Initial bounce speed</param>
    public void Bounce(float speed) {
        bounceSV.Velocity += speed * Vector3.up;
        onUpdate -= UpdatePosition; // This is to prevent duplicate UpdatePosition calls. 
        onUpdate += UpdatePosition;
    }

    public void SetSpin(Vector3 spinSpeed) {
        this.spinSpeed = spinSpeed;
        onUpdate -= UpdateRotation; // This is to prevent duplicate calls. 
        onUpdate += UpdateRotation;
    }

    public void SetSpin(float x, float y = 0, float z = 0) => SetSpin(new(x, y, z));

    public void SetAngle(Vector3 angle) {
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        rotationSV.TargetValue = angle + rotationSV.TargetValue - RestrictAngle(rotationSV.TargetValue, snappingAngles);

        onUpdate -= UpdateRotation; // This is to prevent duplicate calls. 
        onUpdate += UpdateRotation;
    }

    private void UpdateRotation() {
        rotationSV.TargetValue += spinSpeed * TimeStep();
        rotationSV.TimeStep(TimeStep());
        transform.rotation = Quaternion.Euler(rotationSV.Value);

        // If the object is close to its target angle and not rotating quickly, it is assumed to be at rest and no longer needs to be simulated
        if (Vector3.Distance(RestrictAngle(rotationSV.TargetValue, snappingAngles), RestrictAngle(rotationSV.TargetValue, transform.rotation.eulerAngles)) < rotationalTolerance && rotationSV.Velocity.magnitude < angularVelocimetricTolerance) {
            onUpdate -= UpdateRotation;
            transform.rotation = Quaternion.Euler(rotationSV.TargetValue);
        }
    }

    private void UpdatePosition() {
        positionSV.TimeStep(TimeStep());
        bounceSV.TimeStep(TimeStep());
        Position = positionSV.Value + new Vector3(0,Mathf.Abs(bounceSV.Value.y),0);

        float distanceFromTargetPosition = Vector3.Distance(positionSV.TargetValue, Position);
        float velocity = (positionSV.Velocity + bounceSV.Velocity).magnitude;
        // If the object is close to its target and not moving quickly, it is assumed to be at rest and no longer needs to be simulated
        if (distanceFromTargetPosition < positionalTolerance &&  velocity< velocimetricTolerance) {
            Position = positionSV.TargetValue;
            onUpdate -= UpdatePosition;
        }
    }

    private void FixedUpdate() {
        // Only run if script is set to run on physics update
        if (physicsTime) onUpdate?.Invoke();
    }
    private void Update() {
        // Only run if script is not set to run on physics update
        if (!physicsTime) onUpdate?.Invoke();
    }

    private float TimeStep() => physicsTime ? Time.fixedDeltaTime : Time.deltaTime;
    private float RestrictAngle(float x, float snappingLine) {
        var output = (x - snappingLine) % 360 + snappingLine;
        if (output < -180) {
            output += 360;
        } else if(output > 180) {
            output -= 360;
        }
        return output;
    }

    private Vector3 Position {
        get {
            return useRectTransform ? GetComponent<RectTransform>().anchoredPosition3D : transform.localPosition;
        }
        set {
            if (useRectTransform) {
                GetComponent<RectTransform>().anchoredPosition3D = value;
            }
            else transform.localPosition = value;
        }
    }
    private Vector3 RestrictAngle(Vector3 v, Vector3 snappingLine) {
        return new Vector3(RestrictAngle(v.x,snappingLine.x),RestrictAngle(v.y,snappingLine.y),RestrictAngle(v.z,snappingLine.z));
    }

}