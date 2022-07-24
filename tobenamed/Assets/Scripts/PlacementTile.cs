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
    public bool objectPlaced = false;
    private float distanceFromGrid = 2f;
    public GameObject ghostObject;
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
            if (!objectPlaced && PlacementManager.Instance.mode == PlacementMode.Create)
            {
                objectToPlace = PlacementManager.Instance.GetStoredObject();
                if (objectToPlace != null)
                {
                    placedObject = CreateObject();
                    objectPlaced = true;
                }
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
                PlacementManager.Instance.TransitionToMove(placedObject,this);
            }
            else
            {
                if (PlacementManager.Instance.mode == PlacementMode.Move)
                {
                    objectToPlace = PlacementManager.Instance.GetStoredObject();
                    PlacementManager.Instance.TransitionToNoMode();
                    if (objectToPlace != null) {
                        placedObject = CreateObject();
                        Destroy(objectToPlace);
                        objectPlaced = true;
                    }
                }
            }
        }
        PlacementManager.Instance.MoveObject();
    }

    private void OnMouseEnter()
    {
        if (!objectPlaced)
        {
            objectToPlace = PlacementManager.Instance.GetStoredObject();
            if (objectToPlace != null) {
                ghostObject = CreateObject();
                Renderer renderer = ghostObject.GetComponent<Renderer>();
                renderer.material = defaultMaterial;
            }
        }
    }
    private void OnMouseExit() {
        renderer.material = defaultMaterial;
        Destroy(ghostObject);
    }
    private GameObject CreateObject()
    {
        GameObject result;
        Vector3 spawnPosition = transform.position;
        spawnPosition += new Vector3(0, 0, distanceFromGrid);
        Quaternion spawnRotation = new();
        if (objectToPlace != null) {
            result = Instantiate(objectToPlace, spawnPosition, spawnRotation);
            return result;
        }

        return null;

    }
}