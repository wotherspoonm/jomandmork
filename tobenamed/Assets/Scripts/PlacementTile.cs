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
            if (!objectPlaced) {
                objectToPlace = PlacementManager.Instance.GetStoredObjectAndUpdate(PlacementMode.Creating);
                if (objectToPlace != null)
                {
                    placedObject = createObject();
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
                PlacementManager.Instance.SetMovingObject(placedObject,PlacementMode.Moving,this);
            }
            else {
                objectToPlace = PlacementManager.Instance.GetStoredObjectAndUpdate(PlacementMode.Moving);
                if (objectToPlace != null)
                {
                    placedObject = createObject();
                    Destroy(objectToPlace);
                    objectPlaced = true;
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!objectPlaced)
        {
            objectToPlace = PlacementManager.Instance.GetStoredObject();
            if (objectToPlace != null) {
                ghostObject = createObject();
                Renderer renderer = ghostObject.GetComponent<Renderer>();
                renderer.material = defaultMaterial;
            }
        }
    }
    private void OnMouseExit() {
        renderer.material = defaultMaterial;
        Destroy(ghostObject);
    }
    private GameObject createObject()
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