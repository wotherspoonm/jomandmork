using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : PlaceableObject
{
    [SerializeField]
    private Vector2 surfaceNormal = new Vector2(1, 1);
    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Bumper!");
        Astronaut ast;
        if(collision.TryGetComponent(out ast)) {
            ast.ReflectAbout(surfaceNormal);
        }
    }
    public override void Select() {

    }
    public override void Deselect() {

    }


}
