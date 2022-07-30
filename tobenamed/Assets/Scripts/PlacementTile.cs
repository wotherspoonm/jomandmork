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
    public GameObject placedObject;
    public static float distanceFromGrid = 2f;
    private void Start() {
        renderer = GetComponent<Renderer>();
        renderer.material = defaultMaterial;
    }
    void OnMouseOver() {
        base.Interact();
        renderer.material = glowingMaterial;
    }


    private void OnMouseExit() {
        renderer.material = defaultMaterial;
    }

    public void PlaceObject(GameObject objectToPlace)
    {
        placedObject = objectToPlace;
        placedObject.transform.position = transform.position;
        placedObject.transform.position += new Vector3(0, 0, distanceFromGrid);
        objectPlaced = true;
    }

    public GameObject RemoveObject()
    {
        objectPlaced = false;
        return placedObject;
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