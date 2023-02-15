using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : PlaceableObject
{
    [SerializeField]
    private Vector2 _surfaceNormal;
    protected override void Start() {
        base.Start();
        _inspectorFields.Add(new SliderInspectorField(GetAngle,SetAngle,"Angle",360, 0, 90));
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
        _surfaceNormal = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
    }


}
