using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementTile : MonoBehaviour
{
    Collider collider;
    public Material defaultMaterial;
    public Material glowingMaterial;

    private void Start() {
        collider = gameObject.GetComponent<Collider>();
    }

    private void OnMouseOver() {
        GetComponent<Renderer>().material = glowingMaterial;
    }
    private void OnMouseExit() {
        GetComponent<Renderer>().material = defaultMaterial;
    }
}
