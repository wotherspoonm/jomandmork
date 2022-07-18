using System.Collections;
using System.Collections.Generic;
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
    public GameObject testPrefab;

    private void Start() {
        renderer = GetComponent<Renderer>();
        renderer.material = defaultMaterial;
    }
    private void OnMouseOver() {
        renderer.material = glowingMaterial;
        if (Input.GetKey(KeyCode.Mouse0)) // needs to change when KeybindManager is fixed
        {
            if (!objectPlaced) {
                Vector3 spawnPosition = transform.position;
                spawnPosition += new Vector3(0, 0, distanceFromGrid);
                Quaternion spawnRotation = new();
                placedObject = Instantiate(testPrefab, spawnPosition, spawnRotation);
                objectPlaced = true;
            }
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            if (objectPlaced)
            {
                Destroy(placedObject);
                objectPlaced = false;
            }
        }
    }
    private void OnMouseExit() {
        renderer.material = defaultMaterial;
    }
}