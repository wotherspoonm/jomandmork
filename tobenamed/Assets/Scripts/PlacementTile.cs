using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementTile : MonoBehaviour
{
    public Material defaultMaterial;
    public Material glowingMaterial;
    new Renderer renderer;
    public GameObject placedObject;

    private void Start() {
        renderer = GetComponent<Renderer>();
        renderer.material = defaultMaterial;
    }
    private void OnMouseOver() {
        renderer.material = glowingMaterial;
    }
    private void OnMouseExit() {
        renderer.material = defaultMaterial;
    }
}