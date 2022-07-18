using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementTile : MonoBehaviour
{
    public Material defaultMaterial;
    public Material glowingMaterial;


    private void OnMouseOver() {
        GetComponent<Renderer>().material = glowingMaterial;
    }
    private void OnMouseExit() {
        GetComponent<Renderer>().material = defaultMaterial;
    }
}
