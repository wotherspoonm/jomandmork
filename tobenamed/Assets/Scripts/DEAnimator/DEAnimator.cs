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
    private Action onUpdate;
    readonly float positionalTolerance = 0.01f; // How close to the target before simulation is stopped
    readonly float velocimetricTolerance = 0.01f;
    readonly float rotationalTolerance = 0.01f;
    readonly float angularVelocimetricTolerance = 0.01f;
    private Vector3 spinSpeed;
    private Vector3 snappingAngles;

    public Vector3 dispTV;

    private void Start() {
        positionSV = new(transform.position, 1, 1, -1f);
        rotationSV = new(transform.rotation.eulerAngles, 2, 1, 0);
        snappingAngles = transform.rotation.eulerAngles;
    }
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

    public void SetSpin(Vector3 spinSpeed) {
        this.spinSpeed = spinSpeed;
        onUpdate -= UpdateRotation; // This is to prevent duplicate calls. 
        onUpdate += UpdateRotation;
    }

    public void SetAngle(Vector3 angle) {
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        Debug.Log(rotationSV.TargetValue);
        Debug.Log(rotationSV.TargetValue - RestrictAngle(rotationSV.TargetValue, snappingAngles));
        Debug.Log(angle + eulerAngles - RestrictAngle(eulerAngles, snappingAngles));
        rotationSV.TargetValue = angle + rotationSV.TargetValue - RestrictAngle(rotationSV.TargetValue, snappingAngles);

        onUpdate -= UpdateRotation; // This is to prevent duplicate calls. 
        onUpdate += UpdateRotation;
    }

    private void UpdateRotation() {
        rotationSV.TargetValue += spinSpeed * TimeStep();
        rotationSV.TimeStep(TimeStep());
        transform.rotation = Quaternion.Euler(rotationSV.Value);

        // If the object is close to its target angle and not rotating quickly, it is assumed to be at rest and no longer needs to be simulated
        if (Vector3.Distance(rotationSV.TargetValue, transform.rotation.eulerAngles) < rotationalTolerance && rotationSV.Velocity.magnitude < angularVelocimetricTolerance) {
            transform.rotation = Quaternion.Euler(positionSV.TargetValue);
            onUpdate -= UpdateRotation;
        }
    }

    private void UpdatePosition() {
        positionSV.TimeStep(TimeStep());
        transform.position = positionSV.Value;

        // If the object is close to its target and not moving quickly, it is assumed to be at rest and no longer needs to be simulated
        if(Vector3.Distance(positionSV.TargetValue, transform.position) < positionalTolerance && positionSV.Velocity.magnitude < velocimetricTolerance) {
            transform.position = positionSV.TargetValue;
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
        dispTV = rotationSV.TargetValue;
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

    private Vector3 RestrictAngle(Vector3 v, Vector3 snappingLine) {
        return new Vector3(RestrictAngle(v.x,snappingLine.x),RestrictAngle(v.y,snappingLine.y),RestrictAngle(v.z,snappingLine.z));
    }

}
