using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : PlaceableObject
{
    [SerializeField]
    private Vector2 _surfaceNormal;
    protected override void Start() {
        base.Start();
        _inspectorFields.Add(new SliderInspectorField(GetAngle,SetAngle,"Angle",0, 270, 90));
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out Astronaut _ast)) {
            _ast.ReflectAbout(_surfaceNormal);
        }
    }
    public override void Highlight() {

    }
    public override void ResetVisuals() {

    }
    public override void ShowAsGhost() {
        
    }
    public float GetAngle() { 
        return Mathf.Atan2(_surfaceNormal.x, _surfaceNormal.y);
    }

    public void SetAngle(float angle) {
        transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
        // Convert angle to radians and ofset by 45 deg (This probably needs to be refactored)
        angle += 45;
        angle *= Mathf.Deg2Rad;
        _surfaceNormal = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }


}
