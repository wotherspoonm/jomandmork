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
    public GameObject placedObject;
    public GameObject testPrefab;

    private void Start() {
        renderer = GetComponent<Renderer>();
        renderer.material = defaultMaterial;
    }
    private void OnMouseOver() {
        renderer.material = glowingMaterial;
    }
    private void OnMouseExit() {
        renderer.material = defaultMaterial;
    }

    private void OnMouseDown() {
        if (!objectPlaced) {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = new();
            _ = Instantiate(testPrefab, spawnPosition, spawnRotation);
            objectPlaced = true;
        }
    }
}