using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable Object", menuName = "Placeable Object")]
public class PlaceableObjectSO : ScriptableObject
{
    public Material material;
    public Material selectedMaterial;
    public Material ghostMaterial;
    public GameObject prefab;
    public float menuPreviewSize;
}
