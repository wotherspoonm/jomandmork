using System;
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
    public bool ghostObjectSpawned = false;
    public PlaceableObject ghostObject;
    public PlaceableObject placedObject;
    public static float distanceFromGrid = 2f;

    public event EventHandler OnTileMouseEnter;

    private void Start() {
        renderer = GetComponent<Renderer>();
        renderer.material = defaultMaterial;
    }
    void OnMouseEnter() {
        renderer.material = glowingMaterial;
        if (!objectPlaced) {
            OnTileMouseEnter?.Invoke(this, EventArgs.Empty);
        }
    }

    void OnMouseOver() {
        base.Interact();
    }

    private void OnMouseExit() {
        renderer.material = defaultMaterial;
        if (ghostObjectSpawned) {
            Destroy(ghostObject.gameObject);
            ghostObjectSpawned = false;
        }
    }

    public void SpawnGhostObject(PlaceableObject placeableObject) {
        if (!objectPlaced) {
            ghostObject = CloneObject(placeableObject);
            ghostObject.GetComponent<Renderer>().material = placeableObject.data.ghostMaterial;
            ghostObjectSpawned = true;
        }
    }

    public void PlaceObject(PlaceableObject placeableObject) {
        if (!objectPlaced) {
            placedObject = CloneObject(placeableObject);
            if (ghostObjectSpawned) {
                Destroy(ghostObject.gameObject);
                ghostObjectSpawned = false;
            }
            objectPlaced = true;
        }
    }

    public PlaceableObject RemoveObject()
    {
        objectPlaced = false;
        return placedObject;
    }

    public PlaceableObject CloneObject(PlaceableObject objectToClone) {
        Vector3 spawnPosition = transform.position;
        spawnPosition += new Vector3(0, 0, distanceFromGrid);
        Quaternion spawnRotation = new();
        GameObject result = Instantiate(objectToClone.gameObject, spawnPosition, spawnRotation);
        result.SetActive(true);
        return result.GetComponent<PlaceableObject>();
    }
}