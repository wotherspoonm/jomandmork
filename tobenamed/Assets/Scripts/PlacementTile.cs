using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementTile : InteractableGameObject
{
    public Material defaultMaterial;
    public Material glowingMaterial;
    new Renderer renderer;
    public bool objectPlaced = false;
    private float distanceFromGrid = 2f;
    public GameObject ghostObject;
    public GameObject placedObject;
    [CanBeNull] private GameObject objectToPlace;

    private void Start() {
        renderer = GetComponent<Renderer>();
        renderer.material = defaultMaterial;
    }
    protected override void OnMouseOver() {
        base.OnMouseOver();
        renderer.material = glowingMaterial;
    }

    private void OnMouseEnter()
    {

    }
    private void OnMouseExit() {
        renderer.material = defaultMaterial;
    }
    /*
    private GameObject CreateObject()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition += new Vector3(0, 0, distanceFromGrid);
        Quaternion spawnRotation = new();
        if (objectToPlace != null)
        {
            Transform objectParent = objectToPlace.transform.parent;
            objectToPlace.transform.SetParent(null,true);
            GameObject result = Instantiate(objectToPlace, spawnPosition, spawnRotation);
            objectToPlace.transform.SetParent(objectParent,true);
            result.transform.SetParent(transform,true);
            return result;
        }

        return null;

    }
    */
}