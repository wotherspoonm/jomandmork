using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable Object", menuName = "Placeable Object")]
public class PlaceableObjectData : ScriptableObject
{
    public Material material;
    public Material selectedMaterial;
    public GameObject prefab;
    public float menuPreviewSize;
}
