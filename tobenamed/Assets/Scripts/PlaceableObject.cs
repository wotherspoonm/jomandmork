using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class PlaceableObject : MonoBehaviour 
{
    public Material material;
    public Material selectedMaterial;

    public void Select() {
        gameObject.GetComponent<Renderer>().material = selectedMaterial;
    }
    public void Deselect() {
        gameObject.GetComponent<Renderer>().material = material;
    }
}
