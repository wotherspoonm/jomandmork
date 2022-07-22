using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementTile : MonoBehaviour
{
    public Material defaultMaterial;
    public Material glowingMaterial;
    new Renderer renderer;
    private bool objectPlaced = false;
    private float distanceFromGrid = 2f;
    public GameObject placedObject;
    [CanBeNull] private GameObject objectToPlace;

    private void Start() {
        renderer = GetComponent<Renderer>();
        renderer.material = defaultMaterial;
    }
    private void OnMouseOver() {
        renderer.material = glowingMaterial;
        if (Input.GetKeyDown(KeyCode.Mouse0)) // needs to change when KeybindManager is fixed
        {
            if (!objectPlaced) {
                placeObject(false, PlacementMode.Creating);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1)) {
            if (objectPlaced) {
                Destroy(placedObject);
                objectPlaced = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse2)) {
            if (objectPlaced) {
                PlacementManager.Instance.SetStoredObject(placedObject, PlacementMode.Moving);
                objectPlaced = false;
            }
            else {
                placeObject(true, PlacementMode.Moving);
            }
        }
    }
    private void OnMouseExit() {
        renderer.material = defaultMaterial;
    }
    private void placeObject(bool destroyObject, PlacementMode desiredMode)
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition += new Vector3(0, 0, distanceFromGrid);
        Quaternion spawnRotation = new();
        objectToPlace = PlacementManager.Instance.GetStoredObject(desiredMode);
        if (objectToPlace != null) {
            placedObject = Instantiate(objectToPlace, spawnPosition, spawnRotation);
            objectPlaced = true;
        }

        if (destroyObject)
        {
            Destroy(objectToPlace);
        }
    }
}