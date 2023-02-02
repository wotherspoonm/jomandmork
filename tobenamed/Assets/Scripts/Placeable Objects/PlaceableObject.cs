using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class PlaceableObject : MonoBehaviour 
{
    public PlaceableObjectData data;

    public virtual void Select() {
        gameObject.GetComponent<Renderer>().material = data.selectedMaterial;
    }
    public virtual void Deselect() {
        gameObject.GetComponent<Renderer>().material = data.material;
    }
}
