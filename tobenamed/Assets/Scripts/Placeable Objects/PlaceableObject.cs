using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class PlaceableObject : MonoBehaviour 
{
    public PlaceableObjectData data;

    public void Select() {
        gameObject.GetComponent<Renderer>().material = data.material;
    }
    public void Deselect() {
        gameObject.GetComponent<Renderer>().material = data.selectedMaterial;
    }
}
