using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementTile : InteractableGameObject
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material glowingMaterial;

    new Renderer renderer;
    private bool objectPlaced = false;
    private bool ghostObjectSpawned = false;
    private PlaceableObject ghostObject;
    private PlaceableObject placedObject;
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

    public void SpawnGhostObject(PlaceableObjectSO placeableObjectSO) {
        if (!objectPlaced) {
            ghostObject = CloneObject(placeableObjectSO.prefab.GetComponent<PlaceableObject>());

            Quaternion newRotation = new();
            ghostObject.transform.rotation = newRotation;

            ghostObject.ShowAsGhost();
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

    public void PlaceObject(PlaceableObjectSO placeableObjectSO) {
        if (!objectPlaced) {
            placedObject = CloneObject(placeableObjectSO.prefab.GetComponent<PlaceableObject>());

            Quaternion newRotation = new();
            placedObject.transform.rotation = newRotation;

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

    private PlaceableObject CloneObject(PlaceableObject objectToClone) {
        GameObject result = Instantiate(objectToClone.gameObject);

        Vector3 spawnPosition = transform.position;
        spawnPosition += new Vector3(0, 0, distanceFromGrid);
        result.transform.position = spawnPosition;

        result.SetActive(true);
        return result.GetComponent<PlaceableObject>();
    }

    public bool IsObjectPlaced() {
        return objectPlaced;
    }
}